﻿using DevLoopLB.DTO;
using DevLoopLB.Exceptions;
using DevLoopLB.Models;
using DevLoopLB.Repositories.Interfaces;
using DevLoopLB.Services.Interfaces;

namespace DevLoopLB.Services
{
    public class EventService(IEventRepository repository, IImageAssetService imageAssetService,
        ITagService tagService, DevLoopLbContext context) : IEventService
    {
        public async Task<int> AddEventAsync(SaveEventDTO eventDto)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                if (eventDto.Gallery == null || eventDto.Gallery.Count == 0)
                {
                    throw new BusinessValidationException("Gallery cannot be empty");
                }
                if (eventDto.Tags == null || eventDto.Tags.Count == 0)
                {
                    throw new BusinessValidationException("Tags cannot be empty");
                }
                bool doTagsExist = await tagService.CheckIfTagsExistBulkAsync(eventDto.Tags);
                if (!doTagsExist)
                {
                    throw new BusinessValidationException("Invalid tags");
                }
                var tags = await tagService.GetTagsByIdsAsync(eventDto.Tags);

                Event newEvt = new Event
                {
                    Title = eventDto.Title ?? "Default Title",
                    Shortdescription = eventDto.Shortdescription ?? "Default Short Description",
                    Longdescription = eventDto.LongDescription,
                    Metatitle = eventDto.MetaTitle,
                    Metadescription = eventDto.MetaDescription,
                    DateCreated = DateTime.Now,
                    EventDateStart = eventDto.EventDateStart,
                    EventDateEnd = eventDto.EventDateEnd,
                    Tags = tags.ToList()
                };
                newEvt = await repository.AddEventAsync(newEvt, eventDto);
                await repository.SaveChangesAsync();
                await transaction.CommitAsync();
                return newEvt.EventId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteEventAsync(int id)
        {
            var existingEvent = await repository.GetEventByIdAsync(id);
            await imageAssetService.DeleteImageAssetsByEventId(id);

            await repository.DeleteEventAsync(id);
            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await repository.GetAllEventsAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await repository.GetEventByIdAsync(id);
        }

        public async Task<EventPagedResponseDTO> GetFilteredEventsAsync(EventFilterRequestDTO filter)
        {
            var (events, totalRows) = await repository.GetFilteredEventsAsync(filter);
            var totalPages = (int)Math.Ceiling((double)totalRows / filter.PageSize);
            return new EventPagedResponseDTO
            {
                Events = events,
                Pagination = new PaginationMetadata
                {
                    CurrentPage = filter.CurrentPage,
                    PageSize = filter.PageSize,
                    TotalRows = totalRows,
                    TotalPages = totalPages
                }
            };
        }

        public async Task UpdateEventAsync(int id, SaveEventDTO evt)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                if (evt.Gallery == null || evt.Gallery.Count == 0)
                {
                    throw new BusinessValidationException("Gallery cannot be empty");
                }
                if (evt.Tags == null || evt.Tags.Count == 0)
                {
                    throw new BusinessValidationException("Tags cannot be empty");
                }
                var existingEvent = await repository.GetEventByIdAsync(id);
                if (existingEvent == null || existingEvent.EventId == 0)
                {
                    throw new EntityNotFoundException("Event", 0);
                }

                bool doTagsExist = await tagService.CheckIfTagsExistBulkAsync(evt.Tags);
                if (!doTagsExist)
                {
                    throw new BusinessValidationException("Invalid tags");
                }
                var tags = await tagService.GetTagsByIdsAsync(evt.Tags);

                existingEvent.Title = evt.Title ?? existingEvent.Title;
                existingEvent.Shortdescription = evt.Shortdescription ?? existingEvent.Shortdescription;
                existingEvent.Longdescription = evt.LongDescription;
                existingEvent.Metatitle = evt.MetaTitle;
                existingEvent.Metadescription = evt.MetaDescription;
                existingEvent.EventDateStart = evt.EventDateStart;
                existingEvent.EventDateEnd = evt.EventDateEnd;
                existingEvent.Tags = new List<Tag>(tags.ToList());

                await repository.UpdateEventAsync(existingEvent);

                await imageAssetService.DeleteImageAssetsByEventId(id);
                await imageAssetService.AddImageAssetsByEventId(evt.Gallery, id);

                await repository.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
