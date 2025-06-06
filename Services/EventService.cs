using DevLoopLB.DTO;
using DevLoopLB.Models;
using DevLoopLB.Repositories;
using DevLoopLB.Repositories.Interfaces;
using DevLoopLB.Services.Interfaces;

namespace DevLoopLB.Services
{
    public class EventService(IEventRepository repository) : IEventService
    {
        public async Task AddEventAsync(SaveEventDTO evt)
        {
            await repository.AddEventAsync(evt);
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
