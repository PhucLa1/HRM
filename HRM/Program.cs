using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using FluentValidation;
using HRM.Apis.Setting;
using HRM.Apis.Swagger;
using HRM.Data.Data;
using HRM.Data.Jwt;
using HRM.Repositories;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Repositories.Helper;
using HRM.Repositories.Setting;
using HRM.Services.Briefcase;
using HRM.Services.Recruitment;
using HRM.Services.RecruitmentManager;
using HRM.Services.Salary;
using HRM.Services.TimeKeeping;
using HRM.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Configuration;
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
builder.Services.AddScoped<IValidator<AdminLogin>, AdminLoginValidator>();
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

builder.Services.AddScoped<IValidator<CalendarUpsert>, CalendarUpsertValidator>();
#endregion



#region + Repositories
//Repositories
builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));


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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
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
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(RoleExtensions.ADMIN_ROLE, policy => policy.RequireClaim("Role", Role.Admin.ToString())); //1: User, 2: Admin
});

//Logging
builder.Host.UseSerilog((context, configuration) => 
    configuration.ReadFrom.Configuration(context.Configuration)
);


#region + Setting


//Setting config
builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("Email"));
builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<CompanySetting>(builder.Configuration.GetSection("Company"));

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
app.UseHttpsRedirection();
app.UseJwtMiddleware();
app.UseCors("AllowOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

