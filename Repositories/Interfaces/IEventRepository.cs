using DevLoopLB.Models;

namespace DevLoopLB.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event> GetEventByIdAsync(int id);
        Task AddEventAsync(Event evt);
        Task SaveChangesAsync();
        Task DeleteEventAsync(int id);
        Task UpdateEventAsync(Event evt);
        Task<bool> DoesEventExist(int eventId);
    }
}
