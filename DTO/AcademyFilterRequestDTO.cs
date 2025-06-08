using System.ComponentModel.DataAnnotations;

namespace DevLoopLB.DTO
{
    public class AcademyFilterRequestDTO
    {
        [Range(1, 50, ErrorMessage = "Page size must be between 1 and 50")]
        public int PageSize { get; set; } = 10;

        [Range(1, int.MaxValue, ErrorMessage = "Current page must be greater than 0")]
        public int CurrentPage { get; set; } = 1;

        public string? SearchQuery { get; set; }
        public int Skip => (CurrentPage - 1) * PageSize;
        public bool HasSearchQuery => !string.IsNullOrWhiteSpace(SearchQuery);
    }
}
