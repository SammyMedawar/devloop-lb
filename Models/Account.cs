using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DevLoopLB.Models
{
    public partial class Account
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string? Username { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string? Email { get; set; }


        [JsonIgnore]
        public byte[]? HashedPassword { get; set; }

        public bool IsAdmin { get; set; }
    }
}
