using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.Entities;
using HRsystem.Api.Features.ActivityStatus;
using HRsystem.Api.Features.ActivityType;
using HRsystem.Api.Features.AuditLog;
using HRsystem.Api.Features.Auth.UserManagement;
using HRsystem.Api.Features.City;
using HRsystem.Api.Features.Company;
using HRsystem.Api.Features.Department;
using HRsystem.Api.Features.Employee;
using HRsystem.Api.Features.EmployeeAttendance;
using HRsystem.Api.Features.EmployeeVacation;
using HRsystem.Api.Features.Excuse;
using HRsystem.Api.Features.Gov;
using HRsystem.Api.Features.Groups.Create;
using HRsystem.Api.Features.Groups.DeleteGroup;
using HRsystem.Api.Features.Groups.GetALL;
using HRsystem.Api.Features.Groups.GetALlGroup;
using HRsystem.Api.Features.Groups.UpdateGroup;
using HRsystem.Api.Features.JobManagment;
using HRsystem.Api.Features.Mission;
using HRsystem.Api.Features.Project;
using HRsystem.Api.Features.Shift.Endpoints;
using HRsystem.Api.Features.ShiftEndpoints;
using HRsystem.Api.Features.ShiftRule;
using HRsystem.Api.Features.SystemAdmin.RolePermission;
using HRsystem.Api.Features.VacationRule;
using HRsystem.Api.Features.WorkLocation;
using HRsystem.Api.Services;
using HRsystem.Api.Services.AuditLog;
using HRsystem.Api.Services.Auth;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.EncryptText;
using HRsystem.Api.Shared.ExceptionHandling;
using HRsystem.Api.Shared.ValidationHandler;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;






var builder = WebApplication.CreateBuilder(args);



SimpleCrypto.Initialize(builder.Configuration);

// Read JWT settings from config
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);


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

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PipelineValidationHandler<,>));



builder.Services.AddScoped<AuditLogService>();


// Register your services
builder.Services.AddScoped<JwtService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

 
// builder.Services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
//builder.Services.AddScoped<IAuthorizationHandler, PermissionHandlerService>();



var app = builder.Build();


// Register global exception handler early in the pipeline
app.UseMiddleware<GlobalExceptionMiddleware>();


// Enable Swagger UI in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "HRsystem API v1");
        options.RoutePrefix = ""; // 👈 makes Swagger the root page
    });

}

app.UseHttpsRedirection();


// 🔥 apply CORS before authentication/authorization
app.UseCors("AllowBlazorWasm");


app.UseAuthentication(); // Must come before Authorization
app.UseAuthorization();


app.MapUserManagementEndpoints();
 
//app.MapRoleAssignmentEndpoints();
 
 
app.MapGetGroup();
app.MapGetAllGroup();
app.MapUpdateGroup();
app.MapDeleteGroup();

app.MapJobLevelEndpoints();
app.MapJobTitleEndpoints();

app.MapAspPermissionsEndpoints();
app.MapAspRolePermissionsEndpoints();


app.MapCreateGroupEndpoint(); // from CreateGroupEndpoint.cs
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
app.MapVacationEndpoints();
app.MapEmployeeEndpoints();

app.Run();




 
