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
                if (context.Employees.Any() 
                    || context.Departments.Any()
                    || context.Positions.Any()
                    || context.ContractSalaries.Any()
                    || context.Contracts.Any()
                    || context.Employees.Any())
                {
                    return;
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        //Thêm phòng ban
                        var department = new Department { Name = "Phòng Test dự án" };
                        await context.Departments.AddAsync(department);
                        await context.SaveChangesAsync();

                        //Thêm vị trí
                        var position = new Position
                        {
                            DepartmentId = department.Id,
                            Name = "Tester",
                            CurrentPositionsFilled = 100,
                            TotalPositionsNeeded = 100,

                        };
                        await context.Positions.AddAsync(position);
                        await context.SaveChangesAsync();

                        //Thêm contract salary
                        var contractSalary = new ContractSalary
                        {
                            BaseSalary = 10000000,
                            BaseInsurance = 1000000,
                            RequiredDays = 23,
                            RequiredHours = 23 * 8,
                            WageDaily = 100000,
                            WageHourly = 10000,
                            Factor = 1
                        };
                        await context.ContractSalaries.AddAsync(contractSalary);
                        await context.SaveChangesAsync();

                        //Thêm contract type
                        var contractType = new ContractType
                        {
                            Name = "Hợp đồng có thời hạn",
                        };
                        await context.ContractTypes.AddAsync(contractType);
                        await context.SaveChangesAsync();

                        //Thêm contract
                        var contract = new Contract
                        {
                            ContractSalaryId = contractSalary.Id,
                            ContractTypeId = contractType.Id,
                            PositionId = 1,
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now,
                            SignDate = DateTime.Now,
                            FireUrlBase = "",
                            FileUrlSigned = "",
                        };
                        await context.Contracts.AddAsync(contract);
                        await context.SaveChangesAsync();

                        //Thêm dữ liệu nhân viên
                        var employee = new Employee
                        {
                            ContractId = contract.Id,
                            UserName = "Employee Test",
                            PhoneNumber = "0821398128",
                            Email = "phucminhbeos@gmail.com",
                            Password = BCrypt.Net.BCrypt.HashPassword("Phucdeptrai"),
                            StatusEmployee = StatusEmployee.OnLeave,
                        };
                        await context.Employees.AddAsync(employee);
                        await context.SaveChangesAsync();


                        //Commit dữ liệu
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
						await transaction.RollbackAsync();
						throw new Exception(ex.Message);
					}
                }
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
