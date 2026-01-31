using Microsoft.AspNetCore.Builder; 
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure;
using GIBS.Module.MediaGallery.Repository;
using GIBS.Module.MediaGallery.Services;

namespace GIBS.Module.MediaGallery.Startup
{
    public class ServerStartup : IServerStartup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // not implemented
        }

        public void ConfigureMvc(IMvcBuilder mvcBuilder)
        {
            // not implemented
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IMediaGalleryCategoryService, ServerMediaGalleryCategoryService>();
            services.AddTransient<IMediaGalleryItemService, ServerMediaGalleryItemService>();
            services.AddDbContextFactory<MediaGalleryContext>(opt => { }, ServiceLifetime.Transient);
            services.AddTransient<IMediaGalleryCategoryRepository, MediaGalleryCategoryRepository>();
            services.AddTransient<IMediaGalleryItemRepository, MediaGalleryItemRepository>();
        }
    }
}
