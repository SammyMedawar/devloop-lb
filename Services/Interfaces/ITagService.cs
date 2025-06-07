using DevLoopLB.Models;

namespace DevLoopLB.Services.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<Tag> GetTagByIdAsync(int id);
        Task AddTagAsync(Tag tag);
        Task DeleteTagAsync(int id);
        Task UpdateTagAsync(Tag tag);
        Task<bool> CheckIfTagsExistBulkAsync(List<int> tagIds);
        Task<IEnumerable<Tag>> GetTagsByIdsAsync(List<int> tagIds);
    }
}
