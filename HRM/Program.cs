using FluentValidation;
using HRM.Data.Data;
using HRM.Data.Jwt;
using HRM.Repositories;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Repositories.Setting;
using HRM.Services.Briefcase;
using HRM.Services.RecruitmentManager;
using HRM.Services.TimeKeeping;
using HRM.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

//Validation
builder.Services.AddScoped<IValidator<PositionUpsert>, PositionUpsertValidator>();
builder.Services.AddScoped<IValidator<AdminLogin>, AdminLoginValidator>();
builder.Services.AddScoped<IValidator<CalendarUpsert>, CalendarUpsertValidator>();
builder.Services.AddScoped<IValidator<WebUpsert>, WebUpsertValidator>();
builder.Services.AddScoped<IValidator<JobUpsert>, JobUpsertValidator>();
builder.Services.AddScoped<IValidator<TestUpsert>, TestUpsertValidator>();
builder.Services.AddScoped<IValidator<QuestionUpsert>, QuestionUpsertValidator>();
#endregion





#region


//Repositories
builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));


//Services
builder.Services.AddScoped<IPositionsService, PositionsService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddScoped<IWebsService, WebsService>();
builder.Services.AddScoped<IJobsService, JobsService>();
builder.Services.AddScoped<ITestsService, TestsService>();
builder.Services.AddScoped<IQuestionsService, QuestionsService>();

#endregion



#region


//Mapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

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
    options.AddPolicy("Role", policy => policy.RequireClaim("Role", "1")); //1: User, 2: Admin
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseJwtMiddleware();
app.UseCors("AllowOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
