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
                //if (context.Employees.Any())
                //{
                //    return;
                //}

                //if (!context.Departments.Any())
                //{
                //    await context.Departments.AddRangeAsync(
                //       new Department
                //       {
                //           Name = "Phòng Test dự án"
                //           }
                //     );
                //    await context.SaveChangesAsync();
                //}

                //if (!context.Positions.Any())
                //{
                //    await context.Positions.AddRangeAsync(
                //       new Position
                //           {
                //               DepartmentId = 1,
                //               Name = "Tester",
                //               CurrentPositionsFilled = 100,
                //               TotalPositionsNeeded = 100,

                //           }
                //     );
                //    await context.SaveChangesAsync();
                //}

                //if (!context.ContractSalaries.Any())
                //{
                //    await context.ContractSalaries.AddRangeAsync(
                //      new ContractSalary
                //      {
                //          BaseSalary = 10000000,
                //          BaseInsurance = 1000000,
                //          RequiredDays = 23,
                //          RequiredHours = 23 * 8,
                //          WageDaily = 100000,
                //          WageHourly = 10000,
                //          Factor = 1
                //      }
                //);
                //    await context.SaveChangesAsync();
                //}


                //if (!context.Contracts.Any())
                //{
                //    await context.Contracts.AddRangeAsync(
                //       new Contract
                //       {
                //           ContractSalaryId = 1,
                //           ContractTypeId = 1,
                //           PositionId = 1,
                //           StartDate = DateTime.Now,
                //           EndDate = DateTime.Now,
                //           SignDate = DateTime.Now,
                //           FireUrlBase = "",
                //           FileUrlSigned = "",
                //       },
                //       new Contract
                //       {
                //           ContractSalaryId = 1,
                //           ContractTypeId = 1,
                //           PositionId = 1,
                //           StartDate = DateTime.Now,
                //           EndDate = DateTime.Now,
                //           SignDate = DateTime.Now,
                //           FireUrlBase = "",
                //           FileUrlSigned = "",
                //       }
                //     );
                //    await context.SaveChangesAsync();
                //}
                

                //await context.Employees.AddRangeAsync(
                //    new Employee
                //    {
                //        ContractId = 1,
                //        UserName = "Employee Test",
                //        PhoneNumber = "0821398128",
                //        Email = "phucminhbeos@gmail.com",
                //        Password = BCrypt.Net.BCrypt.HashPassword("Phucdeptrai"),
                //        StatusEmployee = StatusEmployee.OnLeave,
                //    },
                //    new Employee
                //    {
                //        ContractId = 2,
                //        UserName = "Employee Test 2",
                //        PhoneNumber = "01238921934",
                //        Email = "hung@gmail.com",
                //        Password = BCrypt.Net.BCrypt.HashPassword("okok"),
                //        StatusEmployee = StatusEmployee.OnLeave,
                //    }
                //    );
                //await context.SaveChangesAsync();

                if (context.Admins.Any())
                {
                    //Mới có thể xóa đi thêm lại từ đầu
                    /*
                    context.Admins.RemoveRange();
                    context.SaveChangesAsync();
                    */
                    return;   // DB has been seeded
                }
                await context.Admins.AddRangeAsync(
                    new Admin { Email = "phucminhbeos@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("Phucdeptrai") },
                    new Admin { Email = "nguyendinhlehoang2003@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("1234") }
                    );
                await context.SaveChangesAsync();

               
            }
        }
    }
}
