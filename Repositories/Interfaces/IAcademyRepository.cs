using DevLoopLB.DTO;
using DevLoopLB.Models;

namespace DevLoopLB.Repositories.Interfaces
{
    public interface IAcademyRepository
    {
        Task<IEnumerable<Academy>> GetAllAcademiesAsync();
        Task<Academy> GetAcademyByIdAsync(int id);
        Task<Academy> AddAcademyAsync(Academy academy);
        Task UpdateAcademyAsync(Academy academy);
        Task DeleteAcademyAsync(int id);
        Task<(List<Academy> academies, int totalRows)> GetFilteredAcademiesAsync(AcademyFilterRequestDTO filter);
        Task SaveChangesAsync();
    }
}
