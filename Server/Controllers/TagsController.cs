using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Oqtane.Shared;
using Oqtane.Enums;
using GIBS.Module.MediaGallery.Models;
using GIBS.Module.MediaGallery.Repository;

namespace GIBS.Module.MediaGallery.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class TagsController : Controller
    {
        private readonly ITagsRepository _tagsRepository;

        public TagsController(ITagsRepository tagsRepository)
        {
            _tagsRepository = tagsRepository;
        }

        // GET: api/<controller>?moduleId=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Tags>> GetTags(int moduleId)
        {
            return await _tagsRepository.GetTagsAsync(moduleId);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<Tags> Get(int id)
        {
            return await _tagsRepository.GetTagAsync(id);
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Tags> Post([FromBody] Tags tag)
        {
            if (ModelState.IsValid)
            {
                tag = await _tagsRepository.AddTagAsync(tag);
            }
            return tag;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Tags> Put(int id, [FromBody] Tags tag)
        {
            if (ModelState.IsValid && tag.TagId == id)
            {
                tag = await _tagsRepository.UpdateTagAsync(tag);
            }
            return tag;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Delete(int id)
        {
            await _tagsRepository.DeleteTagAsync(id);
        }
    }
}
