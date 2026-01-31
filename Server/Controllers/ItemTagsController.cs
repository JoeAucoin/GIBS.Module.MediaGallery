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
    public class ItemTagsController : Controller
    {
        private readonly IItemTagsRepository _itemTagsRepository;

        public ItemTagsController(IItemTagsRepository itemTagsRepository)
        {
            _itemTagsRepository = itemTagsRepository;
        }

        // GET: api/<controller>?itemId=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<ItemTags>> GetItemTags(int itemId)
        {
            return await _itemTagsRepository.GetItemTagsAsync(itemId);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<ItemTags> Get(int id)
        {
            return await _itemTagsRepository.GetItemTagAsync(id);
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ItemTags> Post([FromBody] ItemTags itemTag)
        {
            if (ModelState.IsValid)
            {
                itemTag = await _itemTagsRepository.AddItemTagAsync(itemTag);
            }
            return itemTag;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Delete(int id)
        {
            await _itemTagsRepository.DeleteItemTagAsync(id);
        }
    }
}
