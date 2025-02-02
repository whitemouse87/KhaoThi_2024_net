namespace khaothi_2024_net_server.Core.Models.Common
{
    /// <summary>
    /// Generic class để xử lý kết quả phân trang cho mọi loại entity
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của entity</typeparam>
    public class PaginatedResult<T>
    {
        /// <summary>
        /// Danh sách items trong trang hiện tại
        /// </summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// Tổng số bản ghi (toàn bộ, không phải trong trang hiện tại)
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Số trang hiện tại
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Số bản ghi trên mỗi trang
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        /// <summary>
        /// Kiểm tra có trang trước không
        /// </summary>
        public bool HasPreviousPage => Page > 1;

        /// <summary>
        /// Kiểm tra có trang sau không
        /// </summary>
        public bool HasNextPage => Page < TotalPages;

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public PaginatedResult()
        {
            Items = new List<T>();
        }

        /// <summary>
        /// Constructor với dữ liệu ban đầu
        /// </summary>
        public PaginatedResult(IEnumerable<T> items, int count, int page, int pageSize)
        {
            Items = items;
            TotalCount = count;
            Page = page;
            PageSize = pageSize;
        }

        /// <summary>
        /// Tạo một PaginatedResult trống
        /// </summary>
        public static PaginatedResult<T> Empty()
        {
            return new PaginatedResult<T>
            {
                Items = Enumerable.Empty<T>(),
                TotalCount = 0,
                Page = 1,
                PageSize = 10
            };
        }
    }
}
