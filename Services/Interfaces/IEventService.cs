using DevLoopLB.DTO;
using DevLoopLB.Models;

namespace DevLoopLB.Services.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event> GetEventByIdAsync(int id);
        Task AddEventAsync(SaveEventDTO evt);
    }
}
