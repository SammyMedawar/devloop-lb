using DevLoopLB.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DevLoopLB.DTO
{
    public class SaveAcademyDTO
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Short description is required.")]
        [StringLength(255, ErrorMessage = "Short description cannot exceed 255 characters.")]
        public string ShortDescription { get; set; } = string.Empty;

        public string? LongDescription { get; set; }

        [StringLength(255, ErrorMessage = "Meta title cannot exceed 255 characters.")]
        public string? MetaTitle { get; set; }

        [StringLength(255, ErrorMessage = "Meta description cannot exceed 255 characters.")]
        public string? MetaDescription { get; set; }

        [AllowedImageExtensions(500000, ".jpg", ".jpeg", ".png", ".webp", ErrorMessage = "Only JPG, PNG, and WebP files are allowed.")]
        public IFormFile? Poster { get; set; }

        [StringLength(255, ErrorMessage = "Read more link cannot exceed 255 characters.")]
        public string? ReadMoreLink { get; set; }

        [StringLength(255, ErrorMessage = "Read more text cannot exceed 255 characters.")]
        public string? ReadMoreText { get; set; }
    }
}
