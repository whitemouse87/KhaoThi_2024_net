namespace KhaoThi_2024_net_client.Components
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        // Constructor mặc định cho JSON deserialization
        public PaginatedResult()
        {
        }

        // Constructor hiện tại
        public PaginatedResult(IEnumerable<T> items, int count, int page, int pageSize)
        {
            Items = items;
            TotalCount = count;
            Page = page;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;
    }
}
