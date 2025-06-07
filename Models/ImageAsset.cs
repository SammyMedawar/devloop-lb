using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DevLoopLB.Models;

public partial class ImageAsset
{
    public int ImageAssetId { get; set; }

    public string? Caption { get; set; }

    public int EventId { get; set; }
    public string? ImageAssetLink { get; set; }
    [JsonIgnore]
    public virtual Event Event { get; set; } = null!;
}
