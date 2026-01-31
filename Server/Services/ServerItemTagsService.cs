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
    public class ServerItemTagsService : IItemTagsService
    {
        private readonly IItemTagsRepository _itemTagsRepository;
        private readonly IMediaGalleryItemRepository _itemRepository;
        private readonly IUserPermissions _userPermissions;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;

        public ServerItemTagsService(IItemTagsRepository itemTagsRepository, IMediaGalleryItemRepository itemRepository, IUserPermissions userPermissions, IHttpContextAccessor accessor, ITenantManager tenantManager)
        {
            _itemTagsRepository = itemTagsRepository;
            _itemRepository = itemRepository;
            _userPermissions = userPermissions;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
        }

        public async Task<List<ItemTags>> GetItemTagsAsync(int itemId)
        {
            var item = _itemRepository.GetMediaGalleryItem(itemId);
            if (item != null && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, item.ModuleId, PermissionNames.View))
            {
                return (await _itemTagsRepository.GetItemTagsAsync(itemId)).ToList();
            }
            else
            {
                return null;
            }
        }

        public async Task<ItemTags> GetItemTagAsync(int itemTagId)
        {
            var itemTag = await _itemTagsRepository.GetItemTagAsync(itemTagId);
            if(itemTag != null)
            {
                var item = _itemRepository.GetMediaGalleryItem(itemTag.ItemId);
                if (item != null && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, item.ModuleId, PermissionNames.View))
                {
                    return itemTag;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public async Task<ItemTags> AddItemTagAsync(ItemTags itemTag)
        {
            var item = _itemRepository.GetMediaGalleryItem(itemTag.ItemId);
            if (item != null && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, item.ModuleId, PermissionNames.Edit))
            {
                return await _itemTagsRepository.AddItemTagAsync(itemTag);
            }
            else
            {
                return null;
            }
        }

        public async Task DeleteItemTagAsync(int itemTagId)
        {
            var itemTag = await _itemTagsRepository.GetItemTagAsync(itemTagId);
            if (itemTag != null)
            {
                var item = _itemRepository.GetMediaGalleryItem(itemTag.ItemId);
                if (item != null && _userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, item.ModuleId, PermissionNames.Edit))
                {
                    await _itemTagsRepository.DeleteItemTagAsync(itemTagId);
                }
            }
        }
    }
}
