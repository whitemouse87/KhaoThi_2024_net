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
    generatePdfReport: function (schoolData, nhomMonData, nhomconthiData, nhomlanhdaoData, nhomtruongdiemData) {
        return new Promise((resolve) => {
            try {
                // Tạo container để chứa báo cáo HTML
                const container = document.createElement('div');
                container.style.visibility = 'hidden';
                container.style.position = 'absolute';
                container.style.left = '-9999px';
                document.body.appendChild(container);

                // Thiết lập style và nội dung cho container
                container.innerHTML = this.createReportHTML(schoolData, nhomMonData, nhomconthiData, nhomlanhdaoData, nhomtruongdiemData);

                // Thiết lập style
                const style = document.createElement('style');
                style.textContent = `
                    @page {
                        size: A4;
                        margin: 2mm 2mm 2mm 3mm;
                    }
                    body {
                        font-family: "Times New Roman", Times, serif;
                        font-size: 11pt;
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
                        width: 45%;
                        vertical-align: top;

                        padding-right: 0pt;
                        text-align:center;
                    }
                    .bold {
                        font-weight: bold;
                     }
                    .header-right {
                        width: 55%;
                        vertical-align: top;
                        text-align:center;
                        font-weight: bold;
                    }
                    .header-content p {
                        margin: 3pt 0;
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
                        text-align: center;
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
                    .wrap-text {
                        word-wrap: break-word;
                        white-space: normal;
                        overflow-wrap: break-word;
                        font-weight: bold;
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



    createReportHTML: function (schoolData, nhomMonData, nhomconthiData, nhomlanhdaoData, nhomtruongdiemData) {
        return `
        <div class="report-container">
            <!-- Header -->
            <table class="header-table">
            <tr>
                <td class="header-left">
                    <div class="header-content">
                        <p class="bold no-wrap">SỞ GIÁO DỤC VÀ ĐÀO TẠO</p>
                        <p class="bold no-wrap">THÀNH PHỐ HỒ CHÍ MINH</p>
                        <p class="bold no-wrap">${schoolData.tenTruong.toUpperCase()}</p>
                    </div>
                </td>
                <td class="header-right">
                    <div class="header-content">
                        <p class=""bold no-wrap">CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</p>
                        <p class=""bold no-wrap">Độc lập - Tự do - Hạnh phúc</p>
                    </div>
                </td>
            </tr>
        </table>

        <table class="header-table">
            <tr>
                <td class="header-left">
                    
                    <div class="header-doc-info">
                        <p align="left">Số:</p>
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
                            <th style="width: 10%;">STT</th>
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
                
                ${this.createTotalRow(schoolData, nhomMonData)}
            </div>
              <!-- Thông tin con thi -->
               <div class="avoid-break">
                <p class="title">7. Danh sách cán bộ, giáo viên, nhân viên có con, người thân dự thi các kỳ thi của Thành phố:</p>

                <table class="info-table">
                    <thead>
                        <tr>
                            <th style="width: 5%;">STT</th>
                            <th style="width: 10%;">CCCD cán bộ, giáo viên, nhân viên</th>
                            <th style="width: 20%;">Họ tên</th>
                            <th style="width: 10%;">Chức vụ đơn vị</th>
                            <th style="width: 15%;">Mã định danh người thân</th>
                            <th style="width: 20%;">Họ tên người thân</th>
                            <th style="width: 10%;">Mối quan hệ</th>
                            <th style="width: 10%;">Kỳ thi tham dự</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${this.createConThiGroupRows(nhomconthiData)}
                    </tbody>
                </table>
                
               
             </div>
           
              <!-- Thông tin truong diem -->
              <div class="avoid-break">
                <p class="title">8. Danh sách cán bộ, giáo viên, nhân viên có con, người thân dự thi các kỳ thi của Thành phố:</p>
                <p class="bold">Lưu ý: Tất cả thành viên thuộc Ban giám hiệu phải đăng ký tham gia coi thi, trừ trường hợp có người thân dự thi. Các trường hợp khác phải có văn bản kèm minh chứng gửi Ban giám đốc xin ý kiến.</p>
                <table class="info-table">
                    <thead>
                        <tr>
                            <th style="width: 5%;">STT</th>
                            <th style="width: 10%;">CCCD</th>
                            <th style="width: 25%;">Họ tên</th>
                            <th style="width: 5%">Năm sinh</th>
                            <th style="width: 11%;">Chức vụ</th>
                            <th style="width: 11%;">Coi thi TS10</th>
                            <th style="width: 11%;">Chức vụ TS10</th>                         
                            <th style="width: 11%;">Coi thi THPT</th>
                            <th style="width: 11%;">Chức vụ THPT</th>                           
                        </tr>
                    </thead>
                    <tbody>
                        ${this.createTruongDiemGroupRows(nhomtruongdiemData)}
                    </tbody>
                </table>
                 
               
             </div>
               
             <!-- Thông tin lanh dao -->
               <div class="avoid-break">
                <p class="title">9. Danh sách lãnh đạo đơn vị:</p>

                <table class="info-table">
                    <thead>
                        <tr>
                            <th style="width: 5%;">STT</th>
                            <th style="width: 10%;">CCCD</th>
                            <th style="width: 25%;">Họ tên</th>
                            <th style="width: 5%">Năm sinh</th>
                            <th style="width: 10%;">Chức vụ</th>
                            <th style="width: 15%;">Di động</th>
                            <th style="width: 20%;">Email</th>
                            <th style="width: 5%;">Cụm chuyên môn</th>
                            <th style="width: 5%;">Chức vụ cụm chuyên môn</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${this.createLanhDaoGroupRows(nhomlanhdaoData)}
                    </tbody>
                </table>               
                <p class="bold">Lưu ý: Tất cả thành viên thuộc Ban giám hiệu phải khai báo đầy đủ thông tin SĐT, Email để phục vụ công tác liên hệ tương lai khi xảy ra sự cố.</p>
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
                    <p style="margin-top: 25pt;" class="bold"></p>
                </div>
            </div>
        </div>
          </div>
        `;


    },
    //Tạo danh sách nhóm môn
    //createSubjectGroupRows: function (nhomMonData) {
    //    if (!nhomMonData || nhomMonData.length === 0) {
    //        return '<tr><td colspan="5" style="text-align: center;">Không có dữ liệu</td></tr>';
    //    }

    //    return nhomMonData.map(item => `
    //        <tr>
    //            <td>${item.stt}</td>
    //            <td>${item.tennhomluachon}</td>
    //            <td>${item.monLuaChon1}</td>
    //            <td>${item.monLuaChon2}</td>
    //            <td>${item.soLuong}</td>
    //        </tr>
    //    `).join('');
    //},
    createSubjectGroupRows: function (nhomMonData) {
        if (!nhomMonData || nhomMonData.length === 0) {
            return '<tr><td colspan="5" style="text-align: center;">Không có dữ liệu</td></tr>';
        }

        return nhomMonData.map(item => `
        <tr>
            <td>${item.stt || ''}</td>
            <td>${item.tennhom || 'Chưa xác định'}</td>
            <td>${item.monLuaChon1 || 'Chưa xác định'}</td>
            <td>${item.monLuaChon2 || 'Chưa xác định'}</td>
            <td>${item.soLuong || 0}</td>
        </tr>
    `).join('');
    },

    createTotalRow: function (schoolData, nhomMonData) {
        if (!nhomMonData || nhomMonData.length === 0) {
            return '';
        }
        const totalStudents = nhomMonData.reduce((sum, item) => sum + (parseInt(item.soLuong) || 0), 0);
        if (schoolData.tong12 !== totalStudents) {
            return `<p style="text-align: center; margin-top: 5pt; margin-bottom: 10pt; background-color: yellow;">
                      <span class="bold" style="color: red; font-size: larger;">Tổng số học sinh của các nhóm môn học: ${totalStudents} không bằng với tổng số học sinh 12: ${schoolData.tong12} đã khai báo. Cần xem lại</span>
                    </p>`;
        }
        return `<p style="text-align: right; margin-top: 5pt; margin-bottom: 10pt;"><span class="bold">Tổng số học sinh: ${totalStudents}</span></p>`;
    },

    //Tạo danh sách con thi
    createConThiGroupRows: function (nhomConThiData) {
        if (!nhomConThiData || nhomConThiData.length === 0) {
            return '<tr><td colspan="8" style="text-align: center;">Không có dữ liệu</td></tr>';
        }

        return nhomConThiData.map(item => `
            <tr>
                <td>${item.stt}</td>
                <td>${item.cccd}</td>
                <td>${item.hoten}</td>
                <td>${item.chucvudonvi}</td>
                <td>${item.madinhdanhcuacon}</td>
                <td>${item.hotencon}</td>
                <td>${item.moiquanhe}</td>
                <td>${item.kythithamdu}</td>
            </tr>
        `).join('');
    },
    //Tạo danh sách lãnh đạo
    createLanhDaoGroupRows: function (nhomlanhdaoData) {
        if (!nhomlanhdaoData || nhomlanhdaoData.length === 0) {
            return '<tr><td colspan="9" style="text-align: center;">Không có dữ liệu</td></tr>';
        }

        return nhomlanhdaoData.map(item => `
            <tr>
                <td>${item.stt}</td>
                <td>${item.cccd}</td>
                <td>${item.hoten}</td>
                 <td>${item.namsinh}</td>
                <td>${item.chucvu}</td>
                <td>${item.didong}</td>
                <td>${item.email}</td>
                <td>${item.cumchuyenmon}</td>
                <td>${item.chucvucumchuyenmon}</td>
            </tr>
        `).join('');
    },
    createTruongDiemGroupRows: function (nhomtruongdiemData) {
        if (!nhomtruongdiemData || nhomtruongdiemData.length === 0) {
            return '<tr><td colspan="11" style="text-align: center;">Không có dữ liệu</td></tr>';
        }

        return nhomtruongdiemData.map(item => `
            <tr>
                <td>${item.stt}</td>
                <td>${item.cccd}</td>
                <td>${item.hoten}</td>
                 <td>${item.namsinh}</td>
                <td>${item.chucvu}</td>
                <td style="font-weight:bold">${item.coithits10 === 'True' || item.coithits10 === true || item.coithits10 === 'true' ? 'Tham gia' : 'Không tham gia'}</td>
                <td>${item.chucvuts10}</td>        
                <td style="font-weight:bold">${item.coithithpt === 'True' || item.coithithpt === true || item.coithithpt === 'true' ? 'Tham gia' : 'Không tham gia'}</td>
                 <td>${item.chucvuthpt}</td>
                
            </tr>
          
            <tr><td colspan="9" style="word-wrap: break-word; font-weight:bold; text-align: left; padding: 6pt">${item.coithits10 === 'True' || item.coithits10 === true || item.coithits10 === 'true' ? 'Đã đăng ký tham gia TS10' : `Lý do không tham gia TS10: ${item.lydokothits10} 
            (Cần có văn bản của đơn vị xin ý kiến Ban giám đốc Sở, trừ trường hợp có người thân tham dự các kỳ thi và đã được khai báo ở mục 7.`}</td></tr>
             <tr><td colspan="9" style="word-wrap: break-word; font-weight:bold; text-align: left; padding: 6pt">${item.coithithpt === 'True' || item.coithithpt === true || item.coithithpt === 'true' ? 'Đã đăng ký tham gia THPT' : `Lý do không tham gia THPT: ${item.lydokothithpt} 
             (Cần có văn bản của đơn vị xin ý kiến Ban giám đốc Sở, trừ trường hợp có người thân tham dự các kỳ thi và đã được khai báo ở mục 7.`}</td></tr>
        `).join('');
    }
};

