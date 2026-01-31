using System.Collections.Generic;
using System.Threading.Tasks;
using GIBS.Module.MediaGallery.Models;

namespace GIBS.Module.MediaGallery.Repository
{
    public interface IItemTagsRepository
    {
        Task<IEnumerable<ItemTags>> GetItemTagsAsync(int itemId);
        Task<ItemTags> GetItemTagAsync(int itemTagId);
        Task<ItemTags> AddItemTagAsync(ItemTags itemTag);
        Task DeleteItemTagAsync(int itemTagId);
    }
}
