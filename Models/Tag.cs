namespace DevLoopLB.Models
{
    public class Tag
    {
        public int TagID { get; set; }
        public string Name { get; set; }
        public ICollection<EventTag> EventTags { get; set; }
    }
}
