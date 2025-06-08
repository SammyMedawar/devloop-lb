using DevLoopLB.Models;

namespace DevLoopLB.DTO
{
    public class AcademyLoadMoreResponseDTO
    {
        public List<Academy> Academies { get; set; } = new();
        public LoadMoreMetadata Pagination { get; set; } = new();
    }
}
