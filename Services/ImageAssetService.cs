using DevLoopLB.DTO;
using DevLoopLB.Models;
using DevLoopLB.Repositories.Interfaces;
using DevLoopLB.Services.Interfaces;
using Microsoft.AspNetCore.Components.RenderTree;

namespace DevLoopLB.Services
{
    public class ImageAssetService(IImageAssetRepository imageAssetRepository, IEventRepository eventRepository)
        : IImageAssetService
    {
        public async Task AddImageAssetsByEventId(List<SaveImageAssetDTO> imageAssetsDtos, int eventId)
        {
            var imageAssets = new List<ImageAsset>();

            foreach (var dto in imageAssetsDtos)
            {
                var randomGuid = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(dto.ImageFile?.FileName);
                var fileName = randomGuid + extension;

                var folderPath = Path.Combine("wwwroot", "EventsGallery");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }

                imageAssets.Add(new ImageAsset
                {
                    Caption = dto.Caption,
                    EventId = eventId,
                    ImageAssetLink = "/EventsGallery/" + fileName
                });
            }

            await imageAssetRepository.AddMultipleImageAssets(imageAssets);
        }

        public async Task DeleteImageAssetsByEventId(int eventId)
        {
            if (await eventRepository.DoesEventExist(eventId))
            {
                var assets = await imageAssetRepository.GetAllImageAssetsByEventId(eventId);

                foreach (var asset in assets)
                {
                    if (!string.IsNullOrEmpty(asset.ImageAssetLink))
                    {
                        var filePath = Path.Combine("wwwroot", asset.ImageAssetLink.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                    }
                }

                await imageAssetRepository.DeleteMultipleImageAssetsByEventId(eventId);
            }
        }
        public async Task<IEnumerable<ImageAsset>> GetAllImageAssetsByEventIdAsync(int eventId)
        {
            if (await eventRepository.DoesEventExist(eventId))
            {
                return await imageAssetRepository.GetAllImageAssetsByEventId(eventId);
            }
            throw new KeyNotFoundException($"Event not found with id: {eventId}");
        }


    }
}
