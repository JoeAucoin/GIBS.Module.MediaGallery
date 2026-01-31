using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Services;
using Oqtane.Shared;

namespace GIBS.Module.MediaGallery.Services     
{
    public interface IMediaGalleryCategoryService 
    {
        Task<List<Models.MediaGalleryCategory>> GetMediaGalleryCategoriesAsync(int ModuleId);

        Task<Models.MediaGalleryCategory> GetMediaGalleryCategoryAsync(int categoryId, int ModuleId);

        Task<Models.MediaGalleryCategory> AddMediaGalleryCategoryAsync(Models.MediaGalleryCategory category);

        Task<Models.MediaGalleryCategory> UpdateMediaGalleryCategoryAsync(Models.MediaGalleryCategory category);

        Task DeleteMediaGalleryCategoryAsync(int categoryId, int ModuleId);
    }

    public class MediaGalleryCategoryService : ServiceBase, IMediaGalleryCategoryService
    {
        public MediaGalleryCategoryService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("MediaGalleryCategory");

        public async Task<List<Models.MediaGalleryCategory>> GetMediaGalleryCategoriesAsync(int ModuleId)
        {
            List<Models.MediaGalleryCategory> categories = await GetJsonAsync<List<Models.MediaGalleryCategory>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", EntityNames.Module, ModuleId), Enumerable.Empty<Models.MediaGalleryCategory>().ToList());
            return categories.OrderBy(item => item.CategoryName).ToList();
        }

        public async Task<Models.MediaGalleryCategory> GetMediaGalleryCategoryAsync(int categoryId, int ModuleId)
        {
            return await GetJsonAsync<Models.MediaGalleryCategory>(CreateAuthorizationPolicyUrl($"{Apiurl}/{categoryId}/{ModuleId}", EntityNames.Module, ModuleId));
        }

        public async Task<Models.MediaGalleryCategory> AddMediaGalleryCategoryAsync(Models.MediaGalleryCategory category)
        {
            return await PostJsonAsync<Models.MediaGalleryCategory>(CreateAuthorizationPolicyUrl($"{Apiurl}", EntityNames.Module, category.ModuleId), category);
        }

        public async Task<Models.MediaGalleryCategory> UpdateMediaGalleryCategoryAsync(Models.MediaGalleryCategory category)
        {
            return await PutJsonAsync<Models.MediaGalleryCategory>(CreateAuthorizationPolicyUrl($"{Apiurl}/{category.CategoryId}", EntityNames.Module, category.ModuleId), category);
        }

        public async Task DeleteMediaGalleryCategoryAsync(int categoryId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{categoryId}/{ModuleId}", EntityNames.Module, ModuleId));
        }
    }
}
