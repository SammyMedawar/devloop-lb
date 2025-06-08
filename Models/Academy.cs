using System.ComponentModel.DataAnnotations;

namespace DevLoopLB.Models
{
    public partial class Academy
    {
        public int AcademyId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string ShortDescription { get; set; } = null!;

        public string? LongDescription { get; set; }

        [StringLength(255)]
        public string? MetaTitle { get; set; }

        [StringLength(255)]
        public string? MetaDescription { get; set; }

        [StringLength(255)]
        public string? PosterLink { get; set; }

        public DateTime? DateCreated { get; set; }

        [StringLength(255)]
        public string? ReadMoreLink { get; set; }

        [StringLength(255)]
        public string? ReadMoreText { get; set; }
    }
}
