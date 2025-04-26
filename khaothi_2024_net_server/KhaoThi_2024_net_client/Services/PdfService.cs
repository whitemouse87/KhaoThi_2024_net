using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;
using KhaoThi_2024_net_client.Services.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhaoThi_2024_net_client.Services
{
    /// <summary>
    /// Service xử lý việc tạo và tải xuống báo cáo PDF
    /// </summary>
    public interface IPdfService
    {
        /// <summary>
        /// Tạo báo cáo PDF và trả về dưới dạng mảng byte
        /// </summary>
        /// <param name="thongTinTruong">Thông tin trường học</param>
        /// <param name="nhomMons">Danh sách nhóm môn học</param>
        /// <returns>Mảng byte của file PDF</returns>
        Task<byte[]> GeneratePdfReportAsync(
            KhaoThi_1_THPT_ThongTin_DonViModel thongTinTruong,
            IEnumerable<KhaoThi_2_THPT_NhomMon_DonViModel> nhomMons,
            IEnumerable<KhaoThi_5_THPT_ThongTin_ConThiModel> nhomConThis,
            IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel> nhomlanhdaodonvis
            );

        /// <summary>
        /// Tạo báo cáo PDF và tải xuống trực tiếp từ trình duyệt
        /// </summary>
        /// <param name="thongTinTruong">Thông tin trường học</param>
        /// <param name="nhomMons">Danh sách nhóm môn học</param>
        /// <param name="fileName">Tên file PDF</param>
        /// <returns>Task</returns>
        //Task GeneratePdfReportAndDownloadAsync(
        //    KhaoThi_1_THPT_ThongTin_DonViModel thongTinTruong,
        //    IEnumerable<KhaoThi_2_THPT_NhomMon_DonViModel> nhomMons,
        //    string fileName);
    }

    /// <summary>
    /// Triển khai service tạo và xuất báo cáo PDF
    /// </summary>
    public class PdfService : IPdfService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly ILoggingService _logger;

        /// <summary>
        /// Khởi tạo PdfService với JSRuntime và LoggingService
        /// </summary>
        /// <param name="jsRuntime">JS Runtime để gọi các hàm JavaScript</param>
        /// <param name="logger">Logging service để ghi log</param>
        public PdfService(IJSRuntime jsRuntime, ILoggingService logger)
        {
            _jsRuntime = jsRuntime;
            _logger = logger;
        }

        /// <summary>
        /// Tạo báo cáo PDF và trả về dưới dạng mảng byte
        /// </summary>
        /// <param name="thongTinTruong">Thông tin trường học</param>
        /// <param name="nhomMons">Danh sách nhóm môn học</param>
        /// <returns>Mảng byte của file PDF</returns>
        public async Task<byte[]> GeneratePdfReportAsync(
            KhaoThi_1_THPT_ThongTin_DonViModel thongTinTruong,
            IEnumerable<KhaoThi_2_THPT_NhomMon_DonViModel> nhomMons,
            IEnumerable<KhaoThi_5_THPT_ThongTin_ConThiModel> nhomConThis,
            IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel> nhomlanhdaodonvis
            )
        {
            try
            {
                // Đảm bảo thongTinTruong không null để tránh NullReferenceException
                if (thongTinTruong == null)
                {
                    await _logger.LogErrorAsync("Thông tin trường null khi tạo báo cáo PDF", null, nameof(PdfService));
                    thongTinTruong = new KhaoThi_1_THPT_ThongTin_DonViModel();
                }



                DateTime reportTime = DateTime.Now;

                // Chuẩn bị dữ liệu thông tin trường
                var schoolData = new
                {
                    tenTruong = thongTinTruong.TenTruong ?? "",
                    quan = thongTinTruong.QuanDangKyDuThi.ToString() ?? "",
                    maTruongSo = thongTinTruong.MaTruong ?? "",
                    maTruongBo = thongTinTruong.MaTruongBo ?? "",
                    emailNhanThongBao = thongTinTruong.EmailNhanThongBao ?? "",
                    loaiHinhDaoTao = thongTinTruong.LoaiHinhDaoTao ?? "",
                    sdtTruong = thongTinTruong.SDTTruong ?? "Chưa nhập",
                    sdtHoiDong = thongTinTruong.SDTPhongHoiDong ?? "Chưa nhập",
                    diaChiTruong = thongTinTruong.DiaChiTruong ?? "Chưa nhập",

                    // Thông tin cán bộ nhập liệu
                    hoTenNguoiNhapLieu = thongTinTruong.HoTenNhapLieu ?? "CHƯA NHẬP",
                    chucVuNhapLieu = thongTinTruong.ChucVuNhapLieu ?? "Chưa nhập",
                    soDiDongNhapLieu = thongTinTruong.SDTDiDongNhapLieu ?? "Chưa nhập",
                    emailNhapLieu = thongTinTruong.EmailNhapLieu ?? "Chưa nhập",

                    // Thông tin nhân sự
                    ht = thongTinTruong.TongHieuTruong.ToString(),
                    pht = thongTinTruong.TongPhoHieuTruong.ToString(),
                    giaoVien = thongTinTruong.TongGiaoVien.ToString(),
                    ttcm = thongTinTruong.TongGiaoVien_ToTruong.ToString(),
                    nhanVien = thongTinTruong.TongNhanVien.ToString(),
                    giaoVienCoiThi = thongTinTruong.TongGiaoVien_CoiThi.ToString(),
                    phongToiDa = thongTinTruong.TongSoPhongToiDa.ToString(),
                    phongDuDK = thongTinTruong.TongSoPhongCoiThi.ToString(),

                    // Thông tin học sinh
                    tong12 = thongTinTruong.Tong_HS_12.ToString(),
                    khuyetTatNhe = thongTinTruong.Tong_HS_KhuyetTat_Nhe.ToString(),
                    khuyetTatNang = thongTinTruong.Tong_HS_KhuyetTat_Nang.ToString(),
                    khiemThi = thongTinTruong.Tong_HS_KhiemThi.ToString(),
                    hoTroDacBiet = thongTinTruong.Tong_HS_CanHoTroDacBiet.ToString(),
                    noiDungHoTroDacBiet = thongTinTruong.NoiDung_HoTro_HS ?? "Không có",

                    // Thời gian báo cáo
                    ngayBaoCao = reportTime.ToString("dd/MM/yyyy"),
                    thoiGianXuatBaoCao = reportTime.ToString("HH:mm:ss"),
                    ngayThangNam = $"ngày {reportTime.Day} tháng {reportTime.Month} năm {reportTime.Year}"
                };

                // Đảm bảo nhomMons không null để tránh NullReferenceException
                if (nhomMons == null)
                {
                    nhomMons = Enumerable.Empty<KhaoThi_2_THPT_NhomMon_DonViModel>();
                }

                // Chuẩn bị dữ liệu nhóm môn
                //var nhomMonData = nhomMons
                //    .Select((n, index) => new
                //    {
                //        stt = index + 1,
                //        maTruong = n.MaTruong ?? "",
                //        tenNhomluachon = n.TenNhom.ToString(),
                //        monLuaChon1 = n.MonLuaChon_1 ?? "",
                //        monLuaChon2 = n.MonLuaChon_2 ?? "",
                //        soLuong = n.SoLuong
                //    }).ToArray();

                var nhomMonData = nhomMons
                .Select((n, index) => new
                {
                    stt = index + 1,
                    maTruong = n.MaTruong ?? "",
                    tennhom = n.TenNhom.ToString() ?? "Chưa xác định", // Thêm toán tử ?. và giá trị mặc định
                    monLuaChon1 = n.MonLuaChon_1 ?? "Chưa xác định",
                    monLuaChon2 = n.MonLuaChon_2 ?? "Chưa xác định",
                    soLuong = n.SoLuong
                }).ToArray();


                if (nhomConThis == null)
                {
                    nhomConThis = Enumerable.Empty<KhaoThi_5_THPT_ThongTin_ConThiModel>();
                }

                // Chuẩn bị dữ liệu con thi
                var nhomConData = nhomConThis
                    .Select((n, index) => new
                    {
                        stt = index + 1,
                        matruong = n.MaTruong ?? "",
                        cccd = n.CCCD.ToString(),
                        hoten = n.HoTen ?? "",
                        chucvudonvi = n.ChucVuDonVi ?? "",
                        madinhdanhcuacon = n.MaDinhDanhCuaCon ?? "",
                        hotencon = n.HoTenCon ?? "",
                        moiquanhe = n.ChucVuGiaDinh ?? "",
                        kythithamdu = n.KyThiThamDu ?? ""
                    }).ToArray();

                var nhomlanhdaoData = nhomlanhdaodonvis
                    .Select((n, index) => new
                    {
                        stt = index + 1,
                        matruong = n.MaTruong ?? "",
                        cccd = n.CCCD,
                        hoten = n.HoTen ?? "",
                        namsinh = n.NamSinh,
                        chucvu = n.ChucVuDonVi ?? "",
                        coithits10 = n.CoiThiTS10.ToString(),
                        chucvuts10 = n.ChucVuCoiThiTS10.ToString(),
                        lydokothits10 = n.LyDoKhongThamGiaTS10.ToString(),
                        coithithpt = n.CoiThiTHPT.ToString(),
                        chucvuthpt = n.ChucVuCoiThiTHPT.ToString(),
                        lydokothithpt = n.LyDoKhongThamGiaTHPT.ToString()
                    }).ToArray();
                //var nhomConThi = nhomCons
                //.Select((n, index) => new
                //{
                //    stt = index + 1,
                //    maTruong = n.MaTruong ?? "",
                //    tenNhom = n.TenNhom,
                //    monLuaChon1 = n.MonLuaChon_1 ?? "",
                //    monLuaChon2 = n.MonLuaChon_2 ?? "",
                //    soLuong = n.SoLuong
                //}).ToArray();

                // Ghi log trước khi gọi JavaScript
                await _logger.LogErrorAsync($"Bắt đầu tạo báo cáo PDF cho trường {thongTinTruong.TenTruong}", null, nameof(PdfService));

                // Gọi hàm JavaScript để tạo PDF và trả về dưới dạng base64 string
                string base64Pdf = await _jsRuntime.InvokeAsync<string>("pdfGenerator.generatePdfReport", schoolData, nhomMonData, nhomConData, nhomlanhdaoData);

                // Kiểm tra kết quả trả về
                if (string.IsNullOrEmpty(base64Pdf))
                {
                    throw new Exception("JavaScript không trả về dữ liệu PDF");
                }

                // Chuyển đổi base64 string thành byte array
                return Convert.FromBase64String(base64Pdf);
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync($"Lỗi khi tạo báo cáo PDF: {ex.Message}", ex, nameof(PdfService));
                throw;
            }
        }



        /// <summary>
        /// Chuyển đổi mảng byte thành base64 string để truyền cho JavaScript
        /// </summary>
        /// <param name="bytes">Mảng byte cần chuyển đổi</param>
        /// <returns>Chuỗi base64</returns>
        private string ConvertToBase64(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
    }
}