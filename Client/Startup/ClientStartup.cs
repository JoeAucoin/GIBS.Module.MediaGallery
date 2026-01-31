using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Oqtane.Services;
using GIBS.Module.MediaGallery.Services;

namespace GIBS.Module.MediaGallery.Startup
{
    public class ClientStartup : IClientStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            if (!services.Any(s => s.ServiceType == typeof(IMediaGalleryCategoryService)))
            {
                services.AddScoped<IMediaGalleryCategoryService, MediaGalleryCategoryService>();
            }
            if (!services.Any(s => s.ServiceType == typeof(IMediaGalleryItemService)))
            {
                services.AddScoped<IMediaGalleryItemService, MediaGalleryItemService>();
            }
        }
    }
}
