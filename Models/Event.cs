namespace DevLoopLB.Models
{
    public class Event
    {
        public int EventID { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string Poster { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? EventDateStart { get; set; }
        public DateTime? EventDateEnd { get; set; }

        public ICollection<ImageAsset> ImageAssets { get; set; }
        public ICollection<EventTag> EventTags { get; set; }
    }
}
