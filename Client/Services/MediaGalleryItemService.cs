using GIBS.Module.MediaGallery.Models;
using Oqtane.Services;
using Oqtane.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GIBS.Module.MediaGallery.Services
{
    public interface IMediaGalleryItemService
    {
        Task<List<MediaGalleryAlbum>> GetMediaGalleryAlbumsAsync(int ModuleId);
        Task<List<MediaGalleryItem>> GetMediaGalleryItemsAsync(int ModuleId);
        Task<MediaGalleryItem> GetMediaGalleryItemAsync(int itemId, int ModuleId);
        Task<MediaGalleryItem> AddMediaGalleryItemAsync(MediaGalleryItem item);
        Task<MediaGalleryItem> UpdateMediaGalleryItemAsync(MediaGalleryItem item);
        Task DeleteMediaGalleryItemAsync(int itemId, int ModuleId);
        Task<ThumbnailResponse> CreateImageThumbnailAsync(int fileId, int width, int height, int moduleId);
    }

    public class MediaGalleryItemService : ServiceBase, IMediaGalleryItemService
    {
        private readonly HttpClient _httpClient;
        public MediaGalleryItemService(HttpClient http, SiteState siteState) : base(http, siteState) 
        {
            _httpClient = http ?? throw new ArgumentNullException(nameof(http), "HttpClient is not initialized.");
        }

        private string Apiurl => CreateApiUrl("MediaGalleryItem");
        private string AlbumApiurl => CreateApiUrl("MediaGalleryAlbum");

        public async Task<List<MediaGalleryAlbum>> GetMediaGalleryAlbumsAsync(int ModuleId)
        {
            return await GetJsonAsync<List<MediaGalleryAlbum>>(CreateAuthorizationPolicyUrl($"{AlbumApiurl}?moduleid={ModuleId}", EntityNames.Module, ModuleId));
        }

        public async Task<List<MediaGalleryItem>> GetMediaGalleryItemsAsync(int ModuleId)
        {
            List<MediaGalleryItem> items = await GetJsonAsync<List<MediaGalleryItem>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", EntityNames.Module, ModuleId));
            return items.OrderBy(item => item.SortOrder).ToList();
        }

        public async Task<MediaGalleryItem> GetMediaGalleryItemAsync(int itemId, int ModuleId)
        {
            return await GetJsonAsync<MediaGalleryItem>(CreateAuthorizationPolicyUrl($"{Apiurl}/{itemId}/{ModuleId}", EntityNames.Module, ModuleId));
        }

        public async Task<MediaGalleryItem> AddMediaGalleryItemAsync(MediaGalleryItem item)
        {
            return await PostJsonAsync<MediaGalleryItem>(CreateAuthorizationPolicyUrl($"{Apiurl}", EntityNames.Module, item.ModuleId), item);
        }

        public async Task<MediaGalleryItem> UpdateMediaGalleryItemAsync(MediaGalleryItem item)
        {
            return await PutJsonAsync<MediaGalleryItem>(CreateAuthorizationPolicyUrl($"{Apiurl}/{item.ItemId}", EntityNames.Module, item.ModuleId), item);
        }

        public async Task DeleteMediaGalleryItemAsync(int itemId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{itemId}/{ModuleId}", EntityNames.Module, ModuleId));
        }

        public async Task<ThumbnailResponse> CreateImageThumbnailAsync(int fileId, int width, int height, int moduleId)
        {
            var request = new ResizeRequest { FileId = fileId, Width = width, Height = height, ModuleId = moduleId };
            var url = CreateAuthorizationPolicyUrl($"{Apiurl}/resize-image-thumbnail", EntityNames.Module, moduleId);

            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<ThumbnailResponse>();
            }
            catch (Exception ex)
            {
                // Log or handle exceptions as needed
                Console.WriteLine($"Error creating thumbnail: {ex.Message}");
                return null;
            }
        }
    }
}
