using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GIBS.Module.MediaGallery.Models;

namespace GIBS.Module.MediaGallery.Repository
{
    public class TagsRepository : ITagsRepository
    {
        private readonly MediaGalleryContext _db;

        public TagsRepository(MediaGalleryContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Tags>> GetTagsAsync(int moduleId)
        {
            return await _db.Tags.Where(t => t.ModuleId == moduleId).ToListAsync();
        }

        public async Task<Tags> GetTagAsync(int tagId)
        {
            return await _db.Tags.FindAsync(tagId);
        }

        public async Task<Tags> AddTagAsync(Tags tag)
        {
            _db.Tags.Add(tag);
            await _db.SaveChangesAsync();
            return tag;
        }

        public async Task<Tags> UpdateTagAsync(Tags tag)
        {
            _db.Entry(tag).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return tag;
        }

        public async Task DeleteTagAsync(int tagId)
        {
            var tag = await _db.Tags.FindAsync(tagId);
            if (tag != null)
            {
                _db.Tags.Remove(tag);
                await _db.SaveChangesAsync();
            }
        }
    }
}
