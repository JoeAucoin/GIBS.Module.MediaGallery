using GIBS.Module.MediaGallery.Models;
using GIBS.Module.MediaGallery.Repository;
using Microsoft.AspNetCore.Http;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Repository;
using Oqtane.Security;
using Oqtane.Services;
using Oqtane.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIBS.Module.MediaGallery.Services
{
    public class ServerMediaGalleryItemService : IMediaGalleryItemService
    {
        private readonly IMediaGalleryItemRepository _mediaGalleryItemRepository;
        private readonly IUserPermissions _userPermissions;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;
        private readonly IFileRepository _files;
        private readonly IImageService _imageService;

        public ServerMediaGalleryItemService(IMediaGalleryItemRepository mediaGalleryItemRepository, IUserPermissions userPermissions, ITenantManager tenantManager, ILogManager logger, IHttpContextAccessor accessor, IFileRepository files, IImageService imageService)
        {
            _mediaGalleryItemRepository = mediaGalleryItemRepository;
            _userPermissions = userPermissions;
            _logger = logger;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
            _files = files;
            _imageService = imageService;
        }

        public Task<List<Models.MediaGalleryAlbum>> GetMediaGalleryAlbumsAsync(int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_mediaGalleryItemRepository.GetMediaGalleryAlbums(ModuleId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized MediaGalleryAlbum Get Attempt {ModuleId}", ModuleId);
                return null;
            }
        }

        public Task<List<Models.MediaGalleryItem>> GetMediaGalleryItemsAsync(int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_mediaGalleryItemRepository.GetMediaGalleryItems(ModuleId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized MediaGalleryItem Get Attempt {ModuleId}", ModuleId);
                return null;
            }
        }

        public Task<Models.MediaGalleryItem> GetMediaGalleryItemAsync(int itemId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_mediaGalleryItemRepository.GetMediaGalleryItem(itemId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized MediaGalleryItem Get Attempt {ItemId} {ModuleId}", itemId, ModuleId);
                return null;
            }
        }

        public Task<Models.MediaGalleryItem> AddMediaGalleryItemAsync(Models.MediaGalleryItem item)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, item.ModuleId, PermissionNames.Edit))
            {
                item = _mediaGalleryItemRepository.AddMediaGalleryItem(item);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "MediaGalleryItem Added {MediaGalleryItem}", item);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized MediaGalleryItem Add Attempt {MediaGalleryItem}", item);
                item = null;
            }
            return Task.FromResult(item);
        }

        public Task<Models.MediaGalleryItem> UpdateMediaGalleryItemAsync(Models.MediaGalleryItem item)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, item.ModuleId, PermissionNames.Edit))
            {
                item = _mediaGalleryItemRepository.UpdateMediaGalleryItem(item);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "MediaGalleryItem Updated {MediaGalleryItem}", item);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized MediaGalleryItem Update Attempt {MediaGalleryItem}", item);
                item = null;
            }
            return Task.FromResult(item);
        }

        public Task DeleteMediaGalleryItemAsync(int itemId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.Edit))
            {
                _mediaGalleryItemRepository.DeleteMediaGalleryItem(itemId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "MediaGalleryItem Deleted {ItemId}", itemId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized MediaGalleryItem Delete Attempt {ItemId} {ModuleId}", itemId, ModuleId);
            }
            return Task.CompletedTask;
        }

        public async Task<ThumbnailResponse> CreateImageThumbnailAsync(int fileId, int width, int height, int moduleId)
        {
            if (!_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.Edit))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Thumbnail Creation Attempt for FileId {FileId}", fileId);
                return null;
            }

            var originalFile = _files.GetFile(fileId);
            if (originalFile == null)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "File Not Found For Thumbnail Creation {FileId}", fileId);
                return null;
            }

            string thumbFilePath = null;
            try
            {
                string originalFilePath = _files.GetFilePath(originalFile);
                if (System.IO.File.Exists(originalFilePath))
                {
                    string thumbFileName = $"{System.IO.Path.GetFileNameWithoutExtension(originalFile.Name)}_thumb.{originalFile.Extension}";
                    string folderPath = System.IO.Path.GetDirectoryName(originalFilePath);
                    thumbFilePath = System.IO.Path.Combine(folderPath, thumbFileName);

                    var resizedImagePath = _imageService.CreateImage(originalFilePath, width, height, "medium", "center", "white", "", originalFile.Extension, thumbFilePath);

                    if (string.IsNullOrEmpty(resizedImagePath) || !System.IO.File.Exists(resizedImagePath))
                    {
                        throw new Exception("Image resizing failed. Resized file was not created.");
                    }

                    var fileInfo = new System.IO.FileInfo(resizedImagePath);
                    int imageWidth, imageHeight;
                    using (var image = await SixLabors.ImageSharp.Image.LoadAsync(resizedImagePath))
                    {
                        imageWidth = image.Width;
                        imageHeight = image.Height;
                    }

                    var thumbFile = new Oqtane.Models.File
                    {
                        FolderId = originalFile.FolderId,
                        Name = thumbFileName,
                        Extension = originalFile.Extension,
                        Size = (int)fileInfo.Length,
                        ImageHeight = imageHeight,
                        ImageWidth = imageWidth
                    };

                    var newFile = _files.AddFile(thumbFile);
                    _logger.Log(LogLevel.Information, this, LogFunction.Create, "Thumbnail Created Successfully for FileId {FileId}. New FileId is {NewFileId}", fileId, newFile.FileId);

                    return new ThumbnailResponse { FileId = newFile.FileId, Url = newFile.Url };
                }
                else
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Read, "File Not Found For Resizing {FileId}", fileId);
                    return null;
                }
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(thumbFilePath) && System.IO.File.Exists(thumbFilePath))
                {
                    System.IO.File.Delete(thumbFilePath);
                }
                _logger.Log(LogLevel.Error, this, LogFunction.Create, ex, "Error Creating Thumbnail for FileId {FileId}", fileId);
                return null;
            }
        }
    }
}
