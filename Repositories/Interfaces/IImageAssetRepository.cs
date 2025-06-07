using DevLoopLB.DTO;
using DevLoopLB.Models;

namespace DevLoopLB.Repositories.Interfaces
{
    public interface IImageAssetRepository
    {
        Task<IEnumerable<ImageAsset>> GetAllImageAssetsByEventId(int eventId);
        Task AddMultipleImageAssets(List<ImageAsset> imageAssets);
        Task DeleteMultipleImageAssetsByEventId(int eventId);
        Task SaveChangesAsync();
    }
}
