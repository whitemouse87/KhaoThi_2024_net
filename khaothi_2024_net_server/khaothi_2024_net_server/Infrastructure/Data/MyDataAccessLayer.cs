using Dapper;
using khaothi_2024_net_server.Core.Interfaces;
using Microsoft.Data.SqlClient;

using System.Data;


namespace khaothi_2024_net_server.Infrastructure.Data
{
    public class MyDataAccessLayer : IDataAccessLayer
    {
        private readonly string _connectionString;
        private readonly ILogger<MyDataAccessLayer> _logger;
        private bool _disposed;


        //public MyDataAccessLayer(IConfiguration configuration, ILogger<MyDataAccessLayer> logger)
        //{
        //    _connectionString = configuration.GetConnectionString("SGD")
        //        ?? throw new ArgumentNullException("Connection string 'SGD' not found");
        //    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        //}
        public MyDataAccessLayer(IConfiguration configuration, ILogger<MyDataAccessLayer> logger)
        {
            _logger = logger;

            try
            {
                // Log đường dẫn làm việc
                var currentDir = Directory.GetCurrentDirectory();
                _logger.LogInformation("Current directory: {Directory}", currentDir);

                // Kiểm tra file appsettings.json
                var appSettingsPath = Path.Combine(currentDir, "appsettings.json");
                _logger.LogInformation("appsettings.json exists: {Exists}", File.Exists(appSettingsPath));

                // Tìm connection string từ nhiều nguồn khác nhau
                string connectionString = null;

                // Thử tìm SGD
                connectionString = configuration.GetConnectionString("SGD");
                _logger.LogInformation("Connection string 'SGD': {Found}", !string.IsNullOrEmpty(connectionString));

                // Nếu không có, thử tìm DefaultConnection
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = configuration.GetConnectionString("DefaultConnection");
                    _logger.LogInformation("Connection string 'DefaultConnection': {Found}", !string.IsNullOrEmpty(connectionString));
                }

                // Nếu vẫn không có, thử đọc từ web.config
                if (string.IsNullOrEmpty(connectionString))
                {
                    try
                    {
                        var webConfigPath = Path.Combine(currentDir, "web.config");
                        _logger.LogInformation("web.config exists: {Exists}", File.Exists(webConfigPath));

                        if (File.Exists(webConfigPath))
                        {
                            var configXml = new System.Xml.XmlDocument();
                            configXml.Load(webConfigPath);

                            var connectionNodes = configXml.SelectNodes("//connectionStrings/add[@name='DefaultConnection']");
                            if (connectionNodes?.Count > 0)
                            {
                                connectionString = connectionNodes[0].Attributes["connectionString"]?.Value;
                                _logger.LogInformation("Connection string from web.config: {Found}", !string.IsNullOrEmpty(connectionString));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error reading connection string from web.config");
                    }
                }

                // Nếu vẫn không tìm thấy sau tất cả cách trên, sử dụng hardcoded (tạm thời)
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = "Server=103.77.166.169;Database=ngoaingusgd;MultipleActiveResultSets=true;User ID=whitemouse87;Password=Abc123!!!;TrustServerCertificate=True";
                    _logger.LogWarning("Using hardcoded connection string as a last resort");
                }

                _connectionString = connectionString;
                _logger.LogInformation("Connection string configured successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing MyDataAccessLayer");
                throw;
            }
        }
        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, params object[] parameters)
        {
            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync();
                return await connection.QueryAsync<T>(sql, CreateParameters(parameters));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing query: {Sql}", sql);
                throw new DataAccessException("Error executing query", ex);
            }
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, params object[] parameters)
        {
            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<T>(sql, CreateParameters(parameters));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing query first or default: {Sql}", sql);
                throw new DataAccessException("Error executing query", ex);
            }
        }

        public async Task<int> ExecuteAsync(string sql, params object[] parameters)
        {
            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync();
                return await connection.ExecuteAsync(sql, CreateParameters(parameters));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing command: {Sql}", sql);
                throw new DataAccessException("Error executing command", ex);
            }
        }

        public async Task<T> ExecuteScalarAsync<T>(string sql, params object[] parameters)
        {
            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<T>(sql, CreateParameters(parameters));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing scalar: {Sql}", sql);
                throw new DataAccessException("Error executing scalar", ex);
            }
        }

        public async Task<DataTable> GetDataTableAsync(string sql, params object[] parameters)
        {
            try
            {
                using var connection = CreateConnection();
                await connection.OpenAsync();
                using var reader = await connection.ExecuteReaderAsync(sql, CreateParameters(parameters));
                var table = new DataTable();
                table.Load(reader);
                return table;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating DataTable: {Sql}", sql);
                throw new DataAccessException("Error creating DataTable", ex);
            }
        }

        public async Task ExecuteInTransactionAsync(Func<IDbTransaction, Task> action)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                await action(transaction);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Transaction rolled back due to error");
                throw new DataAccessException("Transaction failed", ex);
            }
        }

        public async Task<(IEnumerable<T> Items, int TotalCount)> QueryPaginatedAsync<T>(
            string sql, int page, int pageSize, params object[] parameters)
        {
            var countSql = $"SELECT COUNT(*) FROM ({sql}) AS CountQuery";
            var pagingSql = $@"{sql}
               ORDER BY (SELECT NULL)
               OFFSET {(page - 1) * pageSize} ROWS
               FETCH NEXT {pageSize} ROWS ONLY";

            using var connection = CreateConnection();
            await connection.OpenAsync();

            var totalCount = await connection.ExecuteScalarAsync<int>(countSql, CreateParameters(parameters));
            var items = await connection.QueryAsync<T>(pagingSql, CreateParameters(parameters));

            return (items, totalCount);
        }

        public async Task BulkInsertWithBulkCopyAsync<T>(string tableName, IEnumerable<T> data)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            if (data == null || !data.Any())
                return;

            using var connection = CreateConnection();
            await connection.OpenAsync();

            using var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, null)
            {
                DestinationTableName = tableName,
                BatchSize = 1000,
                BulkCopyTimeout = 300
            };

            var dataTable = CreateDataTable(data);

            try
            {
                await bulkCopy.WriteToServerAsync(dataTable);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in bulk copy to table {TableName}", tableName);
                throw new DataAccessException($"Bulk copy failed for table {tableName}", ex);
            }
        }

        public async Task BulkUpdateAsync<T>(string tableName, IEnumerable<T> data, string keyField, int batchSize = 1000)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            var properties = typeof(T).GetProperties()
                .Where(p => p.Name != keyField && p.CanRead && !p.GetGetMethod().IsStatic);

            var updateSet = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));
            var sql = $"UPDATE {tableName} SET {updateSet} WHERE {keyField} = @{keyField}";

            using var connection = CreateConnection();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                var batches = data.Select((item, index) => new { Item = item, Index = index })
                                 .GroupBy(x => x.Index / batchSize)
                                 .Select(g => g.Select(x => x.Item));

                foreach (var batch in batches)
                {
                    await connection.ExecuteAsync(sql, batch, transaction);
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error in bulk update for table {TableName}", tableName);
                throw new DataAccessException($"Bulk update failed for table {tableName}", ex);
            }
        }

        private DynamicParameters CreateParameters(object[] parameters)
        {
            var dynamicParams = new DynamicParameters();

            if (parameters == null || parameters.Length == 0)
                return dynamicParams;

            for (int i = 0; i < parameters.Length - 1; i += 2)
            {
                var paramName = parameters[i]?.ToString();
                var paramValue = parameters[i + 1];

                if (string.IsNullOrEmpty(paramName)) continue;

                if (paramValue == null)
                    dynamicParams.Add(paramName, DBNull.Value);
                else
                    dynamicParams.Add(paramName, paramValue);
            }

            return dynamicParams;
        }

        private DataTable CreateDataTable<T>(IEnumerable<T> data)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.CanRead && !p.GetGetMethod().IsStatic);

            var dataTable = new DataTable();

            foreach (var property in properties)
            {
                var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                dataTable.Columns.Add(property.Name, propertyType);
            }

            foreach (var item in data)
            {
                var row = dataTable.NewRow();
                foreach (var property in properties)
                {
                    var value = property.GetValue(item);
                    row[property.Name] = value ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    SqlConnection.ClearAllPools();
                }
                _disposed = true;
            }
        }

        ~MyDataAccessLayer()
        {
            Dispose(false);
        }
    }

    public class DataAccessException : Exception
    {
        public DataAccessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}