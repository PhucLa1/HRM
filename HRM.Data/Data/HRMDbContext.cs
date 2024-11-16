using HRM.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HRM.Data.Data
{
    public class HRMDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HRMDbContext(DbContextOptions<HRMDbContext> options, IHttpContextAccessor httpContextAccessor)
    : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public HRMDbContext(DbContextOptions<HRMDbContext> options)
    : base(options) 
        { 
        }
        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFields()
        {
            var userId = GetCurrentUserId();

            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added))
            {
                if (entry.Entity is IAuditable)
                {
                    var entity = (IAuditable)entry.Entity;
                    entity.UpdatedAt = DateTime.UtcNow;
                    entity.UpdatedBy = userId;

                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedAt = DateTime.UtcNow;
                        entity.CreatedBy = userId;
                    }
                }
            }
        }

        public int GetCurrentUserId()
        {
            if (_httpContextAccessor == null || _httpContextAccessor.HttpContext == null)
            {
                return 0; 
            }
            if (!_httpContextAccessor.HttpContext.Items.ContainsKey("UserId"))
            {
                return 0; 
            }
            var userIdObj = _httpContextAccessor.HttpContext.Items["UserId"];
            if (userIdObj == null)
            {
                return 0;
            }
            if (int.TryParse(userIdObj.ToString(), out int userId))
            {
                return userId;
            }
            return 0;
        }

        public Role GetCurrentUserRole()
        {
            if (_httpContextAccessor == null || _httpContextAccessor.HttpContext == null)
            {
                return 0;
            }
            if (!_httpContextAccessor.HttpContext.Items.ContainsKey("UserRole"))
            {
                return 0;
            }
            var userRoleObj = _httpContextAccessor.HttpContext.Items["UserRole"];
            if (userRoleObj == null)
            {
                return 0;
            }
            return ConvertStringToEnumRole(userRoleObj.ToString()!);
        }
        private Role ConvertStringToEnumRole(string role)
        {
            if (role == "Admin") return Role.Admin;
            else if (role == "Partime") return Role.Partime;
            else return Role.FullTime;
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Advance> Advances { get; set; }
        public DbSet<Allowance> Allowances { get; set; }
        public DbSet<Applicants> Applicants { get; set; }
        public DbSet<Bonus> Bonus { get; set; }
        public DbSet<BonusDetails> BonusDetails { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ContractAllowance> ContractAllowances { get; set; }
        public DbSet<ContractInsurance> ContractInsurances { get; set; }
        public DbSet<ContractSalary> ContractSalaries { get; set; }
        public DbSet<ContractType> ContractTypes { get; set; }
        public DbSet<Deduction> Deductions { get; set; }
        public DbSet<DeductionDetails> DeductionDetails { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Fomula> Fomulas { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Insurance> Insurances { get; set; }
        public DbSet<JobPosting> JobPostings { get; set; }
        public DbSet<LeaveApplication> LeaveApplications { get; set; }
        public DbSet<PartimePlan> PartimePlans { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<RecruitmentWeb> RecruitmentWebs { get; set; }
        public DbSet<TaxDeduction> TaxDeductions { get; set; }
        public DbSet<TaxDeductionDetails> TaxDeductionDetails { get; set; }
        public DbSet<TaxRate> TaxRates { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<UserCalendar> UserCalendars { get; set; }
        public DbSet<Web> Webs { get; set; }
        public DbSet<EmployeeImage> EmployeeImages { get; set; }
        public DbSet<PageFlexibleDashboard> PageFlexibleDashboards { get; set; }
        public DbSet<Chart> Charts { get; set; }

    }
}

