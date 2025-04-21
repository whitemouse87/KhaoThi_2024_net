using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;
using KhaoThi_2024_net_client.Services.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace KhaoThi_2024_net_client.Components
{
    public interface IReportService
    {
        Task<byte[]> GenerateWordReportAsync(string templateUrl, KhaoThi_1_THPT_ThongTin_DonViModel thongTinTruong);
    }
    public class ReportService : IReportService
    {
        private readonly HttpClient _httpClient;
        private readonly ILoggingService _logger;

        public ReportService(HttpClient httpClient, ILoggingService logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<byte[]> GenerateWordReportAsync(string templateUrl, KhaoThi_1_THPT_ThongTin_DonViModel thongTinTruong)
        {
            try
            {
                DateTime reportTime = DateTime.Now;
                byte[] templateBytes = await _httpClient.GetByteArrayAsync(templateUrl);

                using var templateStream = new MemoryStream(templateBytes);
                using var resultStream = new MemoryStream();

                templateStream.CopyTo(resultStream);
                resultStream.Position = 0;

                // Chuẩn bị các placeholder
                Dictionary<string, string> placeholders = PrepareReportPlaceholders(thongTinTruong, reportTime);

                using (WordprocessingDocument doc = WordprocessingDocument.Open(resultStream, true))
                {
                    if (doc.PackageProperties != null)
                    {
                        doc.PackageProperties.Modified = reportTime;
                    }

                    // Mở rộng danh sách placeholders để tìm các biến thể có thể có
                    var expandedPlaceholders = ScanForActualPlaceholders(doc.MainDocumentPart, placeholders);

                    // Thay thế text
                    ReplaceTextInParagraphs(doc.MainDocumentPart, expandedPlaceholders);

                    // Xử lý header và footer
                    if (doc.MainDocumentPart?.HeaderParts != null)
                    {
                        foreach (var headerPart in doc.MainDocumentPart.HeaderParts)
                        {
                            ReplaceTextInParagraphs(headerPart, expandedPlaceholders);
                        }
                    }

                    if (doc.MainDocumentPart?.FooterParts != null)
                    {
                        foreach (var footerPart in doc.MainDocumentPart.FooterParts)
                        {
                            ReplaceTextInParagraphs(footerPart, expandedPlaceholders);
                        }
                    }
                }

                resultStream.Position = 0;
                return resultStream.ToArray();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync($"Lỗi khi tạo file Word: {ex.Message}", ex, "WordReportService");
                throw;
            }
        }
        private Dictionary<string, string> ScanForActualPlaceholders(OpenXmlPart part, Dictionary<string, string> originalPlaceholders)
        {
            try
            {
                var result = new Dictionary<string, string>(originalPlaceholders);
                string xml;
                using (var sr = new StreamReader(part.GetStream()))
                {
                    xml = sr.ReadToEnd();
                }

                // Tìm tất cả các chuỗi có thể là placeholder
                foreach (var key in originalPlaceholders.Keys.ToList())
                {
                    // Loại bỏ {{ và }} để tìm tên placeholder
                    string placeholderName = key.Replace("{{", "").Replace("}}", "");

                    // Tìm các biến thể có thể có
                    var possibleVariations = new List<string>
            {
                $"{{{{{placeholderName}}}}}",
                $"{{ {{{placeholderName}}} }}",
                $"{{{{ {placeholderName} }}}}"
                // Thêm các biến thể khác nếu cần
            };

                    foreach (var variation in possibleVariations)
                    {
                        if (xml.Contains(variation) && !originalPlaceholders.ContainsKey(variation))
                        {
                            Console.WriteLine($"Found variant: {variation} for {key}");
                            result[variation] = originalPlaceholders[key];
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error scanning for placeholders: {ex.Message}");
                return originalPlaceholders;
            }
        }

        private Dictionary<string, string> PrepareReportPlaceholders(KhaoThi_1_THPT_ThongTin_DonViModel thongTinTruong, DateTime reportTime)
        {
            return new Dictionary<string, string>
            {
                //{"{{MaTruong}}", thongTinTruong?.MaTruong ?? ""},
                //{"**{{MaTruong}}**", $"**{thongTinTruong?.MaTruong ?? ""}**"}, // Xử lý variant có định dạng đậm
                //{"{{TenTruong}}", thongTinTruong?.TenTruong ?? ""},
                //{"**{{TenTruong}}**", $"**{thongTinTruong?.TenTruong ?? ""}**"}, // Xử lý variant có định dạng đậm
                //{"{{NgayBaoCao}}", reportTime.ToString("dd/MM/yyyy")},
                //{"{{ThoiGianXuatBaoCao}}", reportTime.ToString("HH:mm:ss")},
               
                {"{{TenTruong}}", thongTinTruong?.TenTruong ?? ""},
                {"**{{TenTruong}}**", $"**{thongTinTruong?.TenTruong.ToString().ToUpper() ?? ""}**"},
                 {"{{Quận}}", thongTinTruong?.QuanDangKyDuThi.ToString() ?? ""},
                 {"**{{Quận}}**", $"**{thongTinTruong?.QuanDangKyDuThi.ToString() ?? ""}**"},


                {"{{MaTruongSo}}", thongTinTruong?.MaTruong ?? ""},
                {"**{{MaTruongSo}}**", $"**{thongTinTruong?.MaTruong ?? ""}**"},
                {"{{MaTruongBo}}", thongTinTruong?.MaTruongBo ?? ""},
                {"**{{MaTruongBo}}**", $"**{thongTinTruong?.MaTruongBo ?? ""}**"},
                {"{{EmailNhanThongBao}}", thongTinTruong?.EmailNhanThongBao ?? ""},
                {"{{LoaiHinhDaoTao}}", thongTinTruong?.LoaiHinhDaoTao ?? ""},
                {"{{SDTTruong}}", thongTinTruong?.SDTTruong ?? ""},
                {"{{SDTHoiDong}}", thongTinTruong?.SDTPhongHoiDong ?? ""},
                {"{{DiaChiTruong}}", thongTinTruong?.DiaChiTruong ?? ""},
        
                // Thông tin cán bộ nhập liệu
                {"{{HoTenNguoiNhapLieu}}", thongTinTruong?.HoTenNhapLieu ?? ""},
                {"**{{HoTenNguoiNhapLieu}}**", $"**{thongTinTruong?.HoTenNhapLieu ?? ""}**"},
                {"{{ChucVuNhapLieu}}", thongTinTruong?.ChucVuNhapLieu ?? ""},
                {"**{{ChucVuNhapLieu}}**", $"**{thongTinTruong?.ChucVuNhapLieu ?? ""}**"},
                {"{{SoDiDongNhapLieu}}", thongTinTruong?.SDTDiDongNhapLieu ?? ""},
                {"**{{SoDiDongNhapLieu}}**", $"**{thongTinTruong?.SDTDiDongNhapLieu ?? ""}**"},
                {"{{EmailNhapLieu}}", thongTinTruong?.EmailNhapLieu ?? ""},
                {"**{{EmailNhapLieu}}**", $"**{thongTinTruong?.EmailNhapLieu ?? ""}**"},
        
                // Thông tin nhân sự - fix cho non-nullable int
                {"{{HT}}", thongTinTruong != null ? thongTinTruong.TongHieuTruong.ToString() : ""},
                {"{{PHT}}", thongTinTruong != null ? thongTinTruong.TongPhoHieuTruong.ToString() : ""},
                {"{{GiaoVien}}", thongTinTruong != null ? thongTinTruong.TongGiaoVien.ToString() : ""},
                {"{{TTCM}}", thongTinTruong != null ? thongTinTruong.TongGiaoVien_ToTruong.ToString() : ""},
                {"{{NhanVien}}", thongTinTruong != null ? thongTinTruong.TongNhanVien.ToString() : ""},
        
                // Thông tin đề cử giáo viên, cơ sở vật chất - fix cho non-nullable int
                {"{{GiaoVienCoiThi}}", thongTinTruong != null ? thongTinTruong.TongGiaoVien_CoiThi.ToString() : ""},
                {"{{PhongToiDa}}", thongTinTruong != null ? thongTinTruong.TongSoPhongToiDa.ToString() : ""},
                {"{{PhongDuDK}}", thongTinTruong != null ? thongTinTruong.TongSoPhongCoiThi.ToString() : ""},
                //Học sinh 12 và học sinh đặc biệt

                 {"{{Tong12}}", thongTinTruong != null ? thongTinTruong.Tong_HS_12.ToString() : ""},
                 {"{{KhuyetTatNhe}}", thongTinTruong != null ? thongTinTruong.Tong_HS_KhuyetTat_Nhe.ToString() : ""},                
                 {"{{khuyettatnang}}", thongTinTruong != null ? thongTinTruong.Tong_HS_KhuyetTat_Nang.ToString() : ""},
                 {"{{KhiemThi}}", thongTinTruong != null ? thongTinTruong.Tong_HS_KhiemThi.ToString() : ""},
                 {"{{HoTroDacBiet}}", thongTinTruong != null ? thongTinTruong.Tong_HS_CanHoTroDacBiet.ToString() : ""},


                 {"{{NoiDungHoTroDacBiet}}", thongTinTruong?.NoiDung_HoTro_HS ?? ""},
                // Ngày tháng
                 {"ngày tháng 4 năm 2025", $"ngày {reportTime.Day} tháng {reportTime.Month} năm {reportTime.Year}"},
                 {"{{NgayBaoCao}}", reportTime.ToString("dd/MM/yyyy")},
                 {"{{ThoiGianXuatBaoCao}}", reportTime.ToString("HH:mm:ss")},
        
                // Xóa text placeholder mặc định
                 {"Click or tap here to enter text.", ""}

            };
           
        }

        // Phương thức để xử lý thay thế text ở cấp đoạn văn để bảo toàn định dạng
        private void ReplaceTextInParagraphs(OpenXmlPart part, Dictionary<string, string> placeholders)
        {
            if (part == null) return;

            // Thêm logging để theo dõi placeholders
            foreach (var key in placeholders.Keys)
            {
                Console.WriteLine($"Checking placeholder: {key} => {placeholders[key]}");
            }

            // Cách tiếp cận 1: Thay thế ở cấp XML
            try
            {
                string xml;
                using (var sr = new StreamReader(part.GetStream()))
                {
                    xml = sr.ReadToEnd();
                }

                bool hasChanges = false;
                foreach (var placeholder in placeholders)
                {
                    if (xml.Contains(placeholder.Key))
                    {
                        Console.WriteLine($"Found and replacing: {placeholder.Key}");
                        xml = xml.Replace(placeholder.Key, placeholder.Value);
                        hasChanges = true;
                    }
                }

                if (hasChanges)
                {
                    using var sw = new StreamWriter(part.GetStream(FileMode.Create));
                    sw.Write(xml);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error replacing text in XML: {ex.Message}");
                TryReplacePlaceholdersInParagraphs(part, placeholders);
            }
        }

        // Phương thức dự phòng nếu phương thức XML thất bại
        private void TryReplacePlaceholdersInParagraphs(OpenXmlPart part, Dictionary<string, string> placeholders)
        {
            try
            {
                // Xử lý từng đoạn văn
                var paragraphs = part.RootElement.Descendants<Paragraph>().ToList();

                foreach (var paragraph in paragraphs)
                {
                    // Lấy tất cả Text nodes trong đoạn
                    var textNodes = paragraph.Descendants<Text>().ToList();

                    // Tạo chuỗi văn bản đầy đủ của đoạn
                    string fullText = string.Join("", textNodes.Select(t => t.Text));

                    // Kiểm tra xem có placeholder nào trong đoạn này không
                    bool hasChanges = false;
                    string modifiedText = fullText;

                    foreach (var placeholder in placeholders)
                    {
                        if (fullText.Contains(placeholder.Key))
                        {
                            modifiedText = modifiedText.Replace(placeholder.Key, placeholder.Value);
                            hasChanges = true;
                            Console.WriteLine($"Thay thế '{placeholder.Key}' bằng '{placeholder.Value}'");
                        }
                    }

                    // Nếu có thay đổi, cập nhật lại nội dung đoạn
                    if (hasChanges)
                    {
                        // Xóa tất cả Run hiện tại
                        foreach (var run in paragraph.Elements<Run>().ToList())
                        {
                            run.Remove();
                        }

                        // Tạo Run mới với nội dung đã thay thế
                        var newRun = new Run(new Text(modifiedText));
                        paragraph.AppendChild(newRun);
                    }
                    else
                    {
                        // Xử lý trường hợp placeholder bị chia cắt giữa các Run
                        string combinedText = "";
                        int combinedLength = 0;
                        List<Run> runsToProcess = new List<Run>();

                        // Thu thập các Run liên tiếp để tìm placeholder
                        foreach (var run in paragraph.Elements<Run>())
                        {
                            var textNode = run.Descendants<Text>().FirstOrDefault();
                            if (textNode != null)
                            {
                                combinedText += textNode.Text;
                                combinedLength += textNode.Text.Length;
                                runsToProcess.Add(run);

                                // Kiểm tra xem có placeholder nào trong văn bản kết hợp
                                bool foundPlaceholder = false;
                                foreach (var placeholder in placeholders)
                                {
                                    if (combinedText.Contains(placeholder.Key))
                                    {
                                        // Thay thế placeholder
                                        string replacedText = combinedText.Replace(placeholder.Key, placeholder.Value);

                                        // Xóa tất cả Run đã thu thập
                                        foreach (var collectedRun in runsToProcess)
                                        {
                                            collectedRun.Remove();
                                        }

                                        // Tạo Run mới với văn bản đã thay thế
                                        var newRun = new Run(new Text(replacedText));
                                        paragraph.AppendChild(newRun);

                                        foundPlaceholder = true;
                                        Console.WriteLine($"Thay thế placeholder bị chia cắt: '{placeholder.Key}'");
                                        break;
                                    }
                                }

                                if (foundPlaceholder)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thay thế placeholder trong đoạn văn: {ex.Message}");
            }
        }
    }
}
