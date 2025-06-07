using System.ComponentModel.DataAnnotations;

namespace DevLoopLB.DTO
{
    public class EventFilterRequestDTO
    {
        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 12;

        [Range(1, int.MaxValue, ErrorMessage = "Current page must be greater than 0")]
        public int CurrentPage { get; set; } = 1;
        public string? SearchQuery { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? EventDateStart { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? EventDateEnd { get; set; } 

        public List<int>? TagIds { get; set; }

        public int Skip => (CurrentPage - 1) * PageSize;
        public bool HasSearchQuery => !string.IsNullOrWhiteSpace(SearchQuery);
        public bool HasDateFilter => EventDateStart.HasValue || EventDateEnd.HasValue;
        public bool HasTagFilter => TagIds?.Any() == true;
    }
}
