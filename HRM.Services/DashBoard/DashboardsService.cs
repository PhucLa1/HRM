using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Repositories.Setting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections;


namespace HRM.Services.Dashboard
{
    public interface IDashboardsService
    {
        Task<ApiResponse<int>> GetJobPostingCount();
        Task<ApiResponse<int>> GetApplicantCount();
        Task<ApiResponse<IEnumerable<ListSalaryResult>>> GetEmployeeCountByBaseSalary();
        Task<ApiResponse<IEnumerable<ApplicantForPositionResult>>> GetApplicantForPosition();
        Task<ApiResponse<int>> GetAdvanceCountByPeriod(string start, string end);
        Task<ApiResponse<IEnumerable<LeaveApplication>>> GetLeaveApplicationsToday();
        Task<ApiResponse<IEnumerable<Contract>>> GetExpiringContracts(string expirationDate);
        Task<ApiResponse<List<Dictionary<string, TypeResult>>>> FlexibleDashboard();
        Task<ApiResponse<List<PageFlexibleDashboardResult>>> GetAllFlexibleDashboard();
        Task<ApiResponse<PageFlexibleDashboardResult>> CreateNewPageFlexibleDashboard();
        Task<ApiResponse<List<ChartResult>>> GetAllChartByPageFlexibleId(int pageId);
        Task<ApiResponse<bool>> CreateNewChart(ChartUpsert chartAdd);
    }
    public class DashboardsService : IDashboardsService
    {
        private readonly IBaseRepository<Applicants> _applicantsRepository;
        private readonly IBaseRepository<Advance> _advancesRepository;
        private readonly IBaseRepository<Position> _positionRepository;
        private readonly IBaseRepository<Contract> _contractRepository;
        private readonly IBaseRepository<Employee> _employeeRepository;
        private readonly IBaseRepository<ContractSalary> _contractSalaryRepository;
        private readonly IBaseRepository<JobPosting> _jobPostingRepository;
        private readonly IBaseRepository<LeaveApplication> _leaveRepository;
        private readonly IBaseRepository<Department> _departmentRepository;
        private readonly IBaseRepository<ContractType> _contractTypeRepository;
        private readonly IBaseRepository<PageFlexibleDashboard> _pageFlexibleDashboardRepository;
        private readonly IBaseRepository<Chart> _chartRepository;
        private readonly CompanySetting _serverCompanySetting;
        private const string FOLDER_DASHBOARD_NAME = "Dashboard";
        private readonly IValidator<ChartUpsert> _chartUpsertValidator;

        public DashboardsService(
            IBaseRepository<Position> positionRepository,
            IBaseRepository<Contract> contractRepository,
            IBaseRepository<JobPosting> jobPostingRepository,
            IBaseRepository<Applicants> applicantRepository,
            IBaseRepository<Employee> employeeRepository,
            IBaseRepository<ContractSalary> contractSalaryRepository,
            IBaseRepository<LeaveApplication> leaveRepository,
            IBaseRepository<Advance> advancesRepository,
            IBaseRepository<Department> departmentRepsitory,
            IBaseRepository<ContractType> contractTypeRepository,
            IBaseRepository<PageFlexibleDashboard> pageFlexibleDashboardRepository,
            IBaseRepository<Chart> chartRepository,
            IOptions<CompanySetting> serverCompanySetting,
             IValidator<ChartUpsert> chartUpsertValidator
            )
        {
            _positionRepository = positionRepository;
            _contractRepository = contractRepository;
            _jobPostingRepository = jobPostingRepository;
            _applicantsRepository = applicantRepository;
            _employeeRepository = employeeRepository;
            _contractSalaryRepository = contractSalaryRepository;
            _leaveRepository = leaveRepository;
            _advancesRepository = advancesRepository;
            _departmentRepository = departmentRepsitory;
            _contractTypeRepository = contractTypeRepository;
            _pageFlexibleDashboardRepository = pageFlexibleDashboardRepository;
            _chartRepository = chartRepository;
            _serverCompanySetting = serverCompanySetting.Value;
            _chartUpsertValidator = chartUpsertValidator;
        }

        public async Task<ApiResponse<bool>> CreateNewChart(ChartUpsert chartAdd)
        {
            try
            {
                var resultValidation = _chartUpsertValidator.Validate(chartAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }

                var chart = new Chart()
                {
                    PageFlexibleDashboardId = chartAdd.PageFlexibleDashboardId,
                    Data = chartAdd.Data,
                    Title = chartAdd.Title,
                    FirstDescription = chartAdd.FirstDescription,
                    SecondDescription = chartAdd.SecondDescription,
                    Size = chartAdd.Size,
                    PropertyName = chartAdd.PropertyName,
                    ChartType = chartAdd.ChartType,
                };
                await _chartRepository.AddAsync(chart);
                await _chartRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<List<ChartResult>>> GetAllChartByPageFlexibleId(int pageId)
        {
            try
            {
                var charts = await _chartRepository
                    .GetAllQueryAble()
                    .Where(e => e.PageFlexibleDashboardId == pageId)
                    .Select(e => new ChartResult
                    {
                        PageFlexibleDashboardId = e.PageFlexibleDashboardId,
                        Data = e.Data,
                        Title = e.Title,
                        FirstDescription = e.FirstDescription,
                        SecondDescription = e.SecondDescription,
                        Size = e.Size,
                        PropertyName = e.PropertyName,
                        ChartType = e.ChartType,
                    })
                    .ToListAsync();

                return new ApiResponse<List<ChartResult>> { IsSuccess = true , Metadata = charts};
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<PageFlexibleDashboardResult>> CreateNewPageFlexibleDashboard()
        {
            try
            {
                var pageFlexibleDashboard = new PageFlexibleDashboard()
                {
                    Url = "media.png"
                };
                await _pageFlexibleDashboardRepository.AddAsync(pageFlexibleDashboard);
                await _pageFlexibleDashboardRepository.SaveChangeAsync();
                var pageFlexibleDashboardResult = new PageFlexibleDashboardResult()
                {
                    Id = pageFlexibleDashboard.Id,
                    Title = pageFlexibleDashboard.Title,
                    Url = _serverCompanySetting.CompanyHost + "/" + FOLDER_DASHBOARD_NAME + "/" + pageFlexibleDashboard.Url,
                };
                return new ApiResponse<PageFlexibleDashboardResult> { IsSuccess = true, Metadata = pageFlexibleDashboardResult };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<ApiResponse<List<PageFlexibleDashboardResult>>> GetAllFlexibleDashboard()
        {
            try
            {
                return new ApiResponse<List<PageFlexibleDashboardResult>>
                {
                    IsSuccess = true,
                    Metadata = await _pageFlexibleDashboardRepository.GetAllQueryAble().Select(e => new PageFlexibleDashboardResult
                    {
                        Id = e.Id,
                        Title = e.Title,
                        Url = _serverCompanySetting.CompanyHost + "/" + FOLDER_DASHBOARD_NAME + "/" + e.Url,
                    }).ToListAsync()
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<ApiResponse<List<Dictionary<string, TypeResult>>>> FlexibleDashboard()
        {
            try
            {
                List<Type> types = new List<Type>()
                {
                    typeof(Contract),
                    typeof(Employee),
                    typeof(Department),
                    typeof(ContractType),
                    typeof(ContractSalary),
                    typeof(Position)
                };
                List<string> tableNames = new List<string>()
                {
                    "Contract",
                    "Employee",
                    "Department",
                    "ContractType",
                    "ContractSalary",
                    "Position"
                };
                List<string> propertyNames = new List<string>();

                //Duyệt qua các phần tử, lấy các tên của các properties có thể có 
                var index = 0;
                foreach (var type in types)
                {

                    var propertyName = type.GetProperties()
                            .Where(prop => !typeof(ICollection).IsAssignableFrom(prop.PropertyType)
                                  && (prop.PropertyType.IsValueType || prop.PropertyType == typeof(string))
                                  && !prop.Name.Contains("Id")
                                  && !prop.Name.Contains("CreatedAt")
                                  && !prop.Name.Contains("UpdatedAt")
                                  && !prop.Name.Contains("CreatedBy")
                                  && !prop.Name.Contains("UpdatedBy"))
                   .Select(prop => tableNames[index] + "_" + prop.Name)
                   .ToList();
                    propertyNames = propertyNames.Concat(propertyName).ToList();
                    index++;
                }


                //Lấy ra kết quả của các giá trị
                var query = await (from em in _employeeRepository.GetAllQueryAble()
                                   join c in _contractRepository.GetAllQueryAble() on em.ContractId equals c.Id
                                   join p in _positionRepository.GetAllQueryAble() on c.PositionId equals p.Id
                                   join d in _departmentRepository.GetAllQueryAble() on p.DepartmentId equals d.Id
                                   join cs in _contractSalaryRepository.GetAllQueryAble() on c.ContractSalaryId equals cs.Id
                                   join ct in _contractTypeRepository.GetAllQueryAble() on c.ContractTypeId equals ct.Id
                                   select new
                                   {
                                       Contract = c,
                                       Employee = em,
                                       Position = p,
                                       Department = d,
                                       ContractSalary = cs,
                                       ContractType = ct
                                   }).ToListAsync(); // Loads data into memory


                var result = query.Select(c =>
                {
                    var contractDictionary = new Dictionary<string, TypeResult>();
                    foreach (var properyName in propertyNames)
                    {
                        var property = properyName.Split("_")[0];
                        if (property == "Contract")
                        {
                            var contractInfo = c.Contract.GetType().GetProperty(properyName.Split("_")[1]);
                            var contractValue = contractInfo?.GetValue(c.Contract)?.ToString()!;
                            var contractType = contractInfo?.PropertyType.ToString();
                            contractDictionary.Add(properyName, new TypeResult
                            {
                                ValueType = contractType,
                                Value = contractValue
                            });
                        }
                        else if (property == "Employee")
                        {
                            var employeeInfo = c.Employee.GetType().GetProperty(properyName.Split("_")[1]);
                            var employeeValue = employeeInfo?.GetValue(c.Employee)?.ToString()!;
                            var employeeType = employeeInfo?.PropertyType.ToString();
                            contractDictionary.Add(properyName, new TypeResult
                            {
                                ValueType = employeeType,
                                Value = employeeValue
                            });
                        }
                        else if (property == "Department")
                        {
                            var departmentInfo = c.Department.GetType().GetProperty(properyName.Split("_")[1]);
                            var departmentValue = departmentInfo?.GetValue(c.Department)?.ToString()!;
                            var departmentType = departmentInfo?.PropertyType.ToString();
                            contractDictionary.Add(properyName, new TypeResult
                            {
                                ValueType = departmentType,
                                Value = departmentValue
                            });
                        }
                        else if (property == "ContractType")
                        {
                            var contractTypeInfo = c.ContractType.GetType().GetProperty(properyName.Split("_")[1]);
                            var contractTypeValue = contractTypeInfo?.GetValue(c.ContractType)?.ToString()!;
                            var contractTypeType = contractTypeInfo?.PropertyType.ToString();
                            contractDictionary.Add(properyName, new TypeResult
                            {
                                ValueType = contractTypeType,
                                Value = contractTypeValue
                            });
                        }
                        else if (property == "ContractSalary")
                        {
                            var contractSalaryInfo = c.ContractSalary.GetType().GetProperty(properyName.Split("_")[1]);
                            var contractSalaryValue = contractSalaryInfo?.GetValue(c.ContractSalary)?.ToString()!;
                            var contractSalaryType = contractSalaryInfo?.PropertyType.ToString();
                            contractDictionary.Add(properyName, new TypeResult
                            {
                                ValueType = contractSalaryType,
                                Value = contractSalaryValue
                            });
                        }
                        else if (property == "Position")
                        {
                            var positionInfo = c.Position.GetType().GetProperty(properyName.Split("_")[1]);
                            var positionValue = positionInfo?.GetValue(c.Position)?.ToString()!;
                            var positionType = positionInfo?.PropertyType.ToString();
                            contractDictionary.Add(properyName, new TypeResult
                            {
                                ValueType = positionType,
                                Value = positionValue
                            });
                        }
                    }
                    return contractDictionary;

                }).ToList();

                return new ApiResponse<List<Dictionary<string, TypeResult>>> { IsSuccess = true, Metadata = result };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<IEnumerable<ListSalaryResult>>> GetEmployeeCountByBaseSalary()
        {
            try
            {
                var contractSalaries = await _contractSalaryRepository
                    .GetAllQueryAble()
                    .GroupJoin(
                        _contractRepository.GetAllQueryAble(),
                        cs => cs.Id,
                        c => c.ContractSalaryId,
                        (cs, contracts) => new
                        {
                            ContractSalary = cs,
                            Contracts = contracts
                        })
                    .Select(g => new ListSalaryResult
                    {
                        BaseSalary = g.ContractSalary.BaseSalary,
                        Count = g.Contracts.Count()
                    })
                    .ToListAsync();

                return new ApiResponse<IEnumerable<ListSalaryResult>>
                {
                    Metadata = contractSalaries,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<IEnumerable<LeaveApplication>>> GetLeaveApplicationsToday()
        {
            var today = DateTime.UtcNow.Date;
            try
            {
                var leaveApplications = await _leaveRepository.GetAllQueryAble()
                    .Where(l => l.CreatedAt.Date == today)
                    .ToListAsync();

                return new ApiResponse<IEnumerable<LeaveApplication>>
                {
                    Metadata = leaveApplications,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<IEnumerable<Contract>>> GetExpiringContracts(string expirationDate)
        {

            DateOnly endDate = DateOnly.Parse(expirationDate);
            var startDate = endDate.AddDays(-30);

            try
            {
                var expiringContracts = await _contractRepository.GetAllQueryAble()
                    .Where(c => DateOnly.FromDateTime(c.EndDate) <= endDate && DateOnly.FromDateTime(c.EndDate) >= startDate)
                    .ToListAsync();

                return new ApiResponse<IEnumerable<Contract>>
                {
                    Metadata = expiringContracts,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<int>> GetAdvanceCountByPeriod(string startDateInput, string endDateInput)
        {

            DateOnly startDate = DateOnly.Parse(startDateInput);
            DateOnly endDate = DateOnly.Parse(endDateInput);

            if (endDate < startDate)
            {
                throw new ArgumentException("End date must be greater than or equal to start date.");
            }

            try
            {
                // Query the database and count the advances within the specified period
                var advanceCount = await _advancesRepository.GetAllQueryAble()
                    .Where(a => DateOnly.FromDateTime(a.CreatedAt) >= startDate && DateOnly.FromDateTime(a.CreatedAt) <= endDate)
                    .CountAsync();

                return new ApiResponse<int>
                {
                    Metadata = advanceCount,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<int>> GetJobPostingCount()
        {
            try
            {
                var count = await _jobPostingRepository.GetAllQueryAble().CountAsync();
                return new ApiResponse<int>
                {
                    Metadata = count,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<int>> GetApplicantCount()
        {
            try
            {
                var count = await _applicantsRepository.GetAllQueryAble().CountAsync();
                return new ApiResponse<int>
                {
                    Metadata = count,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<IEnumerable<ApplicantForPositionResult>>> GetApplicantForPosition()
        {
            try
            {
                var positions = await _positionRepository.GetAllQueryAble().ToListAsync();
                var applicants = await _applicantsRepository.GetAllQueryAble().ToListAsync();

                var applicantCounts = applicants
                    .GroupBy(a => a.PositionId)
                    .Select(g => new
                    {
                        PositionId = g.Key,
                        Count = g.Count()
                    })
                    .ToList();

                var results = positions.Select(p => new ApplicantForPositionResult
                {
                    Name = p.Name,
                    Count = applicantCounts.FirstOrDefault(ac => ac.PositionId == p.Id)?.Count ?? 0
                })
                .OrderByDescending(r => r.Count)
                .Take(10)
                .ToList();

                return new ApiResponse<IEnumerable<ApplicantForPositionResult>>
                {
                    Metadata = results,
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
