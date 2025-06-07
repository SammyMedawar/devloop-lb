using DevLoopLB.DTO;
using DevLoopLB.Models;
using DevLoopLB.Repositories.Interfaces;
using DevLoopLB.Services.Interfaces;

namespace DevLoopLB.Services
{
    public class EventService(IEventRepository repository, IImageAssetService imageAssetService,
        ITagService tagService, DevLoopLbContext context) : IEventService
    {
        public async Task<int> AddEventAsync(SaveEventDTO evt)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                if (evt.Gallery == null || evt.Gallery.Count == 0)
                {
                    throw new BadHttpRequestException("Gallery cannot be empty");
                }
                if (evt.Tags == null || evt.Tags.Count == 0)
                {
                    throw new BadHttpRequestException("Tags cannot be empty");
                }
                bool doTagsExist = await tagService.CheckIfTagsExistBulkAsync(evt.Tags);
                if (!doTagsExist)
                {
                    throw new BadHttpRequestException("Invalid tags");
                }
                var tags = await tagService.GetTagsByIdsAsync(evt.Tags);

                Event newEvt = new Event
                {
                    Title = evt.Title?? "Default Title",
                    Shortdescription = evt.Shortdescription ?? "Default Short Description",
                    Longdescription = evt.LongDescription,
                    Metatitle = evt.MetaTitle,
                    Metadescription = evt.MetaDescription,
                    DateCreated = DateTime.Now,
                    EventDateStart = evt.EventDateStart,
                    EventDateEnd = evt.EventDateEnd,
                    Tags = tags.ToList()
                };
                newEvt = await repository.AddEventAsync(newEvt);
                await imageAssetService.AddImageAssetsByEventId(evt.Gallery, newEvt.EventId);
                await transaction.CommitAsync();
                return newEvt.EventId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await repository.GetAllEventsAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await repository.GetEventByIdAsync(id);
        }


    }
}
