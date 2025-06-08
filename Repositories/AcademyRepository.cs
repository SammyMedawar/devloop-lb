using DevLoopLB.DTO;
using DevLoopLB.Exceptions;
using DevLoopLB.Models;
using DevLoopLB.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevLoopLB.Repositories
{
    public class AcademyRepository (DevLoopLbContext context) : IAcademyRepository
    {
        public async Task<Academy> AddAcademyAsync(Academy academy)
        {
            await context.Academies.AddAsync(academy);
            return academy;
        }

        public async Task DeleteAcademyAsync(int id)
        {
            var academy = await context.Academies.FindAsync(id);
            if (academy == null)
            {
                throw new EntityNotFoundException("Academy", id);
            }
            context.Academies.Remove(academy);
        }

        public async Task<Academy> GetAcademyByIdAsync(int id)
        {
            var academy = await context.Academies.FindAsync(id);
            if (academy == null)
            {
                throw new EntityNotFoundException("Academy", id);
            }
            return academy;
        }

        public async Task<IEnumerable<Academy>> GetAllAcademiesAsync()
        {
            return await context.Academies
                .OrderByDescending(a => a.DateCreated)
                .ToListAsync();
        }

        public async Task<(List<Academy> academies, int totalRows)> GetFilteredAcademiesAsync(AcademyFilterRequestDTO filter)
        {
            var query = context.Academies.AsQueryable();
            if (filter.HasSearchQuery)
            {
                query = query.Where(a => a.Title.Contains(filter.SearchQuery!));
            }
            var totalRows = await query.CountAsync();
            var academies = await query
                .OrderByDescending(a => a.DateCreated)
                .Skip(filter.Skip)
                .Take(filter.PageSize)
                .ToListAsync();
            return (academies, totalRows);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task UpdateAcademyAsync(Academy academy)
        {
            var existingAcademy = await context.Academies.FindAsync(academy.AcademyId);
            if (existingAcademy == null)
            {
                throw new EntityNotFoundException("Academy", academy.AcademyId);
            }

            existingAcademy.Title = academy.Title;
            existingAcademy.ShortDescription = academy.ShortDescription;
            existingAcademy.LongDescription = academy.LongDescription;
            existingAcademy.MetaTitle = academy.MetaTitle;
            existingAcademy.MetaDescription = academy.MetaDescription;
            existingAcademy.PosterLink = academy.PosterLink;
            existingAcademy.ReadMoreLink = academy.ReadMoreLink;
            existingAcademy.ReadMoreText = academy.ReadMoreText;

            context.Academies.Update(existingAcademy);
        }
    }
}
