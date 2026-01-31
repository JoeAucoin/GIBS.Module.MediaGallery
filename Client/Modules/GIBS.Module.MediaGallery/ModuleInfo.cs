using Oqtane.Models;
using Oqtane.Modules;

namespace GIBS.Module.MediaGallery
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "MediaGallery",
            Description = "Oqtane Media Gallery",
            Version = "1.0.0",
            ServerManagerType = "GIBS.Module.MediaGallery.Manager.MediaGalleryManager, GIBS.Module.MediaGallery.Server.Oqtane",
            ReleaseVersions = "1.0.0",
            Dependencies = "GIBS.Module.MediaGallery.Shared.Oqtane",
            PackageName = "GIBS.Module.MediaGallery" 
        };
    }
}
