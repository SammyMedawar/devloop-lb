namespace DevLoopLB.Models
{
    public class ImageAsset
    {
        public int ImageAssetID { get; set; }
        public string Caption { get; set; }
        public int EventID { get; set; }
        public Event Event { get; set; }
    }
}
