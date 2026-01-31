using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Security;
using Oqtane.Shared;
using GIBS.Module.MediaGallery.Repository;

namespace GIBS.Module.MediaGallery.Services
{
    public class ServerMediaGalleryCategoryService : IMediaGalleryCategoryService
    {
        private readonly IMediaGalleryCategoryRepository _mediaGalleryCategoryRepository;
        private readonly IUserPermissions _userPermissions;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;

        public ServerMediaGalleryCategoryService(IMediaGalleryCategoryRepository mediaGalleryCategoryRepository, IUserPermissions userPermissions, ITenantManager tenantManager, ILogManager logger, IHttpContextAccessor accessor)
        {
            _mediaGalleryCategoryRepository = mediaGalleryCategoryRepository;
            _userPermissions = userPermissions;
            _logger = logger;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
        }

        public Task<List<Models.MediaGalleryCategory>> GetMediaGalleryCategoriesAsync(int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_mediaGalleryCategoryRepository.GetMediaGalleryCategories(ModuleId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized MediaGalleryCategory Get Attempt {ModuleId}", ModuleId);
                return null;
            }
        }

        public Task<Models.MediaGalleryCategory> GetMediaGalleryCategoryAsync(int categoryId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_mediaGalleryCategoryRepository.GetMediaGalleryCategory(categoryId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized MediaGalleryCategory Get Attempt {CategoryId} {ModuleId}", categoryId, ModuleId);
                return null;
            }
        }

        public Task<Models.MediaGalleryCategory> AddMediaGalleryCategoryAsync(Models.MediaGalleryCategory category)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, category.ModuleId, PermissionNames.Edit))
            {
                category = _mediaGalleryCategoryRepository.AddMediaGalleryCategory(category);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "MediaGalleryCategory Added {MediaGalleryCategory}", category);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized MediaGalleryCategory Add Attempt {MediaGalleryCategory}", category);
                category = null;
            }
            return Task.FromResult(category);
        }

        public Task<Models.MediaGalleryCategory> UpdateMediaGalleryCategoryAsync(Models.MediaGalleryCategory category)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, category.ModuleId, PermissionNames.Edit))
            {
                category = _mediaGalleryCategoryRepository.UpdateMediaGalleryCategory(category);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "MediaGalleryCategory Updated {MediaGalleryCategory}", category);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized MediaGalleryCategory Update Attempt {MediaGalleryCategory}", category);
                category = null;
            }
            return Task.FromResult(category);
        }

        public Task DeleteMediaGalleryCategoryAsync(int categoryId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.Edit))
            {
                _mediaGalleryCategoryRepository.DeleteMediaGalleryCategory(categoryId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "MediaGalleryCategory Deleted {CategoryId}", categoryId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized MediaGalleryCategory Delete Attempt {CategoryId} {ModuleId}", categoryId, ModuleId);
            }
            return Task.CompletedTask;
        }
    }
}
