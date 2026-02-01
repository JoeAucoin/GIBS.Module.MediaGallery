using GIBS.Module.MediaGallery.Models;
using GIBS.Module.MediaGallery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oqtane.Controllers;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Repository;
using Oqtane.Services;
using Oqtane.Shared;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace GIBS.Module.MediaGallery.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class MediaGalleryItemController : ModuleControllerBase
    {
        private readonly IMediaGalleryItemService _mediaGalleryItemService;
        private readonly IFileRepository _files;
        private readonly IImageService _imageService;
        
        private Models.MediaGalleryItem _selectedItem;
        public MediaGalleryItemController(IMediaGalleryItemService mediaGalleryItemService, IFileRepository files, IImageService imageService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _mediaGalleryItemService = mediaGalleryItemService;
            _files = files;
            _imageService = imageService;
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<MediaGalleryItem>> Get(string moduleid)
        {
            if (int.TryParse(moduleid, out int ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return await _mediaGalleryItemService.GetMediaGalleryItemsAsync(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized MediaGalleryItem Get Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // GET api/<controller>/5/1
        [HttpGet("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<MediaGalleryItem> Get(int id, int moduleid)
        {
            MediaGalleryItem item = await _mediaGalleryItemService.GetMediaGalleryItemAsync(id, moduleid);
            if (item != null && IsAuthorizedEntityId(EntityNames.Module, item.ModuleId))
            {
                return item;
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized MediaGalleryItem Get Attempt {ItemId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<MediaGalleryItem> Post([FromBody] MediaGalleryItem item)
        {
            if (ModelState.IsValid && IsAuthorizedEntityId(EntityNames.Module, item.ModuleId))
            {
                item = await _mediaGalleryItemService.AddMediaGalleryItemAsync(item);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized MediaGalleryItem Post Attempt {MediaGalleryItem}", item);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                item = null;
            }
            return item;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<MediaGalleryItem> Put(int id, [FromBody] MediaGalleryItem item)
        {
            if (ModelState.IsValid && item.ItemId == id && IsAuthorizedEntityId(EntityNames.Module, item.ModuleId))
            {
                item = await _mediaGalleryItemService.UpdateMediaGalleryItemAsync(item);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized MediaGalleryItem Put Attempt {MediaGalleryItem}", item);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                item = null;
            }
            return item;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Delete(int id, int moduleid)
        {
            MediaGalleryItem item = await _mediaGalleryItemService.GetMediaGalleryItemAsync(id, moduleid);
            if (item != null && IsAuthorizedEntityId(EntityNames.Module, item.ModuleId))
            {
                await _mediaGalleryItemService.DeleteMediaGalleryItemAsync(id, moduleid);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized MediaGalleryItem Delete Attempt {ItemId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }


        [HttpPost("resize-image")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<ThumbnailResponse>> ResizeImageFile([FromBody] ResizeRequest request)
        {
            try
            {
                var result = await _mediaGalleryItemService.ResizeImageAsync(request.FileId, request.Width, request.Height, request.ModuleId);
                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound("Image resize failed. See server logs for details.");
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, ex, "Unhandled Error in ResizeImageFile Controller for FileId {FileId}", request.FileId);
                return StatusCode(500, "An unhandled error occurred while resizing the image.");
            }
        }

        [HttpPost("resize-image-thumbnail")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<ThumbnailResponse>> ResizeImage([FromBody] ResizeRequest request)
        {
            try
            {
                var result = await _mediaGalleryItemService.CreateImageThumbnailAsync(request.FileId, request.Width, request.Height, request.ModuleId);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("Thumbnail creation failed. See server logs for details.");
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, ex, "Unhandled Error in ResizeImage Controller for FileId {FileId}", request.FileId);
                return StatusCode(500, "An unhandled error occurred while creating the thumbnail.");
            }
        }


        public class ThumbnailResponse
        {
            public int FileId { get; set; }
            public string Url { get; set; }
        }
    }
}
