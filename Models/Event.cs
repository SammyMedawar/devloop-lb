using System;
using System.Collections.Generic;

namespace DevLoopLB.Models;

public partial class Event
{
    public int EventId { get; set; }

    public string Title { get; set; } = null!;

    public string Shortdescription { get; set; } = null!;

    public string? Longdescription { get; set; }

    public string? Metatitle { get; set; }

    public string? Metadescription { get; set; }

    public string? Poster { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateOnly EventDateStart { get; set; }

    public DateOnly? EventDateEnd { get; set; }

    public virtual ICollection<ImageAsset> ImageAssets { get; set; } = new List<ImageAsset>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
