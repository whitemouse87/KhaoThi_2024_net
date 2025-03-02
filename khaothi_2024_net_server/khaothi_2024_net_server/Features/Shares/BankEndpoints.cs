using Dapper;
using khaothi_2024_net_server.Core.Interfaces;
using khaothi_2024_net_server.Features.Shares.DTOs;
using khaothi_2024_net_server.Features.UserManagement.DTOs;
using System.Data;

namespace khaothi_2024_net_server.Features.Shares
{
    public static class BankEndpoints
    {
        public static void MapBankEndpoints(this WebApplication app)
        {
            // Sử dụng IDataAccessLayer thay vì IDbConnection trực tiếp
            app.MapGet("/banks", async (IDataAccessLayer dataAccess) =>
            {
                var banks = await dataAccess.QueryAsync<NganHangModel>(
                    "select * from Share_DanhSachNganHang order by MaNganHang");
                return Results.Ok(banks);
            });

            // Endpoint lấy ngân hàng theo ID
            app.MapGet("/banks/{id}", async (int id, IDataAccessLayer dataAccess) =>
            {

                const string sql = @"select * from Share_DanhSachNganHang where ID = @ID";
                var bank = await dataAccess.QueryFirstOrDefaultAsync<NganHangModel>(sql, "@ID", id);
                return bank == null ? Results.NotFound() : Results.Ok(bank);
            });

            // Thêm các endpoints khác nếu cần
        }
    }
}
