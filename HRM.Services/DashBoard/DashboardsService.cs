using FluentValidation;
using HRM.Data.Data;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HRM.Services.Dashboard
{
    public interface IDashboardsService{
        Task<ApiResponse<int>> GetJobPostingCount();
        Task<ApiResponse<int>> GetApplicantCount();
        Task<ApiResponse<IEnumerable<ListSalaryResult>>> GetEmployeeCountByBaseSalary();
        Task<ApiResponse<IEnumerable<ApplicantForPositionResult>>> GetApplicantForPosition();
        Task<ApiResponse<int>> GetAdvanceCountByPeriod(string start, string end);
        Task<ApiResponse<IEnumerable<LeaveApplication>>> GetLeaveApplicationsToday();
        Task<ApiResponse<IEnumerable<Contract>>> GetExpiringContracts(string expirationDate);
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
        private readonly IBaseRepository<Applicants> _applicantRepository;
        private readonly IBaseRepository<LeaveApplication> _leaveRepository;

        public DashboardsService(
            IBaseRepository<Position> positionRepository,
            IBaseRepository<Contract> contractRepository,
            IBaseRepository<JobPosting> jobPostingRepository,
            IBaseRepository<Applicants> applicantRepository,
            IBaseRepository<Employee> employeeRepository,
            IBaseRepository<ContractSalary> contractSalaryRepository,
            IBaseRepository<LeaveApplication> leaveRepository,
            IBaseRepository<Advance> advancesRepository
            )
        {
            _positionRepository = positionRepository;
            _contractRepository = contractRepository;
            _jobPostingRepository = jobPostingRepository;
            _applicantRepository = applicantRepository;
            _employeeRepository = employeeRepository;
            _contractSalaryRepository = contractSalaryRepository;
            _leaveRepository = leaveRepository;
            _advancesRepository = advancesRepository;
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
                var count = await _applicantRepository.GetAllQueryAble().CountAsync();
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
                var applicants = await _applicantRepository.GetAllQueryAble().ToListAsync();

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
