using khaothi_2024_net_server.Features.BaoCaoSoLieuConThiTHPT.DTOs;
using khaothi_2024_net_server.Features.BaoCaoSoLieuConThiTHPT.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuConThiTHPT
{
    public class BCThongTinConThiService : IBCThongTinConThiService
    {
        private readonly IBCThongTinConThiRepository _conThiRepository;
        private readonly ILogger<BCThongTinConThiService> _logger;

        public BCThongTinConThiService(
            IBCThongTinConThiRepository conThiRepository,
            ILogger<BCThongTinConThiService> logger)
        {
            _conThiRepository = conThiRepository;
            _logger = logger;
        }

        public async Task<bool> InsertAsync(KhaoThi_5_THPT_ThongTin_ConThi ThongTin)
        {
            try
            {
                // Validate dữ liệu đầu vào (ví dụ: kiểm tra null, độ dài chuỗi, v.v.)
                if (ThongTin == null)
                {
                    _logger.LogError("Không thể thêm thông tin con thí sinh với dữ liệu null.");
                    return false;
                }

                // Kiểm tra xem bản ghi đã tồn tại chưa
                if (await _conThiRepository.ExistsAsync(ThongTin.MaTruong, ThongTin.CCCD, ThongTin.MaDinhDanhCuaCon))
                {
                    _logger.LogWarning("Thông tin con thí sinh đã tồn tại (MaTruong: {MaTruong}, CCCD: {CCCD}, MaDinhDanhCuaCon: {MaDinhDanhCuaCon}).", ThongTin.MaTruong, ThongTin.CCCD, ThongTin.MaDinhDanhCuaCon);
                    return false;
                }

                return await _conThiRepository.InsertAsync(ThongTin);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm thông tin con thí sinh (MaTruong: {MaTruong}, CCCD: {CCCD}, MaDinhDanhCuaCon: {MaDinhDanhCuaCon}).", ThongTin.MaTruong, ThongTin.CCCD, ThongTin.MaDinhDanhCuaCon);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(KhaoThi_5_THPT_ThongTin_ConThi ThongTin)
        {
            try
            {
                // Validate dữ liệu đầu vào
                if (ThongTin == null)
                {
                    _logger.LogError("Không thể cập nhật thông tin con thí sinh với dữ liệu null.");
                    return false;
                }

                // Kiểm tra xem bản ghi có tồn tại không
                if (!await _conThiRepository.ExistsAsync(ThongTin.MaTruong, ThongTin.CCCD, ThongTin.MaDinhDanhCuaCon))
                {
                    _logger.LogWarning("Không tìm thấy thông tin con thí sinh để cập nhật (MaTruong: {MaTruong}, CCCD: {CCCD}, MaDinhDanhCuaCon: {MaDinhDanhCuaCon}).", ThongTin.MaTruong, ThongTin.CCCD, ThongTin.MaDinhDanhCuaCon);
                    return false;
                }

                return await _conThiRepository.UpdateAsync(ThongTin);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật thông tin con thí sinh (MaTruong: {MaTruong}, CCCD: {CCCD}, MaDinhDanhCuaCon: {MaDinhDanhCuaCon}).", ThongTin.MaTruong, ThongTin.CCCD, ThongTin.MaDinhDanhCuaCon);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string MaTruong, string CCCD, string MaDinhDanhCuaCon)
        {
            try
            {
                // Kiểm tra xem các khóa có giá trị không
                if (string.IsNullOrEmpty(MaTruong) || string.IsNullOrEmpty(CCCD) || string.IsNullOrEmpty(MaDinhDanhCuaCon))
                {
                    _logger.LogError("Không thể xóa thông tin con thí sinh với MaTruong, CCCD hoặc MaDinhDanhCuaCon là null hoặc rỗng.");
                    return false;
                }

                // Kiểm tra xem bản ghi có tồn tại không
                if (!await _conThiRepository.ExistsAsync(MaTruong, CCCD, MaDinhDanhCuaCon))
                {
                    _logger.LogWarning("Không tìm thấy thông tin con thí sinh để xóa (MaTruong: {MaTruong}, CCCD: {CCCD}, MaDinhDanhCuaCon: {MaDinhDanhCuaCon}).", MaTruong, CCCD, MaDinhDanhCuaCon);
                    return false;
                }

                return await _conThiRepository.DeleteAsync(MaTruong, CCCD, MaDinhDanhCuaCon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa thông tin con thí sinh (MaTruong: {MaTruong}, CCCD: {CCCD}, MaDinhDanhCuaCon: {MaDinhDanhCuaCon}).", MaTruong, CCCD, MaDinhDanhCuaCon);
                return false;
            }
        }

        public async Task<(IEnumerable<KhaoThi_5_THPT_ThongTin_ConThi> Items, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? searchTerm = null)
        {
            try
            {
                // Validate tham số đầu vào
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;
                if (pageSize > 100) pageSize = 100; // Giới hạn kích thước trang tối đa

                return await _conThiRepository.GetPaginatedAsync(page, pageSize, searchTerm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách thông tin con thí sinh phân trang (page: {page}, pageSize: {pageSize}, searchTerm: {searchTerm}).", page, pageSize, searchTerm);
                return (new List<KhaoThi_5_THPT_ThongTin_ConThi>(), 0);
            }
        }

        public async Task<KhaoThi_5_THPT_ThongTin_ConThi?> GetByKeysAsync(string MaTruong, string CCCD, string MaDinhDanhCuaCon)
        {
            try
            {
                // Validate tham số đầu vào
                if (string.IsNullOrEmpty(MaTruong) || string.IsNullOrEmpty(CCCD) || string.IsNullOrEmpty(MaDinhDanhCuaCon))
                {
                    _logger.LogWarning("Không thể lấy thông tin con thí sinh với MaTruong, CCCD hoặc MaDinhDanhCuaCon là null hoặc rỗng.");
                    return null;
                }

                return await _conThiRepository.GetByKeysAsync(MaTruong, CCCD, MaDinhDanhCuaCon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin con thí sinh theo khóa (MaTruong: {MaTruong}, CCCD: {CCCD}, MaDinhDanhCuaCon: {MaDinhDanhCuaCon}).", MaTruong, CCCD, MaDinhDanhCuaCon);
                return null;
            }
        }

        public async Task<IEnumerable<KhaoThi_5_THPT_ThongTin_ConThi>> GetAllAsync()
        {
            try
            {
                return await _conThiRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy tất cả thông tin con thí sinh.");
                return new List<KhaoThi_5_THPT_ThongTin_ConThi>();
            }
        }

        public async Task<bool> ExistsAsync(string MaTruong, string CCCD, string MaDinhDanhCuaCon)
        {
            try
            {
                // Validate tham số đầu vào
                if (string.IsNullOrEmpty(MaTruong) || string.IsNullOrEmpty(CCCD) || string.IsNullOrEmpty(MaDinhDanhCuaCon))
                {
                    _logger.LogWarning("Không thể kiểm tra sự tồn tại của thông tin con thí sinh với MaTruong, CCCD hoặc MaDinhDanhCuaCon là null hoặc rỗng.");
                    return false;
                }

                return await _conThiRepository.ExistsAsync(MaTruong, CCCD, MaDinhDanhCuaCon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi kiểm tra sự tồn tại của thông tin con thí sinh (MaTruong: {MaTruong}, CCCD: {CCCD}, MaDinhDanhCuaCon: {MaDinhDanhCuaCon}).", MaTruong, CCCD, MaDinhDanhCuaCon);
                return false;
            }
        }
    }
}