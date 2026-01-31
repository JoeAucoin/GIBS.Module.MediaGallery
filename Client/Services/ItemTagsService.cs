using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Services;
using Oqtane.Shared;
using GIBS.Module.MediaGallery.Models;

namespace GIBS.Module.MediaGallery.Services
{
    public class ItemTagsService : ServiceBase, IItemTagsService
    {
        public ItemTagsService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("ItemTags");

        public async Task<List<ItemTags>> GetItemTagsAsync(int itemId)
        {
            return await GetJsonAsync<List<ItemTags>>($"{Apiurl}/?itemId={itemId}");
        }

        public async Task<ItemTags> GetItemTagAsync(int itemTagId)
        {
            return await GetJsonAsync<ItemTags>($"{Apiurl}/{itemTagId}");
        }

        public async Task<ItemTags> AddItemTagAsync(ItemTags itemTag)
        {
            return await PostJsonAsync<ItemTags>(Apiurl, itemTag);
        }

        public async Task DeleteItemTagAsync(int itemTagId)
        {
            await DeleteAsync($"{Apiurl}/{itemTagId}");
        }
    }
}
