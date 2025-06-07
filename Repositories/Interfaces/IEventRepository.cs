using DevLoopLB.DTO;
using DevLoopLB.Models;

namespace DevLoopLB.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event> GetEventByIdAsync(int id);
        Task<Event> AddEventAsync(Event evt, SaveEventDTO eventDTO);
        Task SaveChangesAsync();
        Task DeleteEventAsync(int id);
        Task UpdateEventAsync(Event evt);
        Task<bool> DoesEventExist(int eventId);
        Task<(List<Event> events, int totalRows)> GetFilteredEventsAsync(EventFilterRequestDTO filter);
        Task<int> GetTotalEventCountAsync();
    }
}
