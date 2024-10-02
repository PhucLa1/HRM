using AutoMapper;
using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.Briefcase
{
    public class DepartmentError
    {
        public const string NO_EXIST_EMPLOYEE = "Không có nhân viên nào mã như thế.";
        public const string EXIST_MANAGER_DEPARTMENT = "Nhân viên này đã làm trưởng phòng của phòng ban khác rồi. ";
        public const string NO_MANAGER_DEPARTMENT = "Chưa có";
    }
    public interface IDepartmentsService
    {
        Task<ApiResponse<IEnumerable<DepartmentResult>>> GetAllDepartment();
        Task<ApiResponse<bool>> AddNewDepartment(DepartmentUpsert departmentAdd);
        Task<ApiResponse<bool>> UpdateDepartment(int id, DepartmentUpsert departmentUpdate);
        Task<ApiResponse<bool>> RemoveDepartment(int id);
        Task<ApiResponse<List<DepartmentUserResult>>> GetAllEmployeeInDepartment(int id);
    }
    public class DepartmentsService : IDepartmentsService
    {
        private readonly IBaseRepository<Department> _baseRepository;
        private readonly IBaseRepository<Employee> _employeeRepository;
        private readonly IBaseRepository<Contract> _contractRepository;
        private readonly IBaseRepository<Position> _positionRepository;
        private readonly IValidator<DepartmentUpsert> _departmentUpsertValidator;
        private readonly IMapper _mapper;
        public DepartmentsService(
            IBaseRepository<Department> baseRepository,
            IValidator<DepartmentUpsert> departmentUpsertValidator,
            IBaseRepository<Employee> employeeRepository,
            IBaseRepository<Contract> contractRepository,
            IBaseRepository<Position> positionRepository,
            IMapper mapper)
        {
            _baseRepository = baseRepository;
            _departmentUpsertValidator = departmentUpsertValidator;
            _employeeRepository = employeeRepository;
            _contractRepository = contractRepository;
            _positionRepository = positionRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<List<DepartmentUserResult>>> GetAllEmployeeInDepartment(int id)
        {
            try
            {
                //Lấy hết những vị trí thuộc phòng ban đó ra
                var positionIds = await _positionRepository
                    .GetAllQueryAble()
                    .Where(e => e.DepartmentId == id)
                    .Select(e => e.Id)
                    .ToListAsync();

                //Lấy những hợp đồng thuộc những vị trí đó
                var userInDepartments = await _contractRepository
                    .GetAllQueryAble()
                    .Where(e => positionIds.Contains(e.PositionId))
                    .Join(_employeeRepository
                    .GetAllQueryAble(), c => c.Id, em => em.ContractId,
                    (c, em) => new DepartmentUserResult
                    {
                        Id = em.Id,
                        Name = c.Name,
                        Email = em.Email
                    }).ToListAsync();
                return new ApiResponse<List<DepartmentUserResult>>
                {
                    Metadata = userInDepartments,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddNewDepartment(DepartmentUpsert departmentAdd)
        {
            try
            {
                var resultValidation = _departmentUpsertValidator.Validate(departmentAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var newDepartment = new Department
                {
                    Name = departmentAdd.Name.Trim()
                };
                //Nếu managerId không null
                if (departmentAdd.ManagerId != null)
                {
                    var employee = await _employeeRepository
                        .GetAllQueryAble()
                        .Where(e => e.Id == departmentAdd.ManagerId)
                        .FirstOrDefaultAsync();
                    if (employee == null)
                    {
                        return new ApiResponse<bool> { Message = [DepartmentError.NO_EXIST_EMPLOYEE] };
                    }
                    else
                    {
                        //Tiếp tục check xem nó đã là trưởng phòng của 1 phòng ban nào khác chưa
                        var department = await _baseRepository
                            .GetAllQueryAble()
                            .Where(e => e.ManagerId == departmentAdd.ManagerId)
                            .FirstOrDefaultAsync();
                        if (department != null)
                        {
                            return new ApiResponse<bool> { Message = [DepartmentError.EXIST_MANAGER_DEPARTMENT] };
                        }
                        else
                        {
                            newDepartment.ManagerId = departmentAdd.ManagerId;
                        }
                    }
                }


                await _baseRepository.AddAsync(newDepartment);
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<IEnumerable<DepartmentResult>>> GetAllDepartment()
        {
            try
            {
                var departmentResults = await (from d in _baseRepository.GetAllQueryAble()
                                               join em in _employeeRepository.GetAllQueryAble() 
                                               on d.ManagerId equals em.Id into deptEmployees
                                               from em in deptEmployees.DefaultIfEmpty()
                                               join c in _contractRepository.GetAllQueryAble() 
                                               on em.ContractId equals c.Id into empContracts
                                               from c in empContracts.DefaultIfEmpty()
                                               select new DepartmentResult
                                               {
                                                   Id = d.Id,
                                                   Name = d.Name,
                                                   ManagerName = c.Name == null ? DepartmentError.NO_MANAGER_DEPARTMENT : c.Name,
                                                   ManagerId = c.Id,
                                               }).ToListAsync();
                return new ApiResponse<IEnumerable<DepartmentResult>>
                {
                    Metadata = departmentResults,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> RemoveDepartment(int id)
        {
            try
            {
                await _baseRepository.RemoveAsync(id);
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateDepartment(int id, DepartmentUpsert departmentUpdate)
        {
            try
            {
                var resultValidation = _departmentUpsertValidator.Validate(departmentUpdate);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }

                var department = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
                department.Name = departmentUpdate.Name.Trim();
                department.ManagerId = departmentUpdate.ManagerId;

                _baseRepository.Update(department);
                await _baseRepository.SaveChangeAsync();

                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
