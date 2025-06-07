using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DevLoopLB.Models;

public partial class Tag
{
    public int TagId { get; set; }

    [Required(ErrorMessage = "Tag name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Tag name must be between 2 and 100 characters")]
    [RegularExpression(@"^[a-zA-Z0-9\s-]*$", ErrorMessage = "Tag name can only contain letters, numbers, spaces, and hyphens")]
    public string Name { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
