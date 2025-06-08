using DevLoopLB.Models;

namespace DevLoopLB.DTO
{
    public class EventPagedResponseDTO
    {
        public List<Event> Events { get; set; } = new();
        public PaginationMetadata Pagination { get; set; } = new();
    }
}
