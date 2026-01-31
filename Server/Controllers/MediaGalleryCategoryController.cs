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

namespace GIBS.Module.MediaGallery.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class MediaGalleryCategoryController : ModuleControllerBase
    {
        private readonly IMediaGalleryCategoryService _mediaGalleryCategoryService;

        public MediaGalleryCategoryController(IMediaGalleryCategoryService mediaGalleryCategoryService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _mediaGalleryCategoryService = mediaGalleryCategoryService;
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Models.MediaGalleryCategory>> Get(string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return await _mediaGalleryCategoryService.GetMediaGalleryCategoriesAsync(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized MediaGalleryCategory Get Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<Models.MediaGalleryCategory> Get(int id, int moduleid)
        {
            Models.MediaGalleryCategory category = await _mediaGalleryCategoryService.GetMediaGalleryCategoryAsync(id, moduleid);
            if (category != null && IsAuthorizedEntityId(EntityNames.Module, category.ModuleId))
            {
                return category;
            }
            else
            { 
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized MediaGalleryCategory Get Attempt {CategoryId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.MediaGalleryCategory> Post([FromBody] Models.MediaGalleryCategory category)
        {
            if (ModelState.IsValid && IsAuthorizedEntityId(EntityNames.Module, category.ModuleId))
            {
                category = await _mediaGalleryCategoryService.AddMediaGalleryCategoryAsync(category);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized MediaGalleryCategory Post Attempt {MediaGalleryCategory}", category);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                category = null;
            }
            return category;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.MediaGalleryCategory> Put(int id, [FromBody] Models.MediaGalleryCategory category)
        {
            if (ModelState.IsValid && category.CategoryId == id && IsAuthorizedEntityId(EntityNames.Module, category.ModuleId))
            {
                category = await _mediaGalleryCategoryService.UpdateMediaGalleryCategoryAsync(category);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized MediaGalleryCategory Put Attempt {MediaGalleryCategory}", category);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                category = null;
            }
            return category;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Delete(int id, int moduleid)
        {
            Models.MediaGalleryCategory category = await _mediaGalleryCategoryService.GetMediaGalleryCategoryAsync(id, moduleid);
            if (category != null && IsAuthorizedEntityId(EntityNames.Module, category.ModuleId))
            {
                await _mediaGalleryCategoryService.DeleteMediaGalleryCategoryAsync(id, category.ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized MediaGalleryCategory Delete Attempt {CategoryId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }
    }
}
