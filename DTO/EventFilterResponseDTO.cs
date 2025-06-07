using DevLoopLB.Models;

namespace DevLoopLB.DTO
{
    public class EventPagedResponseDTO
    {
        public List<Event> Events { get; set; } = new();
        public PaginationMetadata Pagination { get; set; } = new();
    }

    public class PaginationMetadata
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalRows { get; set; }
        public int FilteredRows { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
