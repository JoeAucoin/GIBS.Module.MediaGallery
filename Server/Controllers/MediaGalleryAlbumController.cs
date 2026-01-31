using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using GIBS.Module.MediaGallery.Services;
using Oqtane.Controllers;
using System.Net;
using System.Threading.Tasks;
using GIBS.Module.MediaGallery.Models;

namespace GIBS.Module.MediaGallery.Controllers
{
    [Route("api/[controller]")]
    public class MediaGalleryAlbumController : ModuleControllerBase
    {
        private readonly IMediaGalleryItemService _mediaGalleryItemService;

        public MediaGalleryAlbumController(IMediaGalleryItemService mediaGalleryItemService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _mediaGalleryItemService = mediaGalleryItemService;
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<MediaGalleryAlbum>> Get(string moduleid)
        {
            if (int.TryParse(moduleid, out int ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return await _mediaGalleryItemService.GetMediaGalleryAlbumsAsync(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized MediaGalleryAlbum Get Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }
    }
}
