using GIBS.Module.MediaGallery.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIBS.Module.MediaGallery.Repository
{
    public interface IMediaGalleryItemRepository
    {
        IEnumerable<MediaGalleryItem> GetMediaGalleryItems(int ModuleId);
        IEnumerable<MediaGalleryAlbum> GetMediaGalleryAlbums(int ModuleId);
        MediaGalleryItem GetMediaGalleryItem(int itemId);
        MediaGalleryItem GetMediaGalleryItem(int itemId, bool tracking);
        MediaGalleryItem AddMediaGalleryItem(MediaGalleryItem item);
        MediaGalleryItem UpdateMediaGalleryItem(MediaGalleryItem item);
        void DeleteMediaGalleryItem(int itemId);
        //CreateImageThumbnailAsync
      //  Task<Oqtane.Models.File> CreateImageThumbnailAsync(int fileId, int itemId, int width, int height, int moduleId);
    }
}
