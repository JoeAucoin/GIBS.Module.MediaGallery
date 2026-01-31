using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Services;
using Oqtane.Shared;
using GIBS.Module.MediaGallery.Models;

namespace GIBS.Module.MediaGallery.Services
{
    public class TagsService : ServiceBase, ITagsService
    {
        public TagsService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("Tags");

        public async Task<List<Tags>> GetTagsAsync(int moduleId)
        {
            return await GetJsonAsync<List<Tags>>($"{Apiurl}/?moduleId={moduleId}");
        }

        public async Task<Tags> GetTagAsync(int tagId)
        {
            return await GetJsonAsync<Tags>($"{Apiurl}/{tagId}");
        }

        public async Task<Tags> AddTagAsync(Tags tag)
        {
            return await PostJsonAsync<Tags>(Apiurl, tag);
        }

        public async Task<Tags> UpdateTagAsync(Tags tag)
        {
            return await PutJsonAsync<Tags>($"{Apiurl}/{tag.TagId}", tag);
        }

        public async Task DeleteTagAsync(int tagId)
        {
            await DeleteAsync($"{Apiurl}/{tagId}");
        }
    }
}
