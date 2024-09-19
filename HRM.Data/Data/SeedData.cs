using HRM.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HRM.Data.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new HRMDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<HRMDbContext>>()))
            {
                if (context.Admins.Any())
                {
                    //Mới có thể xóa đi thêm lại từ đầu
                    /*
                    context.Admins.RemoveRange();
                    context.SaveChangesAsync();
                    */
                    return;   // DB has been seeded
                }
                context.Admins.AddRangeAsync(
                    new Admin { Email = "phucminhbeos@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("Phucdeptrai") },
                    new Admin { Email = "nguyendinhlehoang2003@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("1234") }
                    );
                context.SaveChangesAsync();
            }
        }
    }
}
