using DevLoopLB.DTO;
using DevLoopLB.Models;

namespace DevLoopLB.Services.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event> GetEventByIdAsync(int id);
        Task<int> AddEventAsync(SaveEventDTO evt);
        Task DeleteEventAsync(int id);
        Task UpdateEventAsync(int id, SaveEventDTO evt);
    }
}
