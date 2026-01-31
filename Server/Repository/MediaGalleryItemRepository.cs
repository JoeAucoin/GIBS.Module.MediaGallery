using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;

namespace GIBS.Module.MediaGallery.Repository
{
    public class MediaGalleryItemRepository : IMediaGalleryItemRepository, ITransientService
    {
        private readonly IDbContextFactory<MediaGalleryContext> _factory;

        public MediaGalleryItemRepository(IDbContextFactory<MediaGalleryContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Models.MediaGalleryItem> GetMediaGalleryItems(int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            return (from i in db.MediaGalleryItem
                    join c in db.MediaGalleryCategory on i.CategoryId equals c.CategoryId into gj
                    from subcat in gj.DefaultIfEmpty()
                    where i.ModuleId == ModuleId
                    select new Models.MediaGalleryItem
                    {
                        ItemId = i.ItemId,
                        ModuleId = i.ModuleId,
                        CategoryId = i.CategoryId,
                        Title = i.Title,
                        Description = i.Description,
                        MediaType = i.MediaType,
                        FileId = i.FileId,
                        FilePath = i.FilePath,
                        ThumbnailFileId = i.ThumbnailFileId,
                        ThumbnailPath = i.ThumbnailPath,
                        SortOrder = i.SortOrder,
                        IsActive = i.IsActive,
                        ViewCount = i.ViewCount,
                        CreatedBy = i.CreatedBy,
                        CreatedOn = i.CreatedOn,
                        ModifiedBy = i.ModifiedBy,
                        ModifiedOn = i.ModifiedOn,
                        CategoryName = subcat.CategoryName
                    }).ToList();
        }

        public IEnumerable<Models.MediaGalleryAlbum> GetMediaGalleryAlbums(int ModuleId)
        {
            using var db = _factory.CreateDbContext();

            // Get all items and their category names for the module
            var itemsWithCategory = (from i in db.MediaGalleryItem
                                     join c in db.MediaGalleryCategory on i.CategoryId equals c.CategoryId
                                     where i.ModuleId == ModuleId && c.IsActive && i.CategoryId != null
                                     select new 
                                     {
                                         Item = i,
                                         Category = c
                                     })
                                     .AsEnumerable(); // Switch to client-side evaluation to allow complex grouping

            // Group by category on the client side
            var albumData = from x in itemsWithCategory
                            group x.Item by x.Category into itemGroup
                            let firstItem = itemGroup.OrderBy(i => i.SortOrder).ThenBy(i => i.ItemId).FirstOrDefault()
                            select new Models.MediaGalleryAlbum
                            {
                                CategoryId = itemGroup.Key.CategoryId,
                                CategoryName = itemGroup.Key.CategoryName,
                                ItemCount = itemGroup.Count(),
                                ThumbnailPath = firstItem.ThumbnailPath ?? firstItem.FilePath
                            };

            return albumData.ToList();
        }

        public Models.MediaGalleryItem GetMediaGalleryItem(int itemId)
        {
            return GetMediaGalleryItem(itemId, true);
        }

        public Models.MediaGalleryItem GetMediaGalleryItem(int itemId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            var query = from i in db.MediaGalleryItem
                        join c in db.MediaGalleryCategory on i.CategoryId equals c.CategoryId into gj
                        from subcat in gj.DefaultIfEmpty()
                        where i.ItemId == itemId
                        select new Models.MediaGalleryItem
                        {
                            ItemId = i.ItemId,
                            ModuleId = i.ModuleId,
                            CategoryId = i.CategoryId,
                            Title = i.Title,
                            Description = i.Description,
                            MediaType = i.MediaType,
                            FileId = i.FileId,
                            FilePath = i.FilePath,
                            ThumbnailFileId = i.ThumbnailFileId,
                            ThumbnailPath = i.ThumbnailPath,
                            SortOrder = i.SortOrder,
                            IsActive = i.IsActive,
                            ViewCount = i.ViewCount,
                            CreatedBy = i.CreatedBy,
                            CreatedOn = i.CreatedOn,
                            ModifiedBy = i.ModifiedBy,
                            ModifiedOn = i.ModifiedOn,
                            CategoryName = subcat.CategoryName
                        };

            if (!tracking)
            {
                query = query.AsNoTracking();
            }

            return query.FirstOrDefault();
        }

        public Models.MediaGalleryItem AddMediaGalleryItem(Models.MediaGalleryItem item)
        {
            using var db = _factory.CreateDbContext();
            db.MediaGalleryItem.Add(item);
            db.SaveChanges();
            return item;
        }

        public Models.MediaGalleryItem UpdateMediaGalleryItem(Models.MediaGalleryItem item)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            return item;
        }

        public void DeleteMediaGalleryItem(int itemId)
        {
            using var db = _factory.CreateDbContext();
            Models.MediaGalleryItem item = db.MediaGalleryItem.Find(itemId);
            db.MediaGalleryItem.Remove(item);
            db.SaveChanges();
        }
    }
}
