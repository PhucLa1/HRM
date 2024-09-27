using Microsoft.AspNetCore.Http;

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
                fileName = DateTime.Now.Ticks.ToString() + extension + folder;

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
