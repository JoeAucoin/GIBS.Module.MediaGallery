using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Security;
using Oqtane.Shared;
using GIBS.Module.MediaGallery.Models;
using GIBS.Module.MediaGallery.Repository;

namespace GIBS.Module.MediaGallery.Services
{
    public class ServerTagsService : ITagsService
    {
        private readonly ITagsRepository _tagsRepository;
        private readonly IUserPermissions _userPermissions;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;

        public ServerTagsService(ITagsRepository tagsRepository, IUserPermissions userPermissions, IHttpContextAccessor accessor, ITenantManager tenantManager)
        {
            _tagsRepository = tagsRepository;
            _userPermissions = userPermissions;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
        }

        public async Task<List<Tags>> GetTagsAsync(int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return (await _tagsRepository.GetTagsAsync(moduleId)).ToList();
            }
            else
            {
                return null;
            }
        }

        public async Task<Tags> GetTagAsync(int tagId)
        {
            var tag = await _tagsRepository.GetTagAsync(tagId);
            if (tag != null && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, tag.ModuleId, PermissionNames.View))
            {
                return tag;
            }
            else
            {
                return null;
            }
        }

        public async Task<Tags> AddTagAsync(Tags tag)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, tag.ModuleId, PermissionNames.Edit))
            {
                return await _tagsRepository.AddTagAsync(tag);
            }
            else
            {
                return null;
            }
        }

        public async Task<Tags> UpdateTagAsync(Tags tag)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, tag.ModuleId, PermissionNames.Edit))
            {
                return await _tagsRepository.UpdateTagAsync(tag);
            }
            else
            {
                return null;
            }
        }

        public async Task DeleteTagAsync(int tagId)
        {
            var tag = await _tagsRepository.GetTagAsync(tagId);
            if (tag != null && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, tag.ModuleId, PermissionNames.Edit))
            {
                await _tagsRepository.DeleteTagAsync(tagId);
            }
        }
    }
}
