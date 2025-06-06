using DevLoopLB.DTO;
using DevLoopLB.Models;
using DevLoopLB.Repositories;
using DevLoopLB.Repositories.Interfaces;
using DevLoopLB.Services.Interfaces;

namespace DevLoopLB.Services
{
    public class EventService(IEventRepository repository, IImageAssetService imageAssetService,
        ITagService tagService) : IEventService
    {
        public async Task AddEventAsync(SaveEventDTO evt)
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
            Event newEvt = new Event
            {
                Title = evt.Title,
                Shortdescription = evt.Shortdescription,
                Longdescription = evt.LongDescription,
                Metatitle = evt.MetaTitle,
                Metadescription = evt.MetaDescription,
                DateCreated = DateTime.Now,
                EventDateStart = evt.EventDateStart,
                EventDateEnd = evt.EventDateEnd,
                Tags = evt.Tags
            };
            newEvt = await repository.AddEventAsync(newEvt);
            await imageAssetService.AddImageAssetsByEventId(evt.Gallery, newEvt.EventId);
            //save tag
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


    }
}
