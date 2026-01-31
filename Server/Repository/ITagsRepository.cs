using System.Collections.Generic;
using System.Threading.Tasks;
using GIBS.Module.MediaGallery.Models;

namespace GIBS.Module.MediaGallery.Repository
{
    public interface ITagsRepository
    {
        Task<IEnumerable<Tags>> GetTagsAsync(int moduleId);
        Task<Tags> GetTagAsync(int tagId);
        Task<Tags> AddTagAsync(Tags tag);
        Task<Tags> UpdateTagAsync(Tags tag);
        Task DeleteTagAsync(int tagId);
    }
}
