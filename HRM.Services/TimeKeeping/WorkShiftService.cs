using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Repositories.Helper;
using HRM.Repositories.Setting;
using HRM.Services.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HRM.Services.TimeKeeping
{
    public class WorkShiftError
    {
        public const string STATUS_FAILED = "Trạng thái truyền vào không đúng. ";
        public const string FORBIDDEN_PLAN = "Không có quyền thay đổi trạng thái kế hoạch làm việc";
        public const string STATUS_NULL = "Trạng thái truyền vào không được trống. ";
        public const string FORBIDDEN_OVERDUE = "Lịch làm việc quá hạn để duyệt. ";
    }
    public interface IWorkShiftService
    {
        Task<ApiResponse<bool>> RegisterWorkShift(WorkPlanInsert workPlanInsert);
        Task<ApiResponse<List<PartimePlanResult>>> GetAllPartimePlan();
        Task<ApiResponse<PartimePlanResult>> GetDetailPartimePlan(int partimePlanId);
        Task<ApiResponse<List<UserCalendarResult>>> GetAllWorkShiftByPartimePlanId(int partimePlanId);
        Task<ApiResponse<bool>> ProcessPartimePlanRequest(int partimePlanId, StatusCalendar statusCalendar);
    }
    public class WorkShiftService : IWorkShiftService
    {
        private readonly IBaseRepository<PartimePlan> _partimePlanRepository;
        private readonly IBaseRepository<UserCalendar> _userCalendarRepository;
        private readonly IValidator<WorkPlanInsert> _workPlanInsertValidator;
        private readonly IValidator<UserCalendarInsert> _userCalendarInserttValidator;
        private readonly IBaseRepository<Contract> _contractRepository;
        private readonly IBaseRepository<Employee> _employeeRepository;
        private readonly IEmailService _emailService;
        private readonly CompanySetting _serverCompanySetting;
        private const string FOLER = "Email";
        private const string PROCESS_PARTIMEPLAN_FILE = "ProcessPartimePlan.html";
        private const string PROCESS_PARTIMEPLAN_NOTIFICATION = "Thông báo về kết quả đăng kí lịch làm .";
        public WorkShiftService(IBaseRepository<PartimePlan> partimePlanRepository,
            IBaseRepository<UserCalendar> userCalendarRepository,
            IValidator<WorkPlanInsert> workPlanInsertValidator,
            IValidator<UserCalendarInsert> userCalendarInserttValidator,
            IBaseRepository<Contract> contractRepository,
            IBaseRepository<Employee> employeeRepository,
            IEmailService emailService,
            IOptions<CompanySetting> serverCompanySetting
            )
        {
            _partimePlanRepository = partimePlanRepository;
            _userCalendarRepository = userCalendarRepository;
            _workPlanInsertValidator = workPlanInsertValidator;
            _userCalendarInserttValidator = userCalendarInserttValidator;
            _contractRepository = contractRepository;
            _employeeRepository = employeeRepository;
            _emailService = emailService;
            _serverCompanySetting = serverCompanySetting.Value;
        }

        public async Task<ApiResponse<bool>> ProcessPartimePlanRequest(int partimePlanId, StatusCalendar statusCalendar)
        {
            try
            {
                //Nếu những trạng thái truyền vào không phải là Approved và Refuse 
                if (statusCalendar == StatusCalendar.Draft
                     || statusCalendar == StatusCalendar.Submit
                     || statusCalendar == StatusCalendar.Cancel)
                {
                    return new ApiResponse<bool> { Message = [WorkShiftError.STATUS_FAILED] };
                }
                
                //Nếu cái lịch làm việc đó mà đến ngày hôm nay chưa được duyệt


                var partimePlan = await _partimePlanRepository
                    .GetAllQueryAble()
                    .Where(e => e.Id == partimePlanId)
                    .FirstAsync();
                if (partimePlan.StatusCalendar != StatusCalendar.Submit)
                {
                    return new ApiResponse<bool> { Message = [WorkShiftError.FORBIDDEN_PLAN] };
                }

                if(partimePlan.TimeStart <= DateOnly.FromDateTime(DateTime.Now))
                {
                    return new ApiResponse<bool> { Message = [WorkShiftError.FORBIDDEN_OVERDUE] };
                }


                partimePlan.StatusCalendar = statusCalendar;
                _partimePlanRepository.Update(partimePlan);
                var userCalendarIds = await _userCalendarRepository
                   .GetAllQueryAble()
                   .Where(e => e.PartimePlanId == partimePlanId)
                   .ExecuteUpdateAsync(s => s.SetProperty(w => w.UserCalendarStatus, statusCalendar == StatusCalendar.Approved ? UserCalendarStatus.Approved : UserCalendarStatus.Inactive));

                await _partimePlanRepository.SaveChangeAsync();

                //Gửi mail cho nhân viên để thông báo


                var employee = await (from pt in _partimePlanRepository.GetAllQueryAble()
                                      join em in _employeeRepository.GetAllQueryAble() on pt.EmployeeId equals em.Id
                                      join c in _contractRepository.GetAllQueryAble() on em.ContractId equals c.Id
                                      where pt.Id == partimePlanId
                                      select new EmployeeInfo
                                      {
                                          Name = c.Name,
                                          Email = em.Email
                                      })
                                          .FirstAsync();

                var bodyContentEmail = HandleFile.READ_FILE(FOLER, PROCESS_PARTIMEPLAN_FILE)
                    .Replace("{employeeName}", employee.Name)
                    .Replace("{process}", statusCalendar == StatusCalendar.Approved ? "đồng ý" : "không được chấp nhận")
                    .Replace("{linkUrl}", "http://localhost:3000/time-keeping/register-shift/" + partimePlanId)
                    .Replace("{companyName}", _serverCompanySetting.CompanyName);

                var bodyEmail = _emailService.TemplateContent
                    .Replace("{content}", bodyContentEmail);

                var email = new Email()
                {
                    To = employee.Email!,
                    Body = bodyEmail,
                    Subject = PROCESS_PARTIMEPLAN_NOTIFICATION
                };
                //Gửi email
                await _emailService.SendEmailToRecipient(email);

                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<PartimePlanResult>> GetDetailPartimePlan(int partimePlanId)
        {
            try
            {
                var partimePlans = await (from pt in _partimePlanRepository.GetAllQueryAble()
                                          join em in _employeeRepository.GetAllQueryAble() on pt.EmployeeId equals em.Id
                                          join c in _contractRepository.GetAllQueryAble() on em.ContractId equals c.Id
                                          where pt.Id == partimePlanId
                                          select new PartimePlanResult
                                          {
                                              TimeStart = pt.TimeStart,
                                              TimeEnd = pt.TimeEnd,
                                              StatusCalendar = pt.StatusCalendar,
                                              CreatedAt = pt.CreatedAt,
                                              EmployeeName = c.Name,
                                              Id = pt.Id,
                                              DiffTime = pt.TimeEnd.DayNumber - pt.TimeStart.DayNumber,
                                          }).FirstAsync();
                return new ApiResponse<PartimePlanResult> { Metadata = partimePlans, IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<List<PartimePlanResult>>> GetAllPartimePlan()
        {
            try
            {
                var partimePlans = await (from pt in _partimePlanRepository.GetAllQueryAble()
                                          join em in _employeeRepository.GetAllQueryAble() on pt.EmployeeId equals em.Id
                                          join c in _contractRepository.GetAllQueryAble() on em.ContractId equals c.Id
                                          select new PartimePlanResult
                                          {
                                              TimeStart = pt.TimeStart,
                                              TimeEnd = pt.TimeEnd,
                                              StatusCalendar = pt.StatusCalendar,
                                              CreatedAt = pt.CreatedAt,
                                              EmployeeName = c.Name,
                                              Id = pt.Id,
                                              DiffTime = pt.TimeEnd.DayNumber - pt.TimeStart.DayNumber
                                          }).ToListAsync();
                return new ApiResponse<List<PartimePlanResult>> { Metadata = partimePlans, IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> RegisterWorkShift(WorkPlanInsert workPlanInsert)
        {
            try
            {
                //Check validate
                var workPlanInsertValidation = _workPlanInsertValidator.Validate(workPlanInsert);
                var dayWorks = workPlanInsert.DayWorks;
                foreach (var dayWork in dayWorks)
                {
                    var userCalendarInsertValidation = _userCalendarInserttValidator.Validate(dayWork);
                    if (!userCalendarInsertValidation.IsValid)
                    {
                        return ApiResponse<bool>.FailtureValidation(userCalendarInsertValidation.Errors);
                    }
                }

                if (!workPlanInsertValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(workPlanInsertValidation.Errors);
                }

                /*Lấy ra id của nhân viên hiện tại 
                 * Mặc định hàm này được sử dụng bởi nhân viên partime
                 */
                int employeeId = _partimePlanRepository
                    .Context.GetCurrentUserId();


                //Những ngày đã được đăng kí trước sẽ bị đè vào
                /*Lấy những ngày đã đăng kí trong khoảng thời gian này*/
                var userCalendarIds = await _userCalendarRepository
                    .GetAllQueryAble()
                    .Where(e => e.PresentShift <= workPlanInsert.TimeEnd
                    && e.PresentShift >= workPlanInsert.TimeStart
                    && (e.UserCalendarStatus == UserCalendarStatus.Submit
                    || e.UserCalendarStatus == UserCalendarStatus.Approved))
                    .ExecuteUpdateAsync(s => s.SetProperty(w => w.UserCalendarStatus, UserCalendarStatus.Inactive));

                //Thêm mới các công ca
                using (var transaction = await _partimePlanRepository.Context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        //Thêm mới kế hoạch mới
                        var partimePlan = new PartimePlan
                        {
                            TimeStart = workPlanInsert.TimeStart,
                            TimeEnd = workPlanInsert.TimeEnd,
                            EmployeeId = employeeId,
                            StatusCalendar = workPlanInsert.StatusCalendar,
                        };
                        await _partimePlanRepository.AddAsync(partimePlan);
                        await _partimePlanRepository.SaveChangeAsync();


                        var userCalendars = new List<UserCalendar>();
                        foreach (var dayWork in dayWorks)
                        {
                            //Tạo mới các user calendar
                            var userCalendar = new UserCalendar
                            {
                                ShiftTime = dayWork.ShiftTime,
                                PresentShift = dayWork.PresentShift,
                                UserCalendarStatus = UserCalendarStatus.Submit,
                                PartimePlanId = partimePlan.Id
                            };
                            userCalendars.Add(userCalendar);
                        }
                        //Thêm user calendar
                        await _userCalendarRepository.AddRangeAsync(userCalendars);
                        await _userCalendarRepository.SaveChangeAsync();




                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }

                return new ApiResponse<bool> { IsSuccess = true };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<List<UserCalendarResult>>> GetAllWorkShiftByPartimePlanId(int partimePlanId)
        {
            try
            {
                var userCalendars = await _userCalendarRepository
                    .GetAllQueryAble()
                    .Where(e => e.PartimePlanId == partimePlanId)
                    .Select(e => new UserCalendarResult
                    {
                        ShiftTime = e.ShiftTime,
                        PresentShift = e.PresentShift,
                        UserCalendarStatus = UserCalendarStatus.Submit,
                    })
                    .ToListAsync();
                return new ApiResponse<List<UserCalendarResult>> { Metadata = userCalendars, IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
