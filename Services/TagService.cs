using DevLoopLB.Models;
using DevLoopLB.Repositories;
using DevLoopLB.Repositories.Interfaces;
using DevLoopLB.Services.Interfaces;

namespace DevLoopLB.Services
{
    public class TagService(ITagRepository repository) : ITagService
    {
        public async Task AddTagAsync(Tag tag)
        {
            await repository.AddTagAsync(tag);
            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await repository.GetAllTagsAsync();
        }

        public async Task<Tag> GetTagByIdAsync(int id)
        {
            return await repository.GetTagByIdAsync(id);
        }


        public async Task DeleteTagAsync(int id)
        {
            Tag tag = await repository.GetTagByIdAsync(id);
            if (tag == null)
            {
                throw new KeyNotFoundException($"Tag with id {id} not found");
            }
            await repository.DeleteTagAsync(id);
            await repository.SaveChangesAsync();
        }

        public async Task UpdateTagAsync(Tag tag)
        {
            Tag oldTag = await repository.GetTagByIdAsync(tag.TagId);
            if (oldTag == null)
            {
                throw new KeyNotFoundException($"Tag with id {tag.TagId} not found");
            }
            await repository.UpdateTagAsync(tag);
            await repository.SaveChangesAsync();
        }


        public async Task<bool> CheckIfTagsExistBulkAsync(List<int> tagIds)
        {
            if (tagIds == null || !tagIds.Any())
                throw new ArgumentException("TagIds list cannot be null or empty.");

            var foundTags = await repository.GetTagsByIdsAsync(tagIds);

            var foundIds = foundTags.Select(t => t.TagId).ToHashSet();

            var missingIds = tagIds.Where(id => !foundIds.Contains(id)).ToList();

            if (missingIds.Any())
            {
                throw new KeyNotFoundException($"Tags not found with ids: {string.Join(", ", missingIds)}");
            }
            return true;
        }
    }
}
