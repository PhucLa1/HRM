using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HPSF;

namespace HRM.Repositories.Helper
{
    public static class HandleFile
    {
        private const string FILE_NOT_FOUND = "Không tìm thấy file";
        public static string UPLOAD(string folder, IFormFile file)
        {
            string fileName = "";
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                fileName = DateTime.Now.Ticks.ToString() + extension;

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), folder);

                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                var exactPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, fileName);

                using (var stream = new FileStream(exactPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return fileName;
        }
        public static IActionResult DownloadFile(string folder, string fileName)
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, fileName);

                // Kiểm tra xem file có tồn tại không
                if (!System.IO.File.Exists(filePath))
                {
                    return new NotFoundResult();
                }

                // Đọc file và trả về file cho client
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return new FileContentResult(fileBytes, "application/octet-stream")
                {
                    FileDownloadName = fileName
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); // Trả về mã lỗi 500 nếu có ngoại lệ
            }
        }
        public static string READ_FILE(string folder, string fileName)
        {
            string content = "";
            try
            {
                // Đường dẫn chính xác đến tệp
                var exactPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, fileName);

                if (File.Exists(exactPath))
                {
                    using (var reader = new StreamReader(exactPath))
                    {
                        content = reader.ReadToEnd();
                    }
                }
                else
                {
                    throw new FileNotFoundException(FILE_NOT_FOUND, fileName);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return content;
        }

        public static void DELETE_FILE(string folder, string fileName)
        {
            try
            {
                // Kết hợp đường dẫn thư mục và tên file để có đường dẫn chính xác
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, fileName);

                // Kiểm tra nếu file tồn tại
                if (File.Exists(filePath))
                {
                    // Xóa file
                    File.Delete(filePath);
                }
                else
                {
                    // Nếu file không tồn tại, bạn có thể ném ra ngoại lệ hoặc xử lý logic khác tùy nhu cầu
                    throw new FileNotFoundException(FILE_NOT_FOUND, fileName);
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có
                throw new Exception(ex.Message);
            }
        }
    }
}
