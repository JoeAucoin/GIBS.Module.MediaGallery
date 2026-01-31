using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;

namespace GIBS.Module.MediaGallery.Repository
{
    public interface IMediaGalleryCategoryRepository
    {
        IEnumerable<Models.MediaGalleryCategory> GetMediaGalleryCategories(int ModuleId);
        Models.MediaGalleryCategory GetMediaGalleryCategory(int CategoryId);
        Models.MediaGalleryCategory GetMediaGalleryCategory(int CategoryId, bool tracking);
        Models.MediaGalleryCategory AddMediaGalleryCategory(Models.MediaGalleryCategory MediaGalleryCategory);
        Models.MediaGalleryCategory UpdateMediaGalleryCategory(Models.MediaGalleryCategory MediaGalleryCategory);
        void DeleteMediaGalleryCategory(int CategoryId);
    }

    public class MediaGalleryCategoryRepository : IMediaGalleryCategoryRepository, ITransientService
    {
        private readonly IDbContextFactory<MediaGalleryContext> _factory;

        public MediaGalleryCategoryRepository(IDbContextFactory<MediaGalleryContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Models.MediaGalleryCategory> GetMediaGalleryCategories(int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            return db.MediaGalleryCategory.Where(item => item.ModuleId == ModuleId).ToList();
        }

        public Models.MediaGalleryCategory GetMediaGalleryCategory(int CategoryId)
        {
            return GetMediaGalleryCategory(CategoryId, true);
        }

        public Models.MediaGalleryCategory GetMediaGalleryCategory(int CategoryId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.MediaGalleryCategory.Find(CategoryId);
            }
            else
            {
                return db.MediaGalleryCategory.AsNoTracking().FirstOrDefault(item => item.CategoryId == CategoryId);
            }
        }

        public Models.MediaGalleryCategory AddMediaGalleryCategory(Models.MediaGalleryCategory MediaGallery)
        {
            using var db = _factory.CreateDbContext();
            db.MediaGalleryCategory.Add(MediaGallery);
            db.SaveChanges();
            return MediaGallery;
        }

        public Models.MediaGalleryCategory UpdateMediaGalleryCategory(Models.MediaGalleryCategory MediaGallery)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(MediaGallery).State = EntityState.Modified;
            db.SaveChanges();
            return MediaGallery;
        }

        public void DeleteMediaGalleryCategory(int CategoryId)
        {
            using var db = _factory.CreateDbContext();
            Models.MediaGalleryCategory MediaGalleryCategory = db.MediaGalleryCategory.Find(CategoryId);
            db.MediaGalleryCategory.Remove(MediaGalleryCategory);
            db.SaveChanges();
        }
    }
}
