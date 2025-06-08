
using System.ComponentModel.DataAnnotations;
using DevLoopLB.Attributes;

namespace DevLoopLB.DTO
{
    public class SaveEventDTO
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 300 characters.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Short description is required.")]
        [StringLength(300, ErrorMessage = "Short description cannot exceed 500 characters.")]
        public string? Shortdescription { get; set; }

        [Required(ErrorMessage = "Long description is required.")]
        public string? LongDescription { get; set; }

        [StringLength(60, ErrorMessage = "Meta title cannot exceed 60 characters.")]
        public string? MetaTitle { get; set; }

        [StringLength(160, ErrorMessage = "Meta description cannot exceed 160 characters.")]
        public string? MetaDescription { get; set; }
        public IFormFile? Poster { get; set; }

        [Required(ErrorMessage = "Event start date is required.")]
        public DateOnly EventDateStart { get; set; }

        [Required(ErrorMessage = "Event end date is required.")]
        [DateGreaterThan(nameof(EventDateStart), ErrorMessage = "End date must be after the start date.")]
        public DateOnly EventDateEnd { get; set; }
        [Required(ErrorMessage = "Gallery is required.")]
        public List<SaveImageAssetDTO>? Gallery { get; set; }
        [Required(ErrorMessage = "Tags are required.")]
        public List<int>? Tags { get; set; }
    }

    public class SaveImageAssetDTO
    {
        [StringLength(100, ErrorMessage = "Caption cannot exceed 100 characters.")]
        public string? Caption { get; set; }

        [Required(ErrorMessage = "Image file is required.")]
        [AllowedImageExtensions(500000, ".jpg", ".jpeg", ".png", ".webp", ErrorMessage = "Only JPG, PNG, and WebP files are allowed.")]
        public IFormFile? ImageFile { get; set; }

        public string? ImageLink { get; set; }
    }
}
