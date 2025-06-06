using DevLoopLB.Models;

namespace DevLoopLB.Repositories.Interfaces
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<Tag> GetTagByIdAsync(int id);
        Task AddTagAsync(Tag tag);
        Task SaveChangesAsync();
        Task DeleteTagAsync(int id);
        Task UpdateTagAsync(Tag tag);
    }
}
