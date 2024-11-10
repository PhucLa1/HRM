using OfficeOpenXml;

namespace HRM.Repositories.Helper
{
    public static class HandleExcel
    {
        public static void CreateExcelFile(string[] features, object[][] data)
        {
            //Tao file name 
            string fileName = DateTime.Now.Ticks.ToString() + ".xlsx";

            // Đường dẫn đến thư mục wwwroot
            var exactPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", fileName);

            // Tạo một file Excel mới
            var package = new ExcelPackage();

            // Tạo một sheet mới
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");

            // Đặt tiêu đề cho sheet
            int featuresLength = features.Length;
            int dataLength = data.Length;
            for (int i = 1; i <= featuresLength; i++)
            {
                worksheet.Cells[1, i + 1].Value = features[i]; //Hàng 1 , bắt đầu từ cột 2
            }

            // Đặt dữ liệu cho sheet
            for (int i = 1; i <= dataLength; i++)
            {
                for (int j = 1; j <= data[i].Length; j++)
                {
                    worksheet.Cells[i + 1, j].Value = data[i - 1][j - 1] == null ? "" : data[i - 1][j - 1]; //Hàng bắt đầu từ 2, cột bắt đầu từ 1
                }
            }

            // Lưu file Excel
            package.SaveAs(new FileInfo(exactPath));
        }
    }
}
