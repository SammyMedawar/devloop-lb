using DevLoopLB.DTO;
using DevLoopLB.Models;
using DevLoopLB.Repositories.Interfaces;
using DevLoopLB.Services.Interfaces;

namespace DevLoopLB.Services
{
    public class AcademyService(IAcademyRepository repository, IFileStorageService fileStorageService)
        : IAcademyService
    {
        public async Task<int> AddAcademyAsync(SaveAcademyDTO academyDto)
        {
            var academy = new Academy
            {
                Title = academyDto.Title,
                ShortDescription = academyDto.ShortDescription,
                LongDescription = academyDto.LongDescription,
                MetaTitle = academyDto.MetaTitle,
                MetaDescription = academyDto.MetaDescription,
                ReadMoreLink = academyDto.ReadMoreLink,
                ReadMoreText = academyDto.ReadMoreText,
                DateCreated = DateTime.Now
            };

            if (academyDto.Poster != null)
            {
                academy.PosterLink = await fileStorageService.SaveFileAsync(academyDto.Poster, "AcademyPosters");
            }

            academy = await repository.AddAcademyAsync(academy);
            await repository.SaveChangesAsync();

            return academy.AcademyId;
        }

        public async Task DeleteAcademyAsync(int id)
        {
            var academy = await repository.GetAcademyByIdAsync(id);

            if (!string.IsNullOrEmpty(academy.PosterLink))
            {
                await fileStorageService.DeleteFileAsync(academy.PosterLink);
            }

            await repository.DeleteAcademyAsync(id);
            await repository.SaveChangesAsync();
        }

        public async Task<Academy> GetAcademyByIdAsync(int id)
        {
            return await repository.GetAcademyByIdAsync(id);
        }

        public async Task<IEnumerable<Academy>> GetAllAcademiesAsync()
        {
            return await repository.GetAllAcademiesAsync();
        }

        public async Task<AcademyLoadMoreResponseDTO> GetFilteredAcademiesAsync(AcademyFilterRequestDTO filter)
        {
            var (academies, totalRows) = await repository.GetFilteredAcademiesAsync(filter);

            var itemsLoaded = academies.Count;
            var totalItemsAlreadyLoaded = filter.Skip + itemsLoaded;
            var hasMore = totalItemsAlreadyLoaded < totalRows;

            return new AcademyLoadMoreResponseDTO
            {
                Academies = academies,
                Pagination = new LoadMoreMetadata
                {
                    CurrentPage = filter.CurrentPage,
                    PageSize = filter.PageSize,
                    TotalRows = totalRows,
                    HasMore = hasMore,
                    ItemsLoaded = itemsLoaded
                }
            };
        }

        public async Task UpdateAcademyAsync(int id, SaveAcademyDTO academyDto)
        {
            var existingAcademy = await repository.GetAcademyByIdAsync(id);

            string? newPosterLink = existingAcademy.PosterLink;
            if (academyDto.Poster != null)
            {
                if (!string.IsNullOrEmpty(existingAcademy.PosterLink))
                {
                    await fileStorageService.DeleteFileAsync(existingAcademy.PosterLink);
                }

                newPosterLink = await fileStorageService.SaveFileAsync(academyDto.Poster, "AcademyPosters");
            }

            var updatedAcademy = new Academy
            {
                AcademyId = id,
                Title = academyDto.Title,
                ShortDescription = academyDto.ShortDescription,
                LongDescription = academyDto.LongDescription,
                MetaTitle = academyDto.MetaTitle,
                MetaDescription = academyDto.MetaDescription,
                PosterLink = newPosterLink,
                ReadMoreLink = academyDto.ReadMoreLink,
                ReadMoreText = academyDto.ReadMoreText,
                DateCreated = existingAcademy.DateCreated
            };

            await repository.UpdateAcademyAsync(updatedAcademy);
            await repository.SaveChangesAsync();
        }
    }
}
