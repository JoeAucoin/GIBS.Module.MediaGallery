using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GIBS.Module.MediaGallery.Models;

namespace GIBS.Module.MediaGallery.Repository
{
    public class ItemTagsRepository : IItemTagsRepository
    {
        private readonly MediaGalleryContext _db;

        public ItemTagsRepository(MediaGalleryContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<ItemTags>> GetItemTagsAsync(int itemId)
        {
            return await _db.ItemTags.Where(it => it.ItemId == itemId).ToListAsync();
        }

        public async Task<ItemTags> GetItemTagAsync(int itemTagId)
        {
            return await _db.ItemTags.FindAsync(itemTagId);
        }

        public async Task<ItemTags> AddItemTagAsync(ItemTags itemTag)
        {
            _db.ItemTags.Add(itemTag);
            await _db.SaveChangesAsync();
            return itemTag;
        }

        public async Task DeleteItemTagAsync(int itemTagId)
        {
            var itemTag = await _db.ItemTags.FindAsync(itemTagId);
            if (itemTag != null)
            {
                _db.ItemTags.Remove(itemTag);
                await _db.SaveChangesAsync();
            }
        }
    }
}
