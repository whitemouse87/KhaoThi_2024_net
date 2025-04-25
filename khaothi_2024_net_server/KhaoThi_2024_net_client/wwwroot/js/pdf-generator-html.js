/**
 * Module tạo báo cáo PDF với định dạng chuẩn A4 và font tiếng Việt
 * Yêu cầu: html2pdf.js
 */
window.pdfGenerator = {
    /**
     * Tạo báo cáo PDF và trả về dưới dạng base64 string
     * @param {Object} schoolData - Dữ liệu thông tin trường học
     * @param {Array} nhomMonData - Dữ liệu danh sách nhóm môn học
     * @returns {Promise<string>} - Promise chứa base64 string của file PDF
     */
    generatePdfReport: function (schoolData, nhomMonData) {
        return new Promise((resolve) => {
            try {
                // Tạo container để chứa báo cáo HTML
                const container = document.createElement('div');
                container.style.visibility = 'hidden';
                container.style.position = 'absolute';
                container.style.left = '-9999px';
                document.body.appendChild(container);

                // Thiết lập style và nội dung cho container
                container.innerHTML = this.createReportHTML(schoolData, nhomMonData);

                // Thiết lập style
                const style = document.createElement('style');
                style.textContent = `
                    @page {
                        size: A4;
                        margin: 10mm 10mm 10mm 10mm;
                    }
                    body {
                        font-family: "Times New Roman", Times, serif;
                        font-size: 12pt;
                        line-height: 1.3;
                        color: #000;
                    }
                    .report-container {
                        width: 100%;
                        box-sizing: border-box;
                        padding: 0;
                        margin: 0;
                    }
                    .header-table {
                        width: 100%;
                        margin-bottom: 15pt;
                        border-collapse: collapse;
                        table-layout: fixed;
                    }
                    .header-left {
                        width: 60%;
                        vertical-align: top;
                        padding-right: 10pt;
                    }
                    .header-right {
                        width: 40%;
                        vertical-align: top;
                    }
                    .header-content p {
                        margin: 2pt 0;
                        line-height: 1.2;
                    }
                    .no-wrap {
                        white-space: nowrap;
                    }
                    .title {
                        font-size: 12pt;
                        font-weight: bold;
                        margin: 10pt 0 5pt 0;
                    }
                    .subtitle {
                        font-size: 12pt;
                        margin: 5pt 0;
                    }
                    .info-table {
                        width: 100%;
                        border-collapse: collapse;
                        margin-top: 5pt;
                        margin-bottom: 10pt;
                        page-break-inside: avoid;
                    }
                    .info-table th, .info-table td {
                        border: 1px solid #000;
                        padding: 4pt;
                        text-align: center;
                    }
                    .info-table th {
                        background-color: #1da787;
                        color: white;
                        font-weight: bold;
                    }
                    .footer {
                        margin-top: 15pt;
                        page-break-inside: avoid;
                        display: flex;
                        justify-content: space-between;
                    }
                    .footer-left {
                        width: 45%;
                        text-align: left;
                    }
                    .footer-right {
                        width: 45%;
                        text-align: center;
                    }
                    .bold {
                        font-weight: bold;
                    }
                    .italic {
                        font-style: italic;
                    }
                    p {
                        margin: 3pt 0;
                    }
                    .page-break {
                        page-break-before: always;
                    }
                    .avoid-break {
                        page-break-inside: avoid;
                    }
                    .header-doc-info p {
                        margin: 1pt 0;
                    }
                `;
                container.appendChild(style);

                // Sử dụng html2pdf để tạo PDF
                const element = container.querySelector('.report-container');

                const options = {
                    margin: [10, 10, 10, 10], // [top, left, bottom, right] margins in mm
                    filename: 'baocao.pdf',
                    image: { type: 'jpeg', quality: 0.98 },
                    html2canvas: {
                        scale: 2,
                        useCORS: true,
                        logging: false,
                        dpi: 192,
                        letterRendering: true
                    },
                    jsPDF: {
                        unit: 'mm',
                        format: 'a4',
                        orientation: 'portrait',
                        compress: true
                    },
                    pagebreak: { mode: ['avoid-all', 'css', 'legacy'] }
                };

                // Tạo PDF
                html2pdf()
                    .from(element)
                    .set(options)
                    .outputPdf('datauristring')
                    .then(pdfBase64 => {
                        // Xóa container
                        document.body.removeChild(container);

                        // Trả về base64 phần sau "data:application/pdf;base64,"
                        resolve(pdfBase64.split(',')[1]);
                    })
                    .catch(error => {
                        console.error("Lỗi khi tạo PDF:", error);
                        document.body.removeChild(container);
                        resolve("");
                    });
            } catch (error) {
                console.error("Lỗi khi chuẩn bị PDF:", error);
                resolve("");
            }
        });
    },

    generateAndDownloadPdfReport: function (schoolData, nhomMonData, fileName) {
        // Tạo container để chứa báo cáo HTML
        const container = document.createElement('div');
        container.style.visibility = 'hidden';
        container.style.position = 'absolute';
        container.style.left = '-9999px';
        document.body.appendChild(container);

        // Thiết lập style và nội dung cho container
        container.innerHTML = this.createReportHTML(schoolData, nhomMonData);

        // Thiết lập style
        const style = document.createElement('style');
        style.textContent = `
            @page {
                size: A4;
                margin: 10mm 10mm 10mm 10mm;
            }
            body {
                font-family: "Times New Roman", Times, serif;
                font-size: 12pt;
                line-height: 1.3;
                color: #000;
            }
            .report-container {
                width: 100%;
                box-sizing: border-box;
                padding: 0;
                margin: 0;
            }
            .header-table {
                width: 100%;
                margin-bottom: 15pt;
                border-collapse: collapse;
                table-layout: fixed;
            }
            .header-left {
                width: 60%;
                vertical-align: top;
                padding-right: 10pt;
            }
            .header-right {
                width: 40%;
                vertical-align: top;
            }
            .header-content p {
                margin: 2pt 0;
                line-height: 1.2;
            }
            .no-wrap {
                white-space: nowrap;
            }
            .title {
                font-size: 12pt;
                font-weight: bold;
                margin: 10pt 0 5pt 0;
            }
            .subtitle {
                font-size: 12pt;
                margin: 5pt 0;
            }
            .info-table {
                width: 100%;
                border-collapse: collapse;
                margin-top: 5pt;
                margin-bottom: 10pt;
                page-break-inside: avoid;
            }
            .info-table th, .info-table td {
                border: 1px solid #000;
                padding: 4pt;
                text-align: center;
            }
            .info-table th {
                background-color: #1da787;
                color: white;
                font-weight: bold;
            }
            .footer {
                margin-top: 15pt;
                page-break-inside: avoid;
                display: flex;
                justify-content: space-between;
            }
            .footer-left {
                width: 45%;
                text-align: left;
            }
            .footer-right {
                width: 45%;
                text-align: center;
            }
            .bold {
                font-weight: bold;
            }
            .italic {
                font-style: italic;
            }
            p {
                margin: 3pt 0;
            }
            .page-break {
                page-break-before: always;
            }
            .avoid-break {
                page-break-inside: avoid;
            }
            .header-doc-info p {
                margin: 1pt 0;
            }
        `;
        container.appendChild(style);

        // Sử dụng html2pdf để tạo PDF
        const element = container.querySelector('.report-container');

        const options = {
            margin: [10, 10, 10, 10], // [top, left, bottom, right] margins in mm
            filename: fileName || 'baocao.pdf',
            image: { type: 'jpeg', quality: 0.98 },
            html2canvas: {
                scale: 2,
                useCORS: true,
                logging: false,
                dpi: 192,
                letterRendering: true
            },
            jsPDF: {
                unit: 'mm',
                format: 'a4',
                orientation: 'portrait',
                compress: true
            },
            pagebreak: { mode: ['avoid-all', 'css', 'legacy'] }
        };

        // Tạo và tải xuống PDF
        html2pdf()
            .from(element)
            .set(options)
            .save()
            .then(() => {
                // Xóa container
                document.body.removeChild(container);
                console.log("PDF đã được tải xuống thành công");
                return true;
            })
            .catch(error => {
                console.error("Lỗi khi tải xuống PDF:", error);
                document.body.removeChild(container);
                return false;
            });
    },

    createReportHTML: function (schoolData, nhomMonData) {
        return `
        <div class="report-container">
            <!-- Header -->
            <table class="header-table">
                <tr>
                    <td class="header-left">
                        <div class="header-content">
                            <p class="no-wrap">SỞ GIÁO DỤC VÀ ĐÀO TẠO</p>
                            <p class="no-wrap">THÀNH PHỐ HỒ CHÍ MINH</p>
                            <p class="bold no-wrap">${schoolData.tenTruong.toUpperCase()}</p>
                        </div>
                    </td>
                    <td class="header-right">
                        <div class="header-content">
                            <p class="bold">CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</p>
                            <p class="bold">Độc lập - Tự do - Hạnh phúc</p>
                        </div>
                    </td>
                </tr>
            </table>
            
            <table class="header-table">
                <tr>
                    <td class="header-left">
                        <div class="header-doc-info">
                            <p>Số:</p>
                            <p>V/v báo cáo số liệu phục vụ công tác</p>
                            <p>tổ chức các kỳ thi năm 2025</p>
                        </div>
                    </td>
                    <td class="header-right">
                        <div class="header-content">
                            <p class="italic">Thành phố Hồ Chí Minh, ${schoolData.ngayThangNam}</p>
                        </div>
                    </td>
                </tr>
            </table>
            
            <!-- Title -->
            <p class="subtitle">Kính gửi: Sở Giáo dục và Đào tạo - phòng Khảo thí và KĐCLGD</p>
            
            <!-- School Info -->
            <p class="title">1. Thông tin đơn vị</p>
            
            <table class="info-table">
                <thead>
                    <tr>
                        <th>Tên trường</th>
                        <th>Quận</th>
                        <th>Mã trường Sở</th>
                        <th>Mã trường Bộ</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>${schoolData.tenTruong || ""}</td>
                        <td>${schoolData.quan || ""}</td>
                        <td>${schoolData.maTruongSo || ""}</td>
                        <td>${schoolData.maTruongBo || ""}</td>
                    </tr>
                </tbody>
            </table>
            
            <p>- Email nhận thông báo: ${schoolData.emailNhanThongBao || ""}</p>
            <p>- Loại hình: ${schoolData.loaiHinhDaoTao || ""}</p>
            <p>- Số điện thoại trường: ${schoolData.sdtTruong || "Chưa nhập"}</p>
            <p>- Số điện thoại phòng hội đồng: ${schoolData.sdtHoiDong || "Chưa nhập"}</p>
            <p>- Địa chỉ trường: ${schoolData.diaChiTruong || "Chưa nhập"}</p>
            
            <!-- Staff Info -->
            <p class="title">2. Thông tin cán bộ nhập liệu</p>
            
            <table class="info-table">
                <tbody>
                    <tr>
                        <td class="bold" style="width: 30%; text-align: left;">Họ tên</td>
                        <td style="text-align: left;">${schoolData.hoTenNguoiNhapLieu || "CHƯA NHẬP"}</td>
                    </tr>
                    <tr>
                        <td class="bold" style="text-align: left;">Chức vụ</td>
                        <td style="text-align: left;">${schoolData.chucVuNhapLieu || "Chưa nhập"}</td>
                    </tr>
                    <tr>
                        <td class="bold" style="text-align: left;">Số di động</td>
                        <td style="text-align: left;">${schoolData.soDiDongNhapLieu || "Chưa nhập"}</td>
                    </tr>
                    <tr>
                        <td class="bold" style="text-align: left;">Email</td>
                        <td style="text-align: left;">${schoolData.emailNhapLieu || "Chưa nhập"}</td>
                    </tr>
                </tbody>
            </table>
            
            <!-- Human Resource Info -->
            <p class="title">3. Thông tin nhân sự</p>
            
            <table class="info-table">
                <thead>
                    <tr>
                        <th style="width: 20%;"></th>
                        <th>Hiệu trưởng</th>
                        <th>Phó hiệu trưởng</th>
                        <th>Giáo viên</th>
                        <th>TTCM</th>
                        <th>Nhân viên</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td class="bold" style="text-align: left;">Số lượng</td>
                        <td>${schoolData.ht || "0"}</td>
                        <td>${schoolData.pht || "0"}</td>
                        <td>${schoolData.giaoVien || "0"}</td>
                        <td>${schoolData.ttcm || "0"}</td>
                        <td>${schoolData.nhanVien || "0"}</td>
                    </tr>
                </tbody>
            </table>
            
            <!-- Facility Info -->
            <p class="title">4. Thông tin đề cử giáo viên, cơ sở vật chất phục vụ công tác coi thi</p>
            
            <table class="info-table">
                <thead>
                    <tr>
                        <th style="width: 20%;"></th>
                        <th>Giáo viên đủ điều kiện tham gia coi thi</th>
                        <th>Số phòng tối đa</th>
                        <th>Số phòng đủ điều kiện phục vụ công tác coi thi</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td class="bold" style="text-align: left;">Số lượng</td>
                        <td>${schoolData.giaoVienCoiThi || "0"}</td>
                        <td>${schoolData.phongToiDa || "0"}</td>
                        <td>${schoolData.phongDuDK || "0"}</td>
                    </tr>
                </tbody>
            </table>
            
            <!-- Student Info -->
            <div class="avoid-break">
                <p class="title">5. Số lượng học sinh lớp 12</p>
                
                <p>- Tổng số học sinh lớp 12: ${schoolData.tong12 || "0"}</p>
                <p>- Trong đó có:</p>
                <p>&nbsp;&nbsp;+ ${schoolData.khuyetTatNang || "0"} học sinh khuyết tật nặng</p>
                <p>&nbsp;&nbsp;+ ${schoolData.khiemThi || "0"} học sinh khiếm thị</p>
                <p>&nbsp;&nbsp;+ ${schoolData.hoTroDacBiet || "0"} học sinh cần hỗ trợ đặc biệt</p>
                <p>- Nội dung hỗ trợ đặc biệt:</p>
                <p>&nbsp;&nbsp;${schoolData.noiDungHoTroDacBiet || "Không có"}</p>
            </div>
            
            <!-- Subject Group Info -->
            <div class="avoid-break">
                <p class="title">6. Danh sách nhóm môn học:</p>
                
                <table class="info-table">
                    <thead>
                        <tr>
                            <th style="width: 10%;">ID</th>
                            <th style="width: 15%;">Tên nhóm</th>
                            <th style="width: 30%;">Môn lựa chọn 1</th>
                            <th style="width: 30%;">Môn lựa chọn 2</th>
                            <th style="width: 15%;">Số lượng</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${this.createSubjectGroupRows(nhomMonData)}
                    </tbody>
                </table>
                
                ${this.createTotalRow(nhomMonData)}
            </div>
            
            <p>./.</p>
            
            <div class="footer avoid-break">
                <div class="footer-left">
                    <p class="bold italic">Nơi nhận:</p>
                    <p class="italic">- Như trên;</p>
                    <p class="italic">- Lưu: VT, Hiệu trưởng.</p>
                </div>
                <div class="footer-right">
                    <p class="bold">HIỆU TRƯỞNG</p>
                    <p style="margin-top: 25pt;" class="bold">Nguyễn Văn Hiếu</p>
                </div>
            </div>
        </div>
        `;
    },

    createSubjectGroupRows: function (nhomMonData) {
        if (!nhomMonData || nhomMonData.length === 0) {
            return '<tr><td colspan="5" style="text-align: center;">Không có dữ liệu</td></tr>';
        }

        return nhomMonData.map(item => `
            <tr>
                <td>${item.stt}</td>
                <td>${item.tenNhom}</td>
                <td>${item.monLuaChon1}</td>
                <td>${item.monLuaChon2}</td>
                <td>${item.soLuong}</td>
            </tr>
        `).join('');
    },

    createTotalRow: function (nhomMonData) {
        if (!nhomMonData || nhomMonData.length === 0) {
            return '';
        }

        const totalStudents = nhomMonData.reduce((sum, item) => sum + (parseInt(item.soLuong) || 0), 0);
        return `<p style="text-align: right; margin-top: 5pt; margin-bottom: 10pt;"><span class="bold">Tổng số học sinh: ${totalStudents}</span></p>`;
    }
};