namespace DevLoopLB.DTO
{
    public class LoadMoreMetadata
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalRows { get; set; }
        public bool HasMore { get; set; }  // Are there more items to load?
        public int ItemsLoaded { get; set; } // Items in current response
    }
}
