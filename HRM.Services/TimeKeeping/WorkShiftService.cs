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
using OfficeOpenXml;

namespace HRM.Services.TimeKeeping
{

    /* Đối với nhân viên partime
     * Ca sáng từ 8h - 12h
     * Ca chiều từ 1h - 5h
     * Ca tối từ 6h - 9h - Công ca tối chỉ tính 3 tiếng 
     * Nếu là ca sáng - Chỉ được chấm công muộn 30p - Chấm công đầu vào chỉ được từ trước 8h
                                                    - Chấm công đầu ra chỉ được từ 12h - 12r 

     * Nếu là ca chiều - Chỉ được chấm công muộn nhất 30p - Chấm công đầu vào từ 12h31 - 13h30
                       - Chỉ được chấm công muộn nhất 30p - Chấm công đầu ra từ 17h - 17h30

     * Nếu là ca tối - Chỉ được chấm công muộn nhất 30p - Chấm công đầu vào từ 17h31 - 6r
                     - Chấm công đầu ra từ 9h - 12h  
     * Lưu ý về việc tính giờ công : Sẽ tính theo mức lương theo giờ của nhân viên partime * số giờ làm . 
     * Nếu làm trên 75% giờ làm (Trên 3 tiếng ) => Tính đủ giờ công
     * Dưới 2 tiếng thì coi như vắng
     * Trên 2h < Giờ làm < 3h => Tính là 2h công
     * Đối với việc quên chấm công (Có thể là chỉ chấm công 1 lần ) => Tính là vắng
     */


    /* Đối với nhân viên fulltime
     * Làm từ 4h - 8h thì tính theo giờ ( Làm tròn )
     * Làm dưới 4h coi như vắng
     * Quy định cho nhân viên fulltime: 1 tuần làm 48 tiếng (Giờ khác tính vào OT) (OT sẽ được phát triền sau )
     */
    public class WorkShiftError
    {
        public const string STATUS_FAILED = "Trạng thái truyền vào không đúng. ";
        public const string FORBIDDEN_PLAN = "Không có quyền thay đổi trạng thái kế hoạch làm việc";
        public const string STATUS_NULL = "Trạng thái truyền vào không được trống. ";
        public const string FORBIDDEN_OVERDUE = "Lịch làm việc quá hạn để duyệt. ";
        public const string FAILED_REGULAR = "Sai định dạng ngày tháng. ";
        public const string NOT_PARTIME_EMPLOYEE = "Nhân viên đang xét không phải nhân viên partime. ";
        public const string NOT_FULLTIME_EMPLOYEE = "Nhân viên đang xét không phải là nhân viên fulltime. ";
        public const string FAILED_TYPE_CONTRACT = "Không phải nhân viên partime. ";
    }
    public interface IWorkShiftService
    {
        Task<ApiResponse<bool>> RegisterWorkShift(WorkPlanInsert workPlanInsert);
        Task<ApiResponse<List<PartimePlanResult>>> GetAllPartimePlan();
        Task<ApiResponse<List<PartimePlanResult>>> GetAllPartimePlanByCurrentEmployeeId();
        Task<ApiResponse<PartimePlanResult>> GetDetailPartimePlan(int partimePlanId);
        Task<ApiResponse<List<UserCalendarResult>>> GetAllWorkShiftByPartimePlanId(int partimePlanId);
        Task<ApiResponse<bool>> ProcessPartimePlanRequest(int partimePlanId, StatusCalendar statusCalendar);
        Task<ApiResponse<List<List<CalendarEntry>>>> GetAllWorkShiftByPartimeEmployee(int employeeId, string startDate, string endDate);
        Task<ApiResponse<bool>> PrintPartimeWorkShiftToExcel(int employeeId, string startDate, string endDate);
        Task<ApiResponse<bool>> PrintFullTimeAttendanceToExcel(int employeeId, string startDate, string endDate);
        Task<ApiResponse<List<EmployeeAttendanceRecord>>> GetAllWorkShiftByFullTimeEmployee(int employeeId, string startDate, string endDate);
        Task<ApiResponse<HistoryCheckResult>> CheckInOutEmployee(int employeeId, HistoryUpsert historyAdd); //Chấm công đầu vào , đầu ra cho nhân viên
        Task<ApiResponse<bool>> UpdateHistoryAttendance(int historyId, HistoryUpsert historyUpdate); //Sửa thời gian chấm công 

        // Tính số giờ làm cho nhân viên 
        Task<ApiResponse<IEnumerable<TotalWorkHours>>> GetTotalHoursOfEmployeeWork(List<int> employeeIds, string startDate, string endDate);

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
        private readonly IBaseRepository<History> _historyRepository;
        private readonly IValidator<HistoryUpsert> _historyUpsertValidator;
        public WorkShiftService(IBaseRepository<PartimePlan> partimePlanRepository,
            IBaseRepository<UserCalendar> userCalendarRepository,
            IValidator<WorkPlanInsert> workPlanInsertValidator,
            IValidator<UserCalendarInsert> userCalendarInserttValidator,
            IBaseRepository<Contract> contractRepository,
            IBaseRepository<Employee> employeeRepository,
            IEmailService emailService,
            IOptions<CompanySetting> serverCompanySetting,
            IBaseRepository<History> historyRepository,
            IValidator<HistoryUpsert> historyUpsertValidator
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
            _historyRepository = historyRepository;
            _emailService = emailService;
            _historyUpsertValidator = historyUpsertValidator;
        }


        public async Task<ApiResponse<IEnumerable<TotalWorkHours>>> GetTotalHoursOfEmployeeWork(List<int> employeeIds, string startDate, string endDate)
        {
            try
            {
                //Kiểm tra xem có đúng định dạng DateOnly không ? 
                string dateFormat = "yyyy-MM-dd";
                bool isValidStartDate = DateOnly.TryParseExact(startDate, dateFormat, out DateOnly parsedStartDate);

                // Kiểm tra endDate
                bool isValidEndDate = DateOnly.TryParseExact(endDate, dateFormat, out DateOnly parsedEndDate);

                if (!isValidStartDate || !isValidEndDate)
                {
                    return new ApiResponse<IEnumerable<TotalWorkHours>> { Message = [WorkShiftError.FAILED_REGULAR] };
                }

                //Chuyển sang dạng DateOnly
                DateOnly.TryParse(startDate, out DateOnly dateOnlyStartDate);
                DateOnly.TryParse(endDate, out DateOnly dateOnlyEndDate);

                var employeeIdRoles = await (from em in _employeeRepository.GetAllQueryAble()
                                             join c in _contractRepository.GetAllQueryAble() on em.ContractId equals c.Id
                                             where employeeIds.Contains(em.Id)
                                             select new { em.Id, c.TypeContract }
                                            ).ToListAsync();

                // Tách kết quả thành hai mảng dựa trên loại hợp đồng (Partime và Fulltime)
                var partimeEmployees = employeeIdRoles.Where(x => x.TypeContract == TypeContract.Partime).Select(x => x.Id).ToList();
                var fulltimeEmployees = employeeIdRoles.Where(x => x.TypeContract == TypeContract.Fulltime).Select(x => x.Id).ToList();

                //Tính giờ làm cho fulltime 
                var histories = await _historyRepository
                    .GetAllQueryAble()
                    .Where(e => DateOnly.FromDateTime(e.TimeSweep) >= dateOnlyStartDate
                        && DateOnly.FromDateTime(e.TimeSweep) <= dateOnlyEndDate
                        && fulltimeEmployees.Contains(e.EmployeeId))
                    .ToListAsync();
                var employeeWorkHours = histories
                    .GroupBy(e => e.EmployeeId) // Nhóm theo EmployeeId
                    .Select(group => new TotalWorkHours
                    {
                        EmployeeId = group.Key,
                        TotalWorkedHours = group
                        .GroupBy(e => DateOnly.FromDateTime(e.TimeSweep)) // Nhóm tiếp theo từng ngày
                        .Sum(dayGroup =>
                            dayGroup.Count() > 1
                            ? (dayGroup.Max(e => e.TimeSweep) - dayGroup.Min(e => e.TimeSweep)).TotalHours
                            : 0 // Không tính ngày chỉ có 1 lần chấm công
)
                    })
                    .ToList();

                //Tính giờ làm cho partime
                // Lấy danh sách các ca làm việc đã được phê duyệt
                var userCalendars = await (from uc in _userCalendarRepository.GetAllQueryAble()
                                           join pt in _partimePlanRepository.GetAllQueryAble() on uc.PartimePlanId equals pt.Id
                                           where uc.PresentShift >= dateOnlyStartDate && uc.PresentShift <= dateOnlyEndDate // Thuộc khoảng đang xét
                                                 && pt.StatusCalendar == StatusCalendar.Approved // Kế hoạch được chấp nhận
                                                 && uc.UserCalendarStatus == UserCalendarStatus.Approved // Những ngày được chấp nhận
                                                 && partimeEmployees.Contains(pt.EmployeeId) // Thuộc về nhân viên đang xét
                                           orderby uc.PresentShift ascending // Theo ngày tháng tăng dần
                                           select new UserCalendarResultTotalWork
                                           {
                                               EmployeeId = pt.EmployeeId, // Lấy thông tin EmployeeId
                                               ShiftTime = uc.ShiftTime,
                                               PresentShift = uc.PresentShift,
                                               UserCalendarStatus = uc.UserCalendarStatus,
                                           }).ToListAsync();

                // Lấy danh sách chấm công trong khoảng thời gian đã chọn cho tất cả nhân viên
                var historiesForPartime = await _historyRepository
                    .GetAllQueryAble()
                    .Where(e => DateOnly.FromDateTime(e.TimeSweep) >= dateOnlyStartDate
                                && DateOnly.FromDateTime(e.TimeSweep) <= dateOnlyEndDate
                                && partimeEmployees.Contains(e.EmployeeId)) // Lọc theo danh sách nhân viên
                    .OrderBy(e => e.TimeSweep)
                    .Select(e => new HistoryResult
                    {
                        Id = e.Id,
                        TimeSweep = e.TimeSweep,
                        EmployeeId = e.EmployeeId,
                        StatusHistory = e.StatusHistory,
                    })
                    .ToListAsync();

                // Tính tổng số giờ làm việc cho từng nhân viên
                var shiftTimes = new Dictionary<ShiftTime, (TimeSpan start, TimeSpan end)>
                {
                    { ShiftTime.Morning, (new TimeSpan(6, 0, 0), new TimeSpan(12, 30, 0)) },
                    { ShiftTime.Afternoon, (new TimeSpan(12, 0, 0), new TimeSpan(18, 0, 0)) },
                    { ShiftTime.Evening, (new TimeSpan(18, 0, 0), new TimeSpan(23, 59, 59)) }
                };

                // Tính tổng số giờ làm việc cho từng nhân viên
                var totalWorkedHoursByEmployee = userCalendars
                    .GroupBy(uc => uc.EmployeeId) // Nhóm theo EmployeeId
                    .Select(group =>
                    {
                        var employeeHistories = histories
                            .Where(h => h.EmployeeId == group.Key) // Lọc ra các lần chấm công của nhân viên này
                            .ToList();

                        var totalWorkedHours = group
                            .Sum(uc =>
                            {
                                // Lấy các lần chấm công thuộc ca hiện tại
                                var checkInOut = employeeHistories
                                    .Where(h => DateOnly.FromDateTime(h.TimeSweep) == uc.PresentShift) // Cùng ngày
                                    .OrderBy(h => h.TimeSweep)
                                    .ToList();

                                // Chỉ tính giờ nếu có ít nhất 2 lần chấm công
                                if (checkInOut.Count >= 2)
                                {
                                    var earliestTime = checkInOut.First().TimeSweep;
                                    var latestTime = checkInOut.Last().TimeSweep;

                                    // Kiểm tra xem thời gian chấm công có khớp với ca không
                                    var shiftStart = uc.PresentShift.ToDateTime(new TimeOnly(shiftTimes[uc.ShiftTime].start.Hours, shiftTimes[uc.ShiftTime].start.Minutes)); // Chuyển thành TimeOnly rồi cộng vào
                                    var shiftEnd = uc.PresentShift.ToDateTime(new TimeOnly(shiftTimes[uc.ShiftTime].end.Hours, shiftTimes[uc.ShiftTime].end.Minutes)); // Chuyển thành TimeOnly rồi cộng vào

                                    // Sau đó, bạn có thể so sánh thời gian như bình thường
                                    if (earliestTime >= shiftStart && latestTime <= shiftEnd)
                                    {
                                        return (latestTime - earliestTime).TotalHours; // Thời gian làm việc trong ca
                                    }
                                }
                                return 0; // Không có giờ làm nếu không đủ chấm công
                            });

                        return new TotalWorkHours
                        {
                            EmployeeId = group.Key,
                            TotalWorkedHours = totalWorkedHours
                        };
                    })
                    .ToList();
                var dataReturn = totalWorkedHoursByEmployee.Concat(employeeWorkHours);
                return new ApiResponse<IEnumerable<TotalWorkHours>> { IsSuccess = true, Metadata = dataReturn };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<ApiResponse<bool>> PrintFullTimeAttendanceToExcel(int employeeId, string startDate, string endDate)
        {
            try
            {
                //Kiểm tra xem có đúng định dạng DateOnly không ? 
                string dateFormat = "yyyy-MM-dd";
                bool isValidStartDate = DateOnly.TryParseExact(startDate, dateFormat, out DateOnly parsedStartDate);

                // Kiểm tra endDate
                bool isValidEndDate = DateOnly.TryParseExact(endDate, dateFormat, out DateOnly parsedEndDate);

                if (!isValidStartDate || !isValidEndDate)
                {
                    return new ApiResponse<bool> { Message = [WorkShiftError.FAILED_REGULAR] };
                }

                //Chuyển sang dạng DateOnly
                DateOnly.TryParse(startDate, out DateOnly dateOnlyStartDate);
                DateOnly.TryParse(endDate, out DateOnly dateOnlyEndDate);

                //Kiểm tra xem nhân viên đang xét có phải là nhân viên fulltime không ?
                var employeeType = await (from em in _employeeRepository.GetAllQueryAble()
                                          join c in _contractRepository.GetAllQueryAble() on em.ContractId equals c.Id
                                          select c.TypeContract).FirstAsync();

                if (employeeType == TypeContract.Partime)
                {
                    return new ApiResponse<bool> { Message = [WorkShiftError.NOT_FULLTIME_EMPLOYEE] };
                }
                var employeeAttendanceRecords = await GetAllHistoryAttendanceFullTimeEmployee(employeeId, dateOnlyStartDate, dateOnlyEndDate);
                try
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    int length = employeeAttendanceRecords.Count;
                    //Tao file name 
                    string fileName = DateTime.Now.Ticks.ToString() + "_FullTime.xlsx";

                    // Đường dẫn đến thư mục wwwroot
                    var exactPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", fileName);

                    // Tạo một file Excel mới
                    var package = new ExcelPackage();

                    // Tạo một sheet mới
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                    worksheet.Cells[2, 1].Value = "Time Work";
                    for (int index = 1; index <= length; index++) //Trả về sẽ là 1 chuỗi ngày tháng
                    {
                        var entry = employeeAttendanceRecords[index - 1];
                        worksheet.Cells[1, index + 1].Value = entry.Date.ToString("dd-MM");

                        worksheet.Cells[2, index + 1].Value = "X";

                        if (entry.HistoryResults != null && entry.HistoryResults.Count >= 2) //Phải chấm công đầu vào và cả đầu ra
                        {
                            var historiesLength = entry.HistoryResults.Count;
                            var firstSweep = TimeOnly.FromDateTime(entry.HistoryResults[0].TimeSweep);
                            var lastSweep = TimeOnly.FromDateTime(entry.HistoryResults[historiesLength - 1].TimeSweep);
                            var difference = lastSweep.ToTimeSpan() - firstSweep.ToTimeSpan();

                            // Chuyển đổi thành số giờ thập phân
                            var decimalHours = Math.Round(difference.TotalHours, 2).ToString();
                            worksheet.Cells[2, index + 1].Value = decimalHours + "h";
                        }
                    }


                    // Lưu file Excel
                    package.SaveAs(new FileInfo(exactPath));
                    HandleFile.DownloadFile("Excel", fileName);

                }
                catch (Exception ex)
                {
                    var innerMessage = ex.InnerException != null ? ex.InnerException.Message : "Không có thông tin chi tiết";
                    throw new Exception($"Lỗi: {ex.Message}. Chi tiết nội bộ: {innerMessage}", ex);
                }
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<List<EmployeeAttendanceRecord>>> GetAllWorkShiftByFullTimeEmployee(int employeeId, string startDate, string endDate)
        {
            try
            {
                //Kiểm tra xem có đúng định dạng DateOnly không ? 
                string dateFormat = "yyyy-MM-dd";
                bool isValidStartDate = DateOnly.TryParseExact(startDate, dateFormat, out DateOnly parsedStartDate);

                // Kiểm tra endDate
                bool isValidEndDate = DateOnly.TryParseExact(endDate, dateFormat, out DateOnly parsedEndDate);

                if (!isValidStartDate || !isValidEndDate)
                {
                    return new ApiResponse<List<EmployeeAttendanceRecord>> { Message = [WorkShiftError.FAILED_REGULAR] };
                }

                //Chuyển sang dạng DateOnly
                DateOnly.TryParse(startDate, out DateOnly dateOnlyStartDate);
                DateOnly.TryParse(endDate, out DateOnly dateOnlyEndDate);

                //Kiểm tra xem nhân viên đang xét có phải là nhân viên fulltime không ?
                var employeeType = await (from em in _employeeRepository.GetAllQueryAble()
                                          join c in _contractRepository.GetAllQueryAble() on em.ContractId equals c.Id
                                          select c.TypeContract).FirstAsync();

                if (employeeType == TypeContract.Partime)
                {
                    return new ApiResponse<List<EmployeeAttendanceRecord>> { Message = [WorkShiftError.NOT_FULLTIME_EMPLOYEE] };
                }
                var employeeAttendanceRecords = await GetAllHistoryAttendanceFullTimeEmployee(employeeId, dateOnlyStartDate, dateOnlyEndDate);
                return new ApiResponse<List<EmployeeAttendanceRecord>> { Metadata = employeeAttendanceRecords, IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<List<EmployeeAttendanceRecord>> GetAllHistoryAttendanceFullTimeEmployee(int employeeId, DateOnly dateOnlyStartDate, DateOnly dateOnlyEndDate)
        {
            //Lấy ra những lần chấm công
            var histories = await _historyRepository
            .GetAllQueryAble()
            .Where(e => DateOnly.FromDateTime(e.TimeSweep) >= dateOnlyStartDate
            && DateOnly.FromDateTime(e.TimeSweep) <= dateOnlyEndDate
            && e.EmployeeId == employeeId)
            .OrderBy(e => e.TimeSweep)
            .Select(e => new HistoryResult
            {
                Id = e.Id,
                TimeSweep = e.TimeSweep,
                EmployeeId = e.EmployeeId,
                StatusHistory = e.StatusHistory,
            })
            .ToListAsync();

            //Lấy ra tất cả những ngày trong khoảng thời gian cho trước
            var daysInRange = Enumerable.Range(0, (dateOnlyEndDate.ToDateTime(TimeOnly.MinValue) - dateOnlyStartDate.ToDateTime(TimeOnly.MinValue)).Days + 1)
                .Select(offset => new
                {
                    Date = dateOnlyStartDate.AddDays(offset),
                    DayOfWeek = dateOnlyStartDate.AddDays(offset).DayOfWeek.ToString()
                })
                .ToList();

            var employeeAttendanceRecords = new List<EmployeeAttendanceRecord>();
            foreach (var date in daysInRange)
            {
                //Lấy những ca làm việc có tháng năm bằng date
                var historiesForDay = histories
                    .Where(e => DateOnly.FromDateTime(e.TimeSweep) == date.Date)
                    .ToList();
                employeeAttendanceRecords.Add(new EmployeeAttendanceRecord
                {
                    DayOfWeek = date.DayOfWeek,
                    Date = date.Date,
                    HistoryResults = historiesForDay
                });
            }
            return employeeAttendanceRecords;
        }


        public async Task<ApiResponse<bool>> UpdateHistoryAttendance(int historyId, HistoryUpsert historyUpdate)
        {
            try
            {

                //Check validate
                var resultValidation = _historyUpsertValidator.Validate(historyUpdate);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var history = await _historyRepository
                    .GetAllQueryAble()
                    .Where(e => e.Id == historyId)
                    .FirstAsync();
                history.StatusHistory = historyUpdate.StatusHistory;
                history.TimeSweep = historyUpdate.TimeSweep;
                _historyRepository.Update(history);
                await _historyRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Chấm công đầu vào không được trước quá 30p - kể từ thời điểm bắt đầu giờ làm
        //Chấm công đầu ra không được sau quá 30p - kể từ thời điểm bắt đầu tan
        public async Task<ApiResponse<HistoryCheckResult>> CheckInOutEmployee(int employeeId, HistoryUpsert historyAdd)
        {
            try
            {
                var resultValidation = _historyUpsertValidator.Validate(historyAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<HistoryCheckResult>.FailtureValidation(resultValidation.Errors);
                }

                var employee = await (from em in _employeeRepository.GetAllQueryAble()
                                      join c in _contractRepository.GetAllQueryAble() on em.ContractId equals c.Id
                                      where em.Id == employeeId
                                      select new
                                      {
                                          Name = c.Name
                                      }
                                          ).FirstAsync();


                var history = new History()
                {
                    StatusHistory = historyAdd.StatusHistory,
                    EmployeeId = employeeId,
                    TimeSweep = historyAdd.TimeSweep,
                };
                await _historyRepository.AddAsync(history);
                await _historyRepository.SaveChangeAsync();
                var dataReturn = new HistoryCheckResult
                {
                    EmployeeName = employee.Name,
                    Id = employeeId,
                    TimeSweep = historyAdd.TimeSweep
                };
                return new ApiResponse<HistoryCheckResult> { IsSuccess = true, Metadata = dataReturn };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private ShiftTime GetShiftTime(TimeSpan time)
        {
            if (time >= new TimeSpan(5, 0, 0) && time < new TimeSpan(13, 0, 0))
            {
                return ShiftTime.Morning; // Hoặc ca tương ứng
            }
            else if (time >= new TimeSpan(13, 0, 0) && time < new TimeSpan(17, 0, 0))
            {
                return ShiftTime.Afternoon; // Hoặc ca tương ứng
            }
            else
            {
                return ShiftTime.Evening; // Hoặc ca tương ứng
            }
        }
        public async Task<ApiResponse<bool>> PrintPartimeWorkShiftToExcel(int employeeId, string startDate, string endDate)
        {
            try
            {
                //Kiểm tra xem có đúng định dạng DateOnly không ? 
                string dateFormat = "yyyy-MM-dd";
                bool isValidStartDate = DateOnly.TryParseExact(startDate, dateFormat, out DateOnly parsedStartDate);

                // Kiểm tra endDate
                bool isValidEndDate = DateOnly.TryParseExact(endDate, dateFormat, out DateOnly parsedEndDate);

                if (!isValidStartDate || !isValidEndDate)
                {
                    return new ApiResponse<bool> { Message = [WorkShiftError.FAILED_REGULAR] };
                }

                //Chuyển sang dạng DateOnly
                DateOnly.TryParse(startDate, out DateOnly dateOnlyStartDate);
                DateOnly.TryParse(endDate, out DateOnly dateOnlyEndDate);


                //Kiểm tra xem nhân viên đang xét có phải là nhân viên partime không ?
                var employeeType = await (from em in _employeeRepository.GetAllQueryAble()
                                          join c in _contractRepository.GetAllQueryAble() on em.ContractId equals c.Id
                                          select c.TypeContract).FirstAsync();

                if (employeeType == TypeContract.Fulltime)
                {
                    return new ApiResponse<bool> { Message = [WorkShiftError.NOT_PARTIME_EMPLOYEE] };
                }

                var calendarEntries = await GetAllWorkShiftAttendanceTracking(employeeId, dateOnlyStartDate, dateOnlyEndDate);
                try
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    int length = calendarEntries.Count;
                    //Tao file name 
                    string fileName = DateTime.Now.Ticks.ToString() + "_Partime.xlsx";

                    // Đường dẫn đến thư mục wwwroot
                    var exactPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel", fileName);

                    // Tạo một file Excel mới
                    var package = new ExcelPackage();

                    // Tạo một sheet mới
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                    worksheet.Cells[2, 1].Value = "Morning";
                    worksheet.Cells[3, 1].Value = "Afternoon";
                    worksheet.Cells[4, 1].Value = "Evening";
                    for (int index = 1; index <= length; index++) //Trả về sẽ là 1 chuỗi ngày tháng
                    {
                        var entry = calendarEntries[index - 1];
                        worksheet.Cells[1, index + 1].Value = entry.Date.ToString("dd-MM");  //Hàng 1, bắt đầu từ cột 2
                                                                                             //Morning = 0, Afternoon = 1, Evening = 2
                                                                                             //Ca nào mà làm thì sẽ đánh dấu X trước
                        if (entry.UserCalendarResult != null)
                        {
                            //Ca nào đã đăng kí rồi mà không đi làm hoặc là chỉ chấm công 1 trong 2 lần thì sẽ đánh dấu X, ngược lại thì là V
                            if (entry.UserCalendarResult.Count > 0) //Nếu ca sáng đã đăng kí
                            {
                                worksheet.Cells[2, index + 1].Value = "X"; //Ca sáng có đăng kí làm
                                                                           //Check xem là ca sáng có đi làm không - Ca sáng thì chấm công 2 lần 
                                if (entry.HistoryEntryResults?.ContainsKey(ShiftTime.Morning) == true && entry.HistoryEntryResults[ShiftTime.Morning].Count >= 2)
                                {
                                    var firstCheckInsMorning = entry.HistoryEntryResults[ShiftTime.Morning]
                                        .Where(e => e.StatusHistory == StatusHistory.In)
                                        .Select(e => TimeOnly.FromDateTime(e.TimeSweep))
                                        .FirstOrDefault();
                                    var lastCheckOutsMorning = entry.HistoryEntryResults[ShiftTime.Morning]
                                        .Where(e => e.StatusHistory == StatusHistory.Out)
                                        .Select(e => TimeOnly.FromDateTime(e.TimeSweep))
                                        .LastOrDefault();
                                    if (firstCheckInsMorning == TimeOnly.MinValue || lastCheckOutsMorning == TimeOnly.MinValue) //Không chấm công đầu vào hoặc đầu ra
                                    {
                                        worksheet.Cells[2, index + 1].Value = "X";
                                    }
                                    else
                                    {
                                        var difference = lastCheckOutsMorning.ToTimeSpan() - firstCheckInsMorning.ToTimeSpan();

                                        // Chuyển đổi thành số giờ thập phân
                                        var decimalHours = Math.Round(difference.TotalHours, 2).ToString();
                                        worksheet.Cells[2, index + 1].Value = decimalHours + "h";
                                    }
                                }
                            }
                            if (entry.UserCalendarResult.Count > 1) //Nếu ca chiều đã đăng kí, có lịch sử chấm công
                            {
                                worksheet.Cells[3, index + 1].Value = "X";
                                //Check xem là ca chiều có đi làm không - Ca chiều thì chấm công 2 lần

                                if (entry.HistoryEntryResults?.ContainsKey(ShiftTime.Afternoon) == true && entry.HistoryEntryResults[ShiftTime.Afternoon].Count >= 2)
                                {
                                    var firstCheckInAfternoon = entry.HistoryEntryResults[ShiftTime.Afternoon]
                                        .Where(e => e.StatusHistory == StatusHistory.In)
                                        .Select(e => TimeOnly.FromDateTime(e.TimeSweep))
                                        .FirstOrDefault();
                                    var lastCheckOutsAfternoon = entry.HistoryEntryResults[ShiftTime.Afternoon]
                                            .Where(e => e.StatusHistory == StatusHistory.Out)
                                            .Select(e => TimeOnly.FromDateTime(e.TimeSweep))
                                            .FirstOrDefault();
                                    if (firstCheckInAfternoon == TimeOnly.MinValue || lastCheckOutsAfternoon == TimeOnly.MinValue) //Không chấm công đầu vào hoặc đầu ra
                                    {
                                        worksheet.Cells[3, index + 1].Value = "M";
                                    }
                                    else
                                    {
                                        var difference = lastCheckOutsAfternoon.ToTimeSpan() - firstCheckInAfternoon.ToTimeSpan();

                                        // Chuyển đổi thành số giờ thập phân
                                        var decimalHours = Math.Round(difference.TotalHours, 2).ToString();
                                        worksheet.Cells[3, index + 1].Value = decimalHours + "h";
                                    }
                                }
                            }
                            if (entry.UserCalendarResult.Count > 2) //Nếu ca sáng đã đăng kí
                            {
                                worksheet.Cells[4, index + 1].Value = "X";
                                //Check xem là ca tối có đi làm không - Ca tối thì chấm công 2 lần 

                                if (entry.HistoryEntryResults?.ContainsKey(ShiftTime.Evening) == true && entry.HistoryEntryResults[ShiftTime.Evening].Count >= 2)
                                {
                                    var firstCheckInEvening = entry.HistoryEntryResults[ShiftTime.Evening]
                                        .Where(e => e.StatusHistory == StatusHistory.In)
                                        .Select(e => TimeOnly.FromDateTime(e.TimeSweep))
                                        .FirstOrDefault();
                                    var lastCheckOutEvening = entry.HistoryEntryResults[ShiftTime.Evening]
                                            .Where(e => e.StatusHistory == StatusHistory.Out)
                                            .Select(e => TimeOnly.FromDateTime(e.TimeSweep))
                                            .FirstOrDefault();
                                    if (firstCheckInEvening == TimeOnly.MinValue || lastCheckOutEvening == TimeOnly.MinValue)
                                    {
                                        worksheet.Cells[4, index + 1].Value = "M";
                                    }
                                    else
                                    {
                                        var difference = lastCheckOutEvening.ToTimeSpan() - firstCheckInEvening.ToTimeSpan();

                                        // Chuyển đổi thành số giờ thập phân
                                        var decimalHours = Math.Round(difference.TotalHours, 2).ToString();
                                        worksheet.Cells[4, index + 1].Value = decimalHours + "h";
                                    }
                                }
                            }
                        }
                    }


                    // Lưu file Excel
                    package.SaveAs(new FileInfo(exactPath));
                    HandleFile.DownloadFile("Excel", fileName);

                }
                catch (Exception ex)
                {
                    var innerMessage = ex.InnerException != null ? ex.InnerException.Message : "Không có thông tin chi tiết";
                    throw new Exception($"Lỗi: {ex.Message}. Chi tiết nội bộ: {innerMessage}", ex);
                }
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<List<List<CalendarEntry>>>> GetAllWorkShiftByPartimeEmployee(int employeeId, string startDate, string endDate)
        {
            try
            {
                //Kiểm tra xem có đúng định dạng DateOnly không ? 
                string dateFormat = "yyyy-MM-dd";
                bool isValidStartDate = DateOnly.TryParseExact(startDate, dateFormat, out DateOnly parsedStartDate);

                // Kiểm tra endDate
                bool isValidEndDate = DateOnly.TryParseExact(endDate, dateFormat, out DateOnly parsedEndDate);

                if (!isValidStartDate || !isValidEndDate)
                {
                    return new ApiResponse<List<List<CalendarEntry>>> { Message = [WorkShiftError.FAILED_REGULAR] };
                }

                //Chuyển sang dạng DateOnly
                DateOnly.TryParse(startDate, out DateOnly dateOnlyStartDate);
                DateOnly.TryParse(endDate, out DateOnly dateOnlyEndDate);

                //Kiểm tra xem nhân viên đang xét có phải là nhân viên partime không ?
                var employeeType = await (from em in _employeeRepository.GetAllQueryAble()
                                          join c in _contractRepository.GetAllQueryAble() on em.ContractId equals c.Id
                                          select c.TypeContract).FirstAsync();

                if (employeeType == TypeContract.Fulltime)
                {
                    return new ApiResponse<List<List<CalendarEntry>>> { Message = [WorkShiftError.NOT_PARTIME_EMPLOYEE] };
                }


                var calendarEntries = await GetAllWorkShiftAttendanceTracking(employeeId, dateOnlyStartDate, dateOnlyEndDate);
                int groupSize = 7; // 1 tuần sẽ có 7 ngày
                var groupedArrayCalendarEntriess = calendarEntries
                    .Select((value, index) => new { value, index })
                    .GroupBy(x => x.index / groupSize)
                    .Select(g => g.Select(x => x.value).ToList())
                    .ToList();

                return new ApiResponse<List<List<CalendarEntry>>> { IsSuccess = true, Metadata = groupedArrayCalendarEntriess };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<List<CalendarEntry>> GetAllWorkShiftAttendanceTracking(int employeeId, DateOnly startDate, DateOnly endDate)
        {
            //Chỉ lấy ra những cái user calendar mà nó trong trạng thái là approved
            var userCalendars = await (from uc in _userCalendarRepository.GetAllQueryAble()
                                       join pt in _partimePlanRepository.GetAllQueryAble() on uc.PartimePlanId equals pt.Id
                                       where uc.PresentShift >= startDate && uc.PresentShift <= endDate //Thuộc khoảng đang xét
                                       && pt.StatusCalendar == StatusCalendar.Approved //Những kế hoạch được chấp nhận
                                       && uc.UserCalendarStatus == UserCalendarStatus.Approved //Những ngày được chấp nhận
                                       && pt.EmployeeId == employeeId //Thuộc ông employee đang xét
                                       orderby uc.PresentShift ascending //Lấy theo ngày tháng tăng dần
                                       select new UserCalendarResult
                                       {
                                           ShiftTime = uc.ShiftTime,
                                           PresentShift = uc.PresentShift,
                                           UserCalendarStatus = uc.UserCalendarStatus,
                                       }).ToListAsync();

            //Lấy ra những lần chấm công
            var histories = await _historyRepository
            .GetAllQueryAble()
            .Where(e => DateOnly.FromDateTime(e.TimeSweep) >= startDate
            && DateOnly.FromDateTime(e.TimeSweep) <= endDate
            && e.EmployeeId == employeeId)
            .OrderBy(e => e.TimeSweep)
            .Select(e => new HistoryResult
            {
                Id = e.Id,
                TimeSweep = e.TimeSweep,
                EmployeeId = e.EmployeeId,
                StatusHistory = e.StatusHistory,
            })
            .ToListAsync();

            //Lấy ra tất cả những ngày trong khoảng thời gian cho trước
            var daysInRange = Enumerable.Range(0, (endDate.ToDateTime(TimeOnly.MinValue) - startDate.ToDateTime(TimeOnly.MinValue)).Days + 1)
                .Select(offset => new
                {
                    Date = startDate.AddDays(offset),
                    DayOfWeek = startDate.AddDays(offset).DayOfWeek.ToString()
                })
                .ToList();



            // Tạo danh sách kết quả
            var calendarEntries = new List<CalendarEntry>();

            // Duyệt qua từng ngày và thêm dữ liệu hoặc null nếu không có
            foreach (var date in daysInRange)
            {
                //Lấy những ngày mà có lịch làm
                var shiftsForDay = userCalendars.Where(d => d.PresentShift == date.Date).ToList();

                //Lấy những ca làm việc có tháng năm bằng date
                var historiesForDay = histories
                    .Where(e => DateOnly.FromDateTime(e.TimeSweep) == date.Date)
                    .OrderBy(e => e.TimeSweep)
                    .ToList();

                // Nhóm lịch sử theo ca (ShiftTime)
                var historyGroupedByShift = historiesForDay
                    .GroupBy(e => GetShiftTime(e.TimeSweep.TimeOfDay)) // Hàm GetShiftTime sẽ xác định ca dựa trên thời gian
                    .ToDictionary(g => g.Key, g => g.Select(h => new HistoryResult
                    {
                        Id = h.Id,
                        TimeSweep = h.TimeSweep,
                        EmployeeId = h.EmployeeId,
                        StatusHistory = h.StatusHistory,
                    }).ToList());

                calendarEntries.Add(new CalendarEntry
                {
                    DayOfWeek = date.DayOfWeek,
                    Date = date.Date,
                    UserCalendarResult = shiftsForDay.Any() ? shiftsForDay : null,
                    HistoryEntryResults = historyGroupedByShift.Any() ? historyGroupedByShift : null,
                });
            }
            return calendarEntries;
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

                if (partimePlan.TimeStart <= DateOnly.FromDateTime(DateTime.Now))
                {
                    return new ApiResponse<bool> { Message = [WorkShiftError.FORBIDDEN_OVERDUE] };
                }


                var currentRole = _userCalendarRepository.Context.GetCurrentUserRole();
                if (currentRole != Role.Admin)
                {
                    return new ApiResponse<bool> { Message = [WorkShiftError.FORBIDDEN_PLAN] };
                }

                //Những ngày đã được đăng kí trước sẽ bị đè vào
                /*Lấy những ngày đã đăng kí trong khoảng thời gian này*/
                var userCalendarIdDeletes = await _userCalendarRepository
                    .GetAllQueryAble()
                    .Where(e => e.PresentShift <= partimePlan.TimeEnd
                    && e.PresentShift >= partimePlan.TimeStart
                    && (e.UserCalendarStatus == UserCalendarStatus.Submit
                    || e.UserCalendarStatus == UserCalendarStatus.Approved))
                    .ExecuteUpdateAsync(s => s.SetProperty(w => w.UserCalendarStatus, UserCalendarStatus.Inactive));


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

        public async Task<ApiResponse<List<PartimePlanResult>>> GetAllPartimePlanByCurrentEmployeeId()
        {
            try
            {
                var employeeId = _employeeRepository.Context.GetCurrentUserId();
                var employeeRole = _employeeRepository.Context.GetCurrentUserRole();

                if (employeeRole != Role.Partime)
                {
                    return new ApiResponse<List<PartimePlanResult>> { Message = [WorkShiftError.NOT_PARTIME_EMPLOYEE] };
                }

                var partimePlans = await (from pt in _partimePlanRepository.GetAllQueryAble()
                                          join em in _employeeRepository.GetAllQueryAble() on pt.EmployeeId equals em.Id
                                          join c in _contractRepository.GetAllQueryAble() on em.ContractId equals c.Id
                                          where em.Id == employeeId
                                          select new PartimePlanResult
                                          {
                                              TimeStart = pt.TimeStart,
                                              TimeEnd = pt.TimeEnd,
                                              StatusCalendar = pt.StatusCalendar,
                                              CreatedAt = pt.CreatedAt,
                                              EmployeeName = c.Name,
                                              Id = pt.Id,
                                              DiffTime = pt.TimeEnd.DayNumber - pt.TimeStart.DayNumber
                                          }).OrderByDescending(e => e.CreatedAt).ToListAsync();

                return new ApiResponse<List<PartimePlanResult>> { IsSuccess = true, Metadata = partimePlans };
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
                                          }).OrderByDescending(e => e.CreatedAt).ToListAsync();
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

                /*Lấy ra id của nhân viên hiện tại*/
                int employeeId = _partimePlanRepository
                    .Context.GetCurrentUserId();

                var role = _partimePlanRepository
                    .Context.GetCurrentUserRole();

                if (role == Role.Admin || role == Role.FullTime)
                {
                    return new ApiResponse<bool> { Message = [WorkShiftError.NOT_PARTIME_EMPLOYEE] };
                }

                var employeeTypeContract = await (from em in _employeeRepository.GetAllQueryAble()
                                                  join c in _contractRepository.GetAllQueryAble() on em.ContractId equals c.Id
                                                  where em.Id == employeeId
                                                  select c.TypeContract).FirstAsync();

                if (employeeTypeContract == TypeContract.Fulltime)
                {
                    return new ApiResponse<bool> { Message = [WorkShiftError.NOT_PARTIME_EMPLOYEE] };
                }


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
                        UserCalendarStatus = e.UserCalendarStatus,
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
