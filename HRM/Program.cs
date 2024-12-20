﻿using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Coravel;
using FluentValidation;
using HRM.Apis.Setting;
using HRM.Apis.Swagger;
using HRM.Data.Data;
using HRM.Data.Entities;
using HRM.Data.Jwt;
using HRM.Repositories;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Setting;
using HRM.Services.Briefcase;
using HRM.Services.Dashboard;
using HRM.Services.Recruitment;
using HRM.Services.RecruitmentManager;
using HRM.Services.Salary;
using HRM.Services.Scheduler;
using HRM.Services.TimeKeeping;
using HRM.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add DB Connect SQLServer
builder.Services.AddDbContext<HRMDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HRMDbContext")));
builder.Services.AddHttpClient();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder => builder
            .WithOrigins("http://127.0.0.1:5500", "http://localhost:3000") // Điền vào tên miền của dự án giao diện của bạn
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials() // Cho phép sử dụng credentials (cookies, xác thực)
    );
});
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
})
.AddMvc() // This is needed for controllers
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

#region + Validation
//Validation
builder.Services.AddScoped<IValidator<PositionUpsert>, PositionUpsertValidator>();
builder.Services.AddScoped<IValidator<AllowanceUpsert>, AllowanceUpsertValidator>();
builder.Services.AddScoped<IValidator<InsuranceUpsert>, InsuranceUpsertValidator>();
builder.Services.AddScoped<IValidator<ContractTypeUpsert>, ContractTypeUpsertValidator>();
builder.Services.AddScoped<IValidator<ContractSalaryUpsert>, ContractSalaryUpsertValidator>();
builder.Services.AddScoped<IValidator<AccountLogin>, AccountLoginValidator>();
builder.Services.AddScoped<IValidator<DepartmentUpsert>, DepartmentUpsertValidator>();
builder.Services.AddScoped<IValidator<DeductionUpsert>, DeductionUpsertValidator>();
builder.Services.AddScoped<IValidator<BonusUpsert>, BonusUpsertValidator>();
builder.Services.AddScoped<IValidator<TaxDeductionUpsert>, TaxDeductionUpsertValidator>();
builder.Services.AddScoped<IValidator<TaxRateUpsert>, TaxRateUpsertValidator>();
builder.Services.AddScoped<IValidator<FomulaUpsert>, FomulaUpsertValidator>();
builder.Services.AddScoped<IValidator<QuestionUpsert>, QuestionUpsertValidator>();
builder.Services.AddScoped<IValidator<TestUpsert>, TestUpsertValidator>();
builder.Services.AddScoped<IValidator<WebUpsert>, WebUpsertValidator>();
builder.Services.AddScoped<IValidator<ContractAdd>, ContractAddValidator>();
builder.Services.AddScoped<IValidator<ContractUpdate>, ContractUpdateValidator>();
builder.Services.AddScoped<IValidator<GmailUpsert>, GmailUpsertValidator>();
builder.Services.AddScoped<IValidator<JobPostingUpsert>, JobPostingUpsertValidator>();
builder.Services.AddScoped<IValidator<CalendarUpsert>, CalendarUpsertValidator>();
builder.Services.AddScoped<IValidator<WorkPlanInsert>, WorkPlanInsertValidator>();
builder.Services.AddScoped<IValidator<UserCalendarInsert>, UserCalendarInsertValidator>();
builder.Services.AddScoped<IValidator<HistoryUpsert>, HistoryUpsertValidator>();
builder.Services.AddScoped<IValidator<FaceRegis>, FaceRegisValidator>();
builder.Services.AddScoped<IValidator<FaceRegisUpdate>, FaceRegisUpdateValidator>();
builder.Services.AddScoped<IValidator<AdvanceUpsert>, AdvanceUpsertValidator>();
builder.Services.AddScoped<IValidator<RecruitmentWebUpsert>, RecruitmentWebUpsertValidator>();
builder.Services.AddScoped<IValidator<ContractUpsert>, ContractUpsertValidator>();
builder.Services.AddScoped<IValidator<LeaveApplicationUpSert>, LeaveApplicationValidator>();
builder.Services.AddScoped<IValidator<ApplicantUpsert>, ApplicantUpsertValidator>();
builder.Services.AddScoped<IValidator<TestResultUpsert>, TestResultUpsertValidator>();
builder.Services.AddScoped<IValidator<AccountUpdate>, AccountUpdateValidator>();

builder.Services.AddScoped<IValidator<EmployeeUpsert>, EmployeeUpsertValidator>();
builder.Services.AddScoped<IValidator<ChartUpsert>, ChartUpsertValidator>();

builder.Services.AddScoped<IValidator<ApplicantTestUpdate>, ApplicantTestUpdateValidator>();

#endregion



#region + Repositories
//Repositories
builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

#endregion



#region + Services

//Services
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPositionsService, PositionsService>();
builder.Services.AddScoped<IAllowancesService, AllowancesService>();
builder.Services.AddScoped<IInsurancesService, InsurancesService>();
builder.Services.AddScoped<IContractTypesService, ContractTypesService>();
builder.Services.AddScoped<IContractSalarysService, ContractSalarysService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IQuestionsService, QuestionsService>();
builder.Services.AddScoped<ITestsService, TestsService>();
builder.Services.AddScoped<IWebsService, WebsService>();
builder.Services.AddScoped<IDeductionsService, DeductionsService>();
builder.Services.AddScoped<IBonusService, BonusService>();
builder.Services.AddScoped<ITaxDeductionsService, TaxDeductionsService>();
builder.Services.AddScoped<ITaxRatesService, TaxRatesService>();
builder.Services.AddScoped<IFomulasService, FomulasService>();
builder.Services.AddScoped<IDepartmentsService, DepartmentsService>();
builder.Services.AddScoped<IContractsService, ContractsService>();
builder.Services.AddScoped<IGmailsService, GmailsService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddScoped<IWorkShiftService, WorkShiftService>();
builder.Services.AddScoped<IEmployeesService, EmployeesService>();
builder.Services.AddScoped<IJobPostingsService, JobPostingsService>();
builder.Services.AddScoped<IEmployeesService, EmployeesService>();
builder.Services.AddScoped<IAdvancesService, AdvancesService>();
builder.Services.AddScoped<ILeaveApplicationsService, LeaveApplicationsService>();
builder.Services.AddScoped<IDashboardsService, DashboardsService>();
builder.Services.AddScoped<IPayrollsService, PayrollsService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeesService, EmployeesService>();
builder.Services.AddScoped<ILeaveApplicationsService, LeaveApplicationsService>();
builder.Services.AddScoped<IRecruitmentWebsService, RecruitmentWebsService>();
builder.Services.AddScoped<ILinkedInPostService, LinkedinPostsService>();
builder.Services.AddScoped<IApplicantsService, ApplicantsService>();
builder.Services.AddScoped<ITestResultsService, TestResultsService>();

#endregion

#region + Mapper


//Mapper
builder.Services.AddAutoMapper(typeof(MappingProfile));


#endregion

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddCookie(x =>
    {
        x.Cookie.Name = "token";
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            //2 dong
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["X-Access-Token"];
                return Task.CompletedTask;
            }
        };
    });


#region Authorization


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(RoleExtensions.ADMIN_ROLE, policy => policy.RequireClaim("Role", Role.Admin.ToString())); 
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(RoleExtensions.PARTIME_ROLE, policy => policy.RequireClaim("Role", Role.Partime.ToString())); 
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(RoleExtensions.FULLTIME_ROLE, policy => policy.RequireClaim("Role", Role.FullTime.ToString()));
});


#endregion



//Logging
builder.Host.UseSerilog((context, configuration) => 
    configuration.ReadFrom.Configuration(context.Configuration)
);


#region  Setting


//Setting config
builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("Email"));
builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<CompanySetting>(builder.Configuration.GetSection("Company"));

#endregion



#region Scheduler
builder.Services.AddScheduler();
builder.Services.AddTransient<ChangePartimePlanStatus>();

#endregion


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    options.ExampleFilters();
});
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();
var app = builder.Build();


#region Scheduler
app.Services.UseScheduler(scheduler =>
{
    scheduler.Schedule<ChangePartimePlanStatus>()
        .EveryMinute()
        .PreventOverlapping(nameof(ChangePartimePlanStatus));
});
#endregion


//Seeding data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Hiển thị từng phiên bản trong UI Swagger
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                                    description.GroupName.ToUpperInvariant());
        }
    });
}
app.UseSerilogRequestLogging();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseJwtMiddleware();
app.UseCors("AllowOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

