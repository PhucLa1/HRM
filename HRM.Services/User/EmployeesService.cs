using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace HRM.Services.User
{
    public interface IEmployeesService
    {
        Task<ApiResponse<IEnumerable<EmployeeResult>>> GetAllEmployee();
    }
    public class EmployeesService:IEmployeesService
    {
        private readonly IBaseRepository<Employee> _employeeRepository;
        public EmployeesService(IBaseRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public async Task<ApiResponse<IEnumerable<EmployeeResult>>> GetAllEmployee()
        {
            try
            {
                return new ApiResponse<IEnumerable<EmployeeResult>>
                {

                    Metadata = await _employeeRepository.GetAllQueryAble().Select(e => new EmployeeResult
                    {
                        Id = e.Id,
                        Name = e.Contract.Name,
                        DateOfBirth = e.Contract.DateOfBirth,
                        Age = DateTime.Now.Year - e.Contract.DateOfBirth.Year
                                 - (DateTime.Now.DayOfYear < e.Contract.DateOfBirth.DayOfYear ? 1 : 0), // Tính tuổi
                        Gender = e.Contract.Gender,
                        Tenure = DateTime.Now.Year - e.Contract.StartDate.Year
                         - (DateTime.Now.DayOfYear < e.Contract.StartDate.DayOfYear ? 1 : 0), // Tính thâm niên
                        Address = e.Contract.Address,
                        CountrySide = e.Contract.CountrySide,
                        NationalID = e.Contract.NationalID,
                        NationalStartDate = e.Contract.NationalStartDate,
                        NationalAddress = e.Contract.NationalAddress,
                        Level = e.Contract.Level,
                        Major = e.Contract.Major,
                        PositionName = e.Contract.Position.Name,
                        PositionId = e.Contract.PositionId,
                        PhoneNumber = e.PhoneNumber,
                        Email = e.Email
                    }).ToListAsync(),
                    IsSuccess = true

                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
