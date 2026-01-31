using System.Collections.Generic;
using System.Threading.Tasks;
using GIBS.Module.MediaGallery.Models;

namespace GIBS.Module.MediaGallery.Services
{
    public interface IItemTagsService
    {
        Task<List<ItemTags>> GetItemTagsAsync(int itemId);
        Task<ItemTags> GetItemTagAsync(int itemTagId);
        Task<ItemTags> AddItemTagAsync(ItemTags itemTag);
        Task DeleteItemTagAsync(int itemTagId);
    }
}
