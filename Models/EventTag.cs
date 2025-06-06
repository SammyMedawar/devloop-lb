namespace DevLoopLB.Models
{
    public class EventTag
    {
        public int EventID { get; set; }
        public Event Event { get; set; }
        public int TagID { get; set; }
        public Tag Tag { get; set; }
    }
}
