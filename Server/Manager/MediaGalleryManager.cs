using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Oqtane.Modules;
using Oqtane.Models;
using Oqtane.Infrastructure;
using Oqtane.Interfaces;
using Oqtane.Enums;
using Oqtane.Repository;
using GIBS.Module.MediaGallery.Repository;
using System.Threading.Tasks;
using Oqtane.Shared;

namespace GIBS.Module.MediaGallery.Manager
{
    public class MediaGalleryManager : MigratableModuleBase, IInstallable, IPortable, ISearchable, IRoutable
    {
        private readonly IMediaGalleryCategoryRepository _MediaGalleryCategoryRepository;
        private readonly IDBContextDependencies _DBContextDependencies;
        private IMediaGalleryItemRepository _mediaGalleryItems;
        private ITagsRepository _tags;
        private IItemTagsRepository _itemTags;
        private ISqlRepository _sql;

        public MediaGalleryManager(IMediaGalleryCategoryRepository MediaGalleryCategoryRepository, IDBContextDependencies DBContextDependencies, IMediaGalleryItemRepository mediaGalleryItems, ITagsRepository tags, IItemTagsRepository itemTags, ISqlRepository sql)
        {
            _MediaGalleryCategoryRepository = MediaGalleryCategoryRepository;
            _DBContextDependencies = DBContextDependencies;
            _mediaGalleryItems = mediaGalleryItems;
            _tags = tags;
            _itemTags = itemTags;
            _sql = sql;
        }

        public bool Install(Tenant tenant, string version)
        {
            return Migrate(new MediaGalleryContext(_DBContextDependencies), tenant, MigrationType.Up);
        }

        public bool Uninstall(Tenant tenant)
        {
            return Migrate(new MediaGalleryContext(_DBContextDependencies), tenant, MigrationType.Down);
        }

        public string ExportModule(Oqtane.Models.Module module)
        {
            string content = "";
            List<Models.MediaGalleryCategory> MediaGallerys = _MediaGalleryCategoryRepository.GetMediaGalleryCategories(module.ModuleId).ToList();
            if (MediaGallerys != null)
            {
                content = JsonSerializer.Serialize(MediaGallerys);
            }
            return content;
        }

        public void ImportModule(Oqtane.Models.Module module, string content, string version)
        {
            List<Models.MediaGalleryCategory> MediaGallerys = null;
            if (!string.IsNullOrEmpty(content))
            {
                MediaGallerys = JsonSerializer.Deserialize<List<Models.MediaGalleryCategory>>(content);
            }
            if (MediaGallerys != null)
            {
                foreach(var MediaGallery in MediaGallerys)
                {
                    _MediaGalleryCategoryRepository.AddMediaGalleryCategory(new Models.MediaGalleryCategory { ModuleId = module.ModuleId, CategoryName = MediaGallery.CategoryName });
                }
            }
        }

        public Task<List<SearchContent>> GetSearchContentsAsync(PageModule pageModule, DateTime lastIndexedOn)
        {
           var searchContentList = new List<SearchContent>();

           foreach (var MediaGallery in _MediaGalleryCategoryRepository.GetMediaGalleryCategories(pageModule.ModuleId))
           {
               if (MediaGallery.ModifiedOn >= lastIndexedOn)
               {
                   searchContentList.Add(new SearchContent
                   {
                       EntityName = "GIBSMediaGallery",
                       EntityId = MediaGallery.CategoryId.ToString(),
                       Title = MediaGallery.CategoryName,
                       Body = MediaGallery.CategoryName,
                       ContentModifiedBy = MediaGallery.ModifiedBy,
                       ContentModifiedOn = MediaGallery.ModifiedOn
                   });
               }
           }

           return Task.FromResult(searchContentList);
        }

        public Dictionary<string, string> GetRoutes()
        {
            return new Dictionary<string, string>
            {
                { "album/{categoryid}", "{categoryid}" },
                { "default", "" }
            };
        }

        public string GetUrl(string route, Dictionary<string, string> parameters)
        {
            var url = "";
            if (route == "album/{categoryid}")
            {
                url = "album/" + parameters["categoryid"];
            }
            return url;
        }
    }
}
