using khaothi_2024_net_server.Core.Interfaces;
using khaothi_2024_net_server.Features.Shares.DTOs;

namespace khaothi_2024_net_server.Features.Shares
{
    public static class QuanEndpoints
    {
        public static void MapQuanEndpoints(this WebApplication app)
        {
            // Sử dụng IDataAccessLayer thay vì IDbConnection trực tiếp
            app.MapGet("/quan", async (IDataAccessLayer dataAccess) =>
            {
                var truong = await dataAccess.QueryAsync<QuanModel>(
                    @"select MaQuan,TenQuan from Quan where Cast(MaQuan as int)<25 and MaQuan not like '0' order by Cast(MaQuan as int)                      
                        ");
                return Results.Ok(truong);
            });

            // Endpoint lấy ngân hàng theo ID
            app.MapGet("/quan/{maquan}", async (string MaQuan, IDataAccessLayer dataAccess) =>
            {

                const string sql = @"select MaQuan,TenQuan from Quan where MaQuan=@MaQuan order by Cast(MaQuan as int)";
                var quan = await dataAccess.QueryFirstOrDefaultAsync<TruongModel>(sql, "@MaQuan", MaQuan);
                return quan == null ? Results.NotFound() : Results.Ok(quan);
            });
        }
    }
}