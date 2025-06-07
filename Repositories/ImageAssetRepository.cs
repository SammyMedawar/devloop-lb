using DevLoopLB.DTO;
using DevLoopLB.Models;
using DevLoopLB.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevLoopLB.Repositories
{
    public class ImageAssetRepository(DevLoopLbContext context) : IImageAssetRepository
    {
        public async Task AddMultipleImageAssets(List<ImageAsset> imageAssets)
        {
            context.ImageAssets.AddRange(imageAssets);
        }

        public async Task DeleteMultipleImageAssetsByEventId(int eventId)
        {
            var assetsToDelete = context.ImageAssets.Where(x => x.EventId == eventId).ToList();
            context.ImageAssets.RemoveRange(assetsToDelete);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ImageAsset>> GetAllImageAssetsByEventId(int eventId)
        {
            return await context.ImageAssets
                .Where(x => x.EventId == eventId)
                .ToListAsync();
        }
    }
}
