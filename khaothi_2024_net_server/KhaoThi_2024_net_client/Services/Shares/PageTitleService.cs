namespace KhaoThi_2024_net_client.Services.Shares
{
    public class PageTitleService:IPageTitleService
    {
        private readonly Dictionary<string, string> _pageNames = new()
    {
        { "/", "Trang chủ" },
        { "/dashboard", "Quản lý hệ thống" },
        { "/profile", "Quản lý người dùng" },
        { "/roles", "Quản lý vai trò" },
        { "/permissions", "Phân quyền" },
        // Thêm các mapping khác
    };
        public string GetPageTitle(string path)
        {
            try
            {
                // Chuẩn hóa path
                path = path.ToLower().TrimEnd('/');

                // Kiểm tra trong dictionary
                if (_pageNames.TryGetValue(path, out string? pageName))
                {
                    return pageName;
                }

                // Xử lý trường hợp không tìm thấy
                var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
                if (segments.Length > 0)
                {
                    var lastSegment = segments.Last();
                    return char.ToUpper(lastSegment[0]) + lastSegment[1..];
                }

                return "Trang chủ";
            }
            catch (Exception)
            {
                return "Trang chủ";
            }
        }
    }
}
