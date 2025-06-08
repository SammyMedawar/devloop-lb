using DevLoopLB.DTO;
using DevLoopLB.Models;

namespace DevLoopLB.Services.Interfaces
{
    public interface IAcademyService
    {
        Task<IEnumerable<Academy>> GetAllAcademiesAsync();
        Task<Academy> GetAcademyByIdAsync(int id);
        Task<int> AddAcademyAsync(SaveAcademyDTO academyDto);
        Task UpdateAcademyAsync(int id, SaveAcademyDTO academyDto);
        Task DeleteAcademyAsync(int id);
        Task<AcademyLoadMoreResponseDTO> GetFilteredAcademiesAsync(AcademyFilterRequestDTO filter);
    }
}
