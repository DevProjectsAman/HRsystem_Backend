
using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.Entities;
using HRsystem.Api.Features.AccessManagment.Auth.UserManagement;
using HRsystem.Api.Features.AccessManagment.SystemAdmin.RolePermission;
using HRsystem.Api.Features.AccessManagment.SystemAdmin.Roles;
using HRsystem.Api.Features.ActivityType;
using HRsystem.Api.Features.AuditLog;
using HRsystem.Api.Features.City;
using HRsystem.Api.Features.Employee;
using HRsystem.Api.Features.EmployeeApproval;
using HRsystem.Api.Features.EmployeeAttendance;
using HRsystem.Api.Features.EmployeeDashboard.EmployeeApp;
using HRsystem.Api.Features.EmployeeDashboard.EmployeeMonthlyReport;
using HRsystem.Api.Features.EmployeeDashboard.GetPendingActivities;
using HRsystem.Api.Features.EmployeeDashboard.ManagerActivity;
using HRsystem.Api.Features.EmployeeDashboard.mangeractivity;
using HRsystem.Api.Features.EmployeeHandler;
using HRsystem.Api.Features.EmployeeRequest.EmployeeVacation;
using HRsystem.Api.Features.EmployeeRequest.Execuse;
using HRsystem.Api.Features.employeevacations;
using HRsystem.Api.Features.Groups;
using HRsystem.Api.Features.Holiday;
using HRsystem.Api.Features.HolidayType;
using HRsystem.Api.Features.Lookups.ActivityStatus;
using HRsystem.Api.Features.Lookups.ActivityTypeStatus;
using HRsystem.Api.Features.Lookups.ContractTypes;
using HRsystem.Api.Features.Lookups.GeneralLookups;
using HRsystem.Api.Features.Lookups.MaretialStatus;
using HRsystem.Api.Features.Mission;
using HRsystem.Api.Features.Organization.Company;
using HRsystem.Api.Features.Organization.Department;
using HRsystem.Api.Features.Organization.Govermenet;
using HRsystem.Api.Features.Organization.JobManagment;
using HRsystem.Api.Features.Organization.Project;
using HRsystem.Api.Features.Organization.WorkLocation;
using HRsystem.Api.Features.Reports;
using HRsystem.Api.Features.Scheduling.RemoteWorkdays;
using HRsystem.Api.Features.Scheduling.Shift;
using HRsystem.Api.Features.Scheduling.ShiftRule;
using HRsystem.Api.Features.Scheduling.VacationRule;
using HRsystem.Api.Features.Scheduling.VacationRule.UpdateVacationRule;
using HRsystem.Api.Features.Scheduling.VacationRulesGroup;
using HRsystem.Api.Features.ShiftEndpoints;
using HRsystem.Api.Features.SystemAdmin.RolePermission;
using HRsystem.Api.Features.WorkDaysRules;
using HRsystem.Api.Services;
using HRsystem.Api.Services.AuditLog;
using HRsystem.Api.Services.Auth;
using HRsystem.Api.Services.Chatbot;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Services.DeviceEnforcement;
using HRsystem.Api.Services.LookupCashing;
using HRsystem.Api.Services.Reports;
using HRsystem.Api.Shared.EncryptText;
using HRsystem.Api.Shared.ExceptionHandling;
using HRsystem.Api.Shared.ValidationHandler;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;





var builder = WebApplication.CreateBuilder(args);



SimpleCrypto.Initialize(builder.Configuration);

// Read JWT settings from config
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);


// ✅ Make JSON property name matching case-insensitive globally
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});


builder.Services.AddDbContext<DBContextHRsystem>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));


builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Configure password requirements
    options.Password.RequiredLength = 3; // Minimum password length
    options.Password.RequireDigit = false; // Require at least one digit (0-9)
    options.Password.RequireLowercase = false; // Require at least one lowercase letter
    options.Password.RequireUppercase = false; // Require at least one uppercase letter
    options.Password.RequireNonAlphanumeric = false; // Require at least one special character (e.g., !@#$%)
    options.Password.RequiredUniqueChars = 2; // Require at least 4 unique characters
})
    .AddEntityFrameworkStores<DBContextHRsystem>()
    .AddDefaultTokenProviders();





// C# Code - Program.cs or Startup.cs
var alloworg = builder.Configuration.GetSection("Cors:AllowedOrigins");
var allowedOrigins = alloworg.Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorWasm",
        policy =>
        {
            policy.WithOrigins(allowedOrigins ?? Array.Empty<string>())
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});




// Add services to the container
builder.Services.AddEndpointsApiExplorer(); // Needed for minimal APIs
builder.Services.AddSwaggerGen(options =>
{
    // Swagger support for JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Register JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});

builder.Services.AddAuthorization();


builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(5); // how often to re-check
});

// Replace default with our version
builder.Services.AddScoped<ISecurityStampValidator, PermissionVersionValidator<ApplicationUser>>();


builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Configure FluentValidation globally - affects ALL validators
ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Continue;
ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Continue;



builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PipelineValidationHandler<,>));




builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<AuditLogService>();


// Register your services
builder.Services.AddScoped<JwtService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddSingleton<IActivityTypeLookupCache>(sp =>
{
    using var scope = sp.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<DBContextHRsystem>();

    var data = db.TbActivityTypes
        .Select(x => new ActivityTypeLookup
        {
            Id = x.ActivityTypeId,
            Code = x.ActivityCode,
            NameAr = x.ActivityName.ar,
            NameEn = x.ActivityName.en
        })
        .ToList();

    return new ActivityTypeLookupCache(data);
});

builder.Services.AddSingleton<IActivityStatusLookupCache>(sp =>
{
    using var scope = sp.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<DBContextHRsystem>();

    var data = db.TbActivityStatuses
        .Select(x => new ActivityStatusLookup
        {
            Id = x.StatusId,
            Code = x.StatusCode,
            NameAr = x.StatusName.ar,
            NameEn = x.StatusName.en,
            IsFinal = x.IsFinal
        })
        .ToList();

    return new ActivityStatusLookupCache(data);
});

//    for rate limiter


// Program.cs

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // Default global policy for all requests

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        // Get user ID if authenticated
        var userId = httpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        string key = !string.IsNullOrEmpty(userId) ? $"user:{userId}" : "anonymous";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: key,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = !string.IsNullOrEmpty(userId) ? 120 : 30,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            });
    });
});


// builder.Services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
//builder.Services.AddScoped<IAuthorizationHandler, PermissionHandlerService>();

// Monthly Report Service
builder.Services.AddScoped<IEmployeeMonthlyReportService, EmployeeMonthlyReportService>();
builder.Services.AddHostedService<EmployeeMonthlyReportScheduler>();


//chatbot
builder.Services.AddScoped<ChatbotService>();
builder.Services.AddScoped<IntentExecutorService>();
builder.Services.AddControllers();


var app = builder.Build();


// Register global exception handler early in the pipeline
app.UseMiddleware<GlobalExceptionMiddleware>();


// Enable Swagger UI in development
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
// Redirect root "/" to "/swagger"
app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "HRsystem API v1");

        // options.RoutePrefix = ""; // 👈 makes Swagger the root page
    });

//}

//app.UseSwagger();

//if (app.Environment.IsDevelopment())
//{
//    // 👇 In development: Swagger is the default (root) page
//    app.UseSwaggerUI(options =>
//    {
//        options.SwaggerEndpoint("/swagger/v1/swagger.json", "HRsystem API v1");
//        options.RoutePrefix = string.Empty; // root access: https://localhost:5001/
//    });
//}
//else
//{
//    // 👇 In production: Swagger only at /swagger
//    app.UseSwaggerUI(options =>
//    {
//        options.SwaggerEndpoint("/swagger/v1/swagger.json", "HRsystem API v1");
//        // No RoutePrefix override here
//    });
//}




app.UseHttpsRedirection();


// 🔥 apply CORS before authentication/authorization
app.UseCors("AllowBlazorWasm");


app.UseAuthentication(); // Must come before Authorization
app.UseAuthorization();


app.UseRateLimiter();


/// this to enforce device binding
// app.UseMiddleware<DeviceEnforcementMiddleware>();




//builder.Services.AddControllers();


app.MapUserManagementEndpoints();

//app.MapRoleAssignmentEndpoints();

app.MapJobLevelEndpoints();
app.MapJobTitleEndpoints();

app.MapAspPermissionsEndpoints();
app.MapAspRolePermissionsEndpoints();

app.MapGroupsEndpoint();

app.MapCompanyEndpoints();
app.MapShiftEndpoints();
app.MapShiftRuleEndpoints();
app.MapVacationRuleEndpoints();
app.MapVacationTypeEndpoints();
app.MapWorkLocationEndpoints();
app.MapActivityStatusEndpoints();
app.MapActivityTypeEndpoints();
app.MapGovEndpoints();
app.MapCityEndpoints();
app.MapDepartmentEndpoints();
app.MapAuditLogEndpoints();
app.MapProjectEndpoints();
app.MapMissionEndPoint();
app.MapExcuseEndPoint();
app.MapEmployeePunchEndpoints(); // from EmployeePunchEndpoints.cs
//app.MapEmployeeVacationsEndPoints();
app.MapEmployeeEndpoints();
app.MapEmployeeActivityApprovalEndpoints();



app.MapAspRoleEndpoints();

app.MapVacationRulesGroupEndpoints();

app.MapEmployeeVacationsEndPoints();
app.MapPendingActivitiesEndPoints();
app.MapPendingStatuesForManager();

app.MapMaritalStatusEndpoints();
app.MapGetAllNationalitiesEndpoint();
app.MapContractTypeEndpoints();




app.MapRemoteWorkDaysEndpoints();

app.MapHolidayTypeEndpoints();

app.MapActivityTypeStatusEndpoints();

app.MapWorkDaysRuleEndpoints();

app.MapWorkDaysEndpoints();

app.MapHolidayEndpoints();

app.MapEmployeeAppEndPoints();

 app.MapEmployeeHandlerEndpoints();



app.MapEmployeeReportEndpoints();
app.MapReportEndPoints();

// Rate Limitter applied to all controllers
app.MapControllers()   ;


app.Run();






