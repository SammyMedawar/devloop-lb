using System;
using System.Collections.Generic;

namespace DevLoopLB.Models;

public partial class ImageAsset
{
    public int ImageAssetId { get; set; }

    public string? Caption { get; set; }

    public int EventId { get; set; }

    public virtual Event Event { get; set; } = null!;
}
