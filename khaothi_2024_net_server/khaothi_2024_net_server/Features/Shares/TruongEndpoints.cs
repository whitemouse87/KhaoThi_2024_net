using khaothi_2024_net_server.Core.Interfaces;
using khaothi_2024_net_server.Features.Shares.DTOs;

namespace khaothi_2024_net_server.Features.Shares
{
    public static class TruongEndpoints
    {
        public static void MapTruongEndpoints(this WebApplication app)
        {
            // Sử dụng IDataAccessLayer thay vì IDbConnection trực tiếp
            app.MapGet("/truong", async (IDataAccessLayer dataAccess) =>
            {
                var truong = await dataAccess.QueryAsync<TruongModel>(
                    @"select MaTruong,
                        MaTruong+' - '+TenTruongLookupQuangIch as TenTruong,
                        MaTruong_New.Quan as MaQuan,
                        Quan.TenQuan,
                        CapHoc,
                        MaTruongNew,
                        CoiThi,
                        ChamThi,
                        ChucVuVanPhong
                        from MaTruong_New
                        inner join Quan on MaTruong_New.Quan=Quan.MaQuan
                        where CapHoc in ('THCS','THPT','PGD','GDTX','SGD') and SUBSTRING(MaTruong,8,2) not like 'KD' order by MaTruong
                        ");
                return Results.Ok(truong);
            });

            // Endpoint lấy ngân hàng theo ID
            app.MapGet("/truong/{matruong}", async (string MaTruong, IDataAccessLayer dataAccess) =>
            {

                const string sql = @"select MaTruong,
                        MaTruong+' - '+TenTruongLookupQuangIch as TenTruong,
                        MaTruong_New.Quan as MaQuan,
                        Quan.TenQuan,
                        CapHoc,
                        MaTruongNew,
                        CoiThi,
                        ChamThi,
                        ChucVuVanPhong
                        from MaTruong_New
                        inner join Quan on MaTruong_New.Quan=Quan.MaQuan 
                        where MaTruong = @MaTruong and CapHoc in ('THCS','THPT','PGD','GDTX','SGD') and SUBSTRING(MaTruong,8,2) not like 'KD' order by MaTruong";
                var truong = await dataAccess.QueryFirstOrDefaultAsync<TruongModel>(sql, "@MaTruong", MaTruong);
                return truong == null ? Results.NotFound() : Results.Ok(truong);
            });
        }


        // Thêm các endpoints khác nếu cần
    }
}
