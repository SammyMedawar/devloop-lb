using DevLoopLB.Models;
using DevLoopLB.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevLoopLB.Repositories
{
    public class TagRepository(DevLoopLbContext context) : ITagRepository
    {
        public async Task AddTagAsync(Tag tag)
        {
            await context.Tags.AddAsync(tag);
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await context.Tags.ToListAsync();
        }

        public async Task<Tag> GetTagByIdAsync(int id)
        {
            var tagObj = await context.Tags
                .FirstOrDefaultAsync(e => e.TagId == id);
            if(tagObj == null)
            {
                throw new KeyNotFoundException($"Event with ID {id} not found.");
            }
            return tagObj;
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task DeleteTagAsync(int id)
        {
            var tag = await context.Tags.FindAsync(id);
            if(tag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {id} not found.");
            }
            context.Tags.Remove(tag);
        }

        public async Task UpdateTagAsync(Tag tag)
        {
            var oldTag = await context.Tags.FindAsync(tag.TagId);
            if (oldTag == null)
            {
                throw new KeyNotFoundException($"Tag with ID {tag.TagId} not found.");
            }

            oldTag.Name = tag.Name;
            context.Tags.Update(oldTag);
        }

        public async Task<IEnumerable<Tag>> GetTagsByIdsAsync(List<int> tagIds)
        {
            return await context.Tags
                .Where(t => tagIds.Contains(t.TagId))
                .ToListAsync();
        }
    }
}
