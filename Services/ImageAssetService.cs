﻿using DevLoopLB.DTO;
using DevLoopLB.Models;
using DevLoopLB.Repositories.Interfaces;
using DevLoopLB.Services.Interfaces;

namespace DevLoopLB.Services
{
    public class ImageAssetService(IImageAssetRepository imageAssetRepository, IEventRepository eventRepository,
        IFileStorageService fileStorageService)
        : IImageAssetService
    {
        public async Task AddImageAssetsByEventId(List<SaveImageAssetDTO> imageAssetsDtos, int eventId)
        {
            var imageAssets = new List<ImageAsset>();

            foreach (var dto in imageAssetsDtos)
            {
                if (dto.ImageFile == null)
                    throw new ArgumentException("Image file is required");

                var savedFilePath = await fileStorageService.SaveFileAsync(dto.ImageFile, "EventsGallery");

                imageAssets.Add(new ImageAsset
                {
                    Caption = dto.Caption,
                    EventId = eventId,
                    ImageAssetLink = savedFilePath
                });
            }

            await imageAssetRepository.AddMultipleImageAssets(imageAssets);
        }

        public async Task DeleteImageAssetsByEventId(int eventId)
        {
            if (await eventRepository.DoesEventExist(eventId))
            {
                var assets = await imageAssetRepository.GetAllImageAssetsByEventId(eventId);

                var filePaths = assets
                    .Where(a => !string.IsNullOrEmpty(a.ImageAssetLink))
                    .Select(a => a.ImageAssetLink!)
                    .ToList();

                await fileStorageService.DeleteMultipleFilesAsync(filePaths);

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
