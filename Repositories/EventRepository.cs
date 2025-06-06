
using DevLoopLB.Models;
using DevLoopLB.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevLoopLB.Repositories
{
    public class EventRepository(DevLoopLbContext context) : IEventRepository
    {
        public async Task<Event> AddEventAsync(Event evt)
        {
            await context.Events.AddAsync(evt);
            await context.SaveChangesAsync();
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
                .ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            var eventObj = await context.Events
                .Include(e => e.ImageAssets)
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
            var oldEvent = await context.Events.FindAsync(evt.EventId);
            if(oldEvent == null)
            {
                throw new KeyNotFoundException($"Event with ID {id} not found.");
            }
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

    }
}
