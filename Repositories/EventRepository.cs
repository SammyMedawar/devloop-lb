
using DevLoopLB.DTO;
using DevLoopLB.Models;
using DevLoopLB.Repositories.Interfaces;
using DevLoopLB.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevLoopLB.Repositories
{
    public class EventRepository(DevLoopLbContext context, IImageAssetService imageAssetService) : IEventRepository
    {
        public async Task<Event> AddEventAsync(Event evt, SaveEventDTO eventDTO)
        {
            await context.Events.AddAsync(evt);
            await context.SaveChangesAsync();
            await imageAssetService.AddImageAssetsByEventId(eventDTO.Gallery, evt.EventId);
            return evt;
        }

        public async Task DeleteEventAsync(int id)
        {
            var evt = await context.Events.FindAsync(id);
            if (evt == null) {
                throw new KeyNotFoundException($"Event with ID {id} not found.");
            }
            context.Events.Remove(evt);
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await context.Events
                .Include(e => e.ImageAssets)
                .Include(e=>e.Tags)
                .ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            var eventObj = await context.Events
                .Include(e => e.ImageAssets)
                .Include(e=>e.Tags)
                .FirstOrDefaultAsync(e => e.EventId == id);
            if (eventObj == null)
            {
                throw new KeyNotFoundException($"Event with ID {id} not found.");
            }
            return eventObj;
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task UpdateEventAsync(Event evt)
        {
            context.Events.Update(evt);
        }

        public async Task<bool> DoesEventExist(int eventId)
        {
            var evt = await GetEventByIdAsync(eventId);
            if (evt == null || evt.EventId == 0)
            {
                throw new KeyNotFoundException($"Event not found with id: {eventId}");
            }
            return true;
        }

        public async Task<(List<Event> events, int totalRows)> GetFilteredEventsAsync(EventFilterRequestDTO filter)
        {
            var query = context.Events
                .Include(e => e.ImageAssets)
                .Include(e => e.Tags)
                .AsQueryable();

            query = ApplyFilters(query, filter);

            var totalRows = await query.CountAsync();

            var events = await query
                .OrderByDescending(e => e.DateCreated)
                .Skip(filter.Skip)
                .Take(filter.PageSize)
                .ToListAsync();

            return (events, totalRows);
        }

        public async Task<int> GetTotalEventCountAsync()
        {
            return await context.Events.CountAsync();
        }

        private IQueryable<Event> ApplyFilters(IQueryable<Event> query, EventFilterRequestDTO filter)
        {
            if (filter.HasSearchQuery)
            {
                query = query.Where(e => e.Title.Contains(filter.SearchQuery!, StringComparison.OrdinalIgnoreCase));
            }

            if (filter.EventDateStart.HasValue)
            {
                query = query.Where(e => e.EventDateStart >= filter.EventDateStart.Value);
            }

            if (filter.EventDateEnd.HasValue)
            {
                query = query.Where(e => e.EventDateStart <= filter.EventDateEnd.Value);
            }

            if (filter.HasTagFilter)
            {
                query = query.Where(e => e.Tags.Any(t => filter.TagIds!.Contains(t.TagId)));
            }

            return query;
        }
    }
}
