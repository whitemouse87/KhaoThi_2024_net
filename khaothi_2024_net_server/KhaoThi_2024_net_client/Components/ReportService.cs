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
                // Tạo DateTime hiện tại một lần để đảm bảo tính nhất quán
                DateTime reportTime = DateTime.Now;

                // Đọc template
                byte[] templateBytes = await _httpClient.GetByteArrayAsync(templateUrl);

                using var templateStream = new MemoryStream(templateBytes);
                using var resultStream = new MemoryStream();

                // Copy template vào stream kết quả
                templateStream.CopyTo(resultStream);
                resultStream.Position = 0;

                // Chuẩn bị tất cả placeholder và giá trị tương ứng
                Dictionary<string, string> placeholders = PrepareReportPlaceholders(thongTinTruong, reportTime);

                // Mở template và thay thế các placeholder
                using (WordprocessingDocument doc = WordprocessingDocument.Open(resultStream, true))
                {
                    // Cập nhật document properties
                    if (doc.PackageProperties != null)
                    {
                        doc.PackageProperties.Modified = reportTime;
                    }

                    // Thay đổi cách tiếp cận - Xử lý ở cấp đoạn văn để bảo toàn định dạng
                    ReplaceTextInParagraphs(doc.MainDocumentPart, placeholders);

                    // Xử lý header và footer
                    if (doc.MainDocumentPart?.HeaderParts != null)
                    {
                        foreach (var headerPart in doc.MainDocumentPart.HeaderParts)
                        {
                            ReplaceTextInParagraphs(headerPart, placeholders);
                        }
                    }

                    if (doc.MainDocumentPart?.FooterParts != null)
                    {
                        foreach (var footerPart in doc.MainDocumentPart.FooterParts)
                        {
                            ReplaceTextInParagraphs(footerPart, placeholders);
                        }
                    }
                }

                // Return the report as byte array
                resultStream.Position = 0;
                return resultStream.ToArray();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync($"Lỗi khi tạo file Word: {ex.Message}", ex, "WordReportService");
                throw;
            }
        }

        private Dictionary<string, string> PrepareReportPlaceholders(KhaoThi_1_THPT_ThongTin_DonViModel thongTinTruong, DateTime reportTime)
        {
            return new Dictionary<string, string>
            {
                {"{{MaTruong}}", thongTinTruong?.MaTruong ?? ""},
                {"**{{MaTruong}}**", $"**{thongTinTruong?.MaTruong ?? ""}**"}, // Xử lý variant có định dạng đậm
                {"{{TenTruong}}", thongTinTruong?.TenTruong ?? ""},
                {"**{{TenTruong}}**", $"**{thongTinTruong?.TenTruong ?? ""}**"}, // Xử lý variant có định dạng đậm
                {"{{NgayBaoCao}}", reportTime.ToString("dd/MM/yyyy")},
                {"{{ThoiGianXuatBaoCao}}", reportTime.ToString("HH:mm:ss")},
                {"Click or tap here to enter text.", ""}
            };
        }

        // Phương thức để xử lý thay thế text ở cấp đoạn văn để bảo toàn định dạng
        private void ReplaceTextInParagraphs(OpenXmlPart part, Dictionary<string, string> placeholders)
        {
            if (part == null) return;

            // Cách tiếp cận 1: Thay thế ở cấp XML (đơn giản và hiệu quả nhất)
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
                Console.WriteLine($"Lỗi khi thay thế text trong XML: {ex.Message}");

                // Thử với cách 2 nếu cách 1 thất bại
                TryReplacePlaceholdersInParagraphs(part, placeholders);
            }
        }

        // Phương thức dự phòng nếu phương thức XML thất bại
        private void TryReplacePlaceholdersInParagraphs(OpenXmlPart part, Dictionary<string, string> placeholders)
        {
            try
            {
                // Lấy tất cả các đoạn văn
                var paragraphs = part.RootElement.Descendants<Paragraph>().ToList();

                foreach (var paragraph in paragraphs)
                {
                    // Tìm các placeholder trong đoạn
                    string paraText = string.Join("", paragraph.Descendants<Text>().Select(t => t.Text));
                    bool needReplace = false;

                    foreach (var placeholder in placeholders)
                    {
                        if (paraText.Contains(placeholder.Key))
                        {
                            needReplace = true;
                            break;
                        }
                    }

                    if (needReplace)
                    {
                        // Lưu tất cả nội dung của đoạn
                        string paraContent = paraText;

                        // Thay thế tất cả placeholder
                        foreach (var placeholder in placeholders)
                        {
                            paraContent = paraContent.Replace(placeholder.Key, placeholder.Value);
                        }

                        // Xóa tất cả run hiện tại
                        var runs = paragraph.Elements<Run>().ToList();
                        foreach (var run in runs)
                        {
                            run.Remove();
                        }

                        // Tạo run mới với text đã thay thế
                        var newRun = new Run(new Text(paraContent));

                        // Bảo toàn định dạng đậm nếu có chứa "**"
                        if (paraText.Contains("**"))
                        {
                            newRun.RunProperties = new RunProperties(new Bold());
                        }

                        paragraph.AppendChild(newRun);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thay thế trong đoạn văn: {ex.Message}");
            }
        }
    }
}
