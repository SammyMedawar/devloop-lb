using DevLoopLB.DTO;
using DevLoopLB.Models;

namespace DevLoopLB.Services.Interfaces
{
    public interface IImageAssetService
    {
        Task<IEnumerable<ImageAsset>> GetAllImageAssetsByEventIdAsync(int eventId);
        Task DeleteImageAssetsByEventId(int eventId);
        Task AddImageAssetsByEventId(List<SaveImageAssetDTO> imageAssetsDtos, int eventId);
    }
}
