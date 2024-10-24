using FluentValidation;
using FluentValidation.Results;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.TimeKeeping
{

    public interface ILeaveApplicationsService
    {
        Task<ApiResponse<IEnumerable<LeaveApplicationResult>>> GetAllApplications();
        Task<ApiResponse<bool>> AddNewApplication(LeaveApplicationUpSert leaveAdd);
        Task<ApiResponse<bool>> UpdateApplication(int id, LeaveApplicationUpSert leaveUpdate);
        Task<ApiResponse<bool>> RemoveApplication(int id);
        Task<ApiResponse<IEnumerable<EmployeeDataResult>>> GetAllEmployees();

    }
    public class LeaveApplicationsService : ILeaveApplicationsService
    {
        private readonly IBaseRepository<Employee> _employeeRepository;
        private readonly IBaseRepository<LeaveApplication> _baseRepository;
        private readonly IBaseRepository<Calendar> _calendarRepository;
        private readonly IValidator<LeaveApplicationUpSert> _leaveUpsertValidator;

        public LeaveApplicationsService
            (
            IBaseRepository<LeaveApplication> baseRepository,
            IBaseRepository<Employee> employeeRepository,
            IBaseRepository<Calendar> calendarRepository,
            IValidator<LeaveApplicationUpSert> leaveUpsertValidator
            )
        {
            _baseRepository = baseRepository;
            _employeeRepository = employeeRepository;
            _calendarRepository = calendarRepository;
            _leaveUpsertValidator = leaveUpsertValidator;
        }

        public async Task<ApiResponse<IEnumerable<EmployeeDataResult>>> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeRepository.GetAllQueryAble().ToListAsync();

                var employeeResults = employees.Select(e => new EmployeeDataResult
                {
                    Id = e.Id,
                    Name = e.UserName
                }).ToList();

                return new ApiResponse<IEnumerable<EmployeeDataResult>>
                {
                    Metadata = employeeResults,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<ApiResponse<bool>> AddNewApplication(LeaveApplicationUpSert leaveAdd)
        {
            try
            {
                var resultValidation = _leaveUpsertValidator.Validate(leaveAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }

                var employeeExists = await _employeeRepository.GetAllQueryAble().AnyAsync(e => e.Id == leaveAdd.EmployeeId);
                if (!employeeExists)
                {
                    return ApiResponse<bool>.FailtureValidation(new List<ValidationFailure>
                {
                    new ValidationFailure("EmployeeId", "Nhân viên không tồn tại.")
                });
                }

                await _baseRepository.AddAsync(new LeaveApplication
                {
                    EmployeeId = leaveAdd.EmployeeId,
                    RefuseReason = leaveAdd.RefuseReason?.Trim(),
                    TimeLeave = leaveAdd.TimeLeave,
                    StatusLeave = leaveAdd.StatusLeave,
                    Description = leaveAdd.Description?.Trim(),
                    ReplyMessage = leaveAdd.ReplyMessage?.Trim()
                });
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<IEnumerable<LeaveApplicationResult>>> GetAllApplications()
        {
            try
            {
                return new ApiResponse<IEnumerable<LeaveApplicationResult>>
                {
                    Metadata = await _baseRepository.GetAllQueryAble().Select(e => new LeaveApplicationResult
                    {
                        Id = e.Id,
                        EmployeeId = e.EmployeeId,
                        RefuseReason = e.RefuseReason,
                        TimeLeave = e.TimeLeave,
                        StatusLeave = e.StatusLeave,
                        Description = e.Description,
                        ReplyMessage = e.ReplyMessage,

                    }).ToListAsync(),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> RemoveApplication(int id)
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

        public async Task<ApiResponse<bool>> UpdateApplication(int id, LeaveApplicationUpSert leaveUpdate)
        {
            try
            {
                var resultValidation = _leaveUpsertValidator.Validate(leaveUpdate);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }

                var employeeExists = await _employeeRepository.GetAllQueryAble().AnyAsync(e => e.Id == leaveUpdate.EmployeeId);
                if (!employeeExists)
                {
                    return ApiResponse<bool>.FailtureValidation(new List<ValidationFailure>
                {
                    new ValidationFailure("EmployeeId", "Nhân viên không tồn tại.")
                });
                }

                var leave = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
                leave.EmployeeId = leaveUpdate.EmployeeId;
                leave.RefuseReason = leaveUpdate.RefuseReason?.Trim();
                leave.TimeLeave = leaveUpdate.TimeLeave;
                leave.StatusLeave = leaveUpdate.StatusLeave;
                leave.Description = leaveUpdate.Description?.Trim();
                leave.ReplyMessage = leaveUpdate.ReplyMessage?.Trim();
                _baseRepository.Update(leave);
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
