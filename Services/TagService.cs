using DevLoopLB.Models;
using DevLoopLB.Repositories;
using DevLoopLB.Repositories.Interfaces;
using DevLoopLB.Services.Interfaces;

namespace DevLoopLB.Services
{
    public class TagService (ITagRepository repository) : ITagService
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
            await repository.DeleteTagAsync(id);
            await repository.SaveChangesAsync();
        }

        public async Task UpdateTagAsync(Tag tag)
        {
            await repository.UpdateTagAsync(tag);
            await repository.SaveChangesAsync();
        }
    }
}
