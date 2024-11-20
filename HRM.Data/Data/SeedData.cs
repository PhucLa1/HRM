using HRM.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HRM.Data.Data
{
    public class SeedData
    {
        public async static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new HRMDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<HRMDbContext>>()))
            {          
                if (context.Admins.Any())
                {
                    return;   // DB has been seeded
                }
                await context.Admins.AddRangeAsync(
                    new Admin { Email = "phucminhbeos@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("Phucdeptrai") },
                    new Admin { Email = "nguyendinhlehoang2003@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("1234") },
                    new Admin { Email = "admin@hrm.com", Password = BCrypt.Net.BCrypt.HashPassword("123456") }
                    );
                await context.SaveChangesAsync();          
            }
        }
    }
}
