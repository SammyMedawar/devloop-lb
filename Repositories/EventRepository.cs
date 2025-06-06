
using DevLoopLB.Models;
using DevLoopLB.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevLoopLB.Repositories
{
    public class EventRepository(DevLoopLbContext context) : IEventRepository
    {
        public async Task AddEventAsync(Event evt)
        {
            await context.Events.AddAsync(evt);
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
    }
}
