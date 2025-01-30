using System.Data;

namespace khaothi_2024_net_server.Core.Interfaces
{
    public interface IDataAccessLayer : IDisposable
    {
        /// <summary>
        /// Thực hiện truy vấn và trả về tập kết quả
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        /// <param name="sql">Câu lệnh SQL</param>
        /// <param name="parameters">Các tham số theo cặp (tên tham số, giá trị)</param>
        Task<IEnumerable<T>> QueryAsync<T>(string sql, params object[] parameters);

        /// <summary>
        /// Thực hiện truy vấn và trả về kết quả đầu tiên hoặc null
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        /// <param name="sql">Câu lệnh SQL</param>
        /// <param name="parameters">Các tham số theo cặp (tên tham số, giá trị)</param>
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, params object[] parameters);

        /// <summary>
        /// Thực thi câu lệnh SQL không trả về kết quả (Insert, Update, Delete)
        /// </summary>
        /// <param name="sql">Câu lệnh SQL</param>
        /// <param name="parameters">Các tham số theo cặp (tên tham số, giá trị)</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        Task<int> ExecuteAsync(string sql, params object[] parameters);

        /// <summary>
        /// Thực thi và trả về giá trị đơn
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
        Task<T> ExecuteScalarAsync<T>(string sql, params object[] parameters);

        /// <summary>
        /// Lấy dữ liệu dưới dạng DataTable
        /// </summary>
        Task<DataTable> GetDataTableAsync(string sql, params object[] parameters);

        /// <summary>
        /// Thực hiện nhiều câu lệnh trong một transaction
        /// </summary>
        /// <param name="action">Hàm thực thi các câu lệnh trong transaction</param>
        Task ExecuteInTransactionAsync(Func<IDbTransaction, Task> action);

        /// <summary>
        /// Thực hiện truy vấn với phân trang
        /// </summary>
        /// <param name="page">Số trang (bắt đầu từ 1)</param>
        /// <param name="pageSize">Số bản ghi mỗi trang</param>
        Task<(IEnumerable<T> Items, int TotalCount)> QueryPaginatedAsync<T>(
            string sql,
            int page,
            int pageSize,
            params object[] parameters);

        /// <summary>
        /// Thực hiện bulk insert sử dụng SqlBulkCopy (Hiệu năng tốt nhất cho insert số lượng lớn)
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu của bảng</typeparam>
        /// <param name="tableName">Tên bảng</param>
        /// <param name="data">Dữ liệu cần insert</param>
        Task BulkInsertWithBulkCopyAsync<T>(string tableName, IEnumerable<T> data);

        /// <summary>
        /// Thực hiện bulk update cho nhiều bản ghi
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu của bảng</typeparam>
        /// <param name="tableName">Tên bảng</param>
        /// <param name="data">Dữ liệu cần update</param>
        /// <param name="keyField">Tên trường khóa chính</param>
        /// <param name="batchSize">Số lượng bản ghi mỗi lần update</param>
        Task BulkUpdateAsync<T>(string tableName, IEnumerable<T> data, string keyField, int batchSize = 1000);
    }
}