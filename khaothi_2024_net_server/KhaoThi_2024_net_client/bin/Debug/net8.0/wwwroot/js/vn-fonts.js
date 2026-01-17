window.VietnamFonts = {
    initFonts: function (doc) {
        // Đây là mã base64 của font Times New Roman đã rút gọn
        // Trong thực tế, bạn sẽ cần thay thế bằng mã base64 đầy đủ của font Times New Roman
        const timesNormalBase64 = 'data:font/ttf;base64,AAEAAAASAQAABAAgR...[mã base64 dài]...';
        const timesBoldBase64 = 'data:font/ttf;base64,AAEAAAASAQAABAAgR...[mã base64 dài]...';
        const timesItalicBase64 = 'data:font/ttf;base64,AAEAAAASAQAABAAgR...[mã base64 dài]...';
        const timesBoldItalicBase64 = 'data:font/ttf;base64,AAEAAAASAQAABAAgR...[mã base64 dài]...';

        // Tạo ArrayBuffer từ base64 string
        function base64ToArrayBuffer(base64) {
            const binary_string = window.atob(base64.split(',')[1]);
            const len = binary_string.length;
            const bytes = new Uint8Array(len);
            for (let i = 0; i < len; i++) {
                bytes[i] = binary_string.charCodeAt(i);
            }
            return bytes.buffer;
        }

        // Thêm fonts vào jsPDF
        doc.addFileToVFS('times-normal.ttf', timesNormalBase64);
        doc.addFileToVFS('times-bold.ttf', timesBoldBase64);
        doc.addFileToVFS('times-italic.ttf', timesItalicBase64);
        doc.addFileToVFS('times-bold-italic.ttf', timesBoldItalicBase64);

        doc.addFont('times-normal.ttf', 'TimesNewRoman', 'normal');
        doc.addFont('times-bold.ttf', 'TimesNewRoman', 'bold');
        doc.addFont('times-italic.ttf', 'TimesNewRoman', 'italic');
        doc.addFont('times-bold-italic.ttf', 'TimesNewRoman', 'bolditalic');

        // Sử dụng font Times New Roman
        doc.setFont('TimesNewRoman');
    }
};