using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.Entities;
using HRsystem.Api.Features.ActivityStatus;
using HRsystem.Api.Features.ActivityStatus.CreateActivityStatus;
using HRsystem.Api.Features.ActivityStatus.DeleteActivityStatus;
using HRsystem.Api.Features.ActivityStatus.GetActivityStatusById;
using HRsystem.Api.Features.ActivityStatus.GetAllActivityStatuses;
using HRsystem.Api.Features.ActivityStatus.UpdateActivityStatus;
using HRsystem.Api.Features.ActivityType;
using HRsystem.Api.Features.ActivityType.CreateActivityType;
using HRsystem.Api.Features.ActivityType.UpdateActivityType;
using HRsystem.Api.Features.AuditLog;
using HRsystem.Api.Features.AuditLog.CreateAuditLog;
using HRsystem.Api.Features.AuditLog.UpdateAuditLog;
using HRsystem.Api.Features.Auth.UserManagement;
using HRsystem.Api.Features.City;
using HRsystem.Api.Features.City.CreateCity;
using HRsystem.Api.Features.City.UpdateCity;
using HRsystem.Api.Features.Company;
using HRsystem.Api.Features.Company.CreateCompany;
using HRsystem.Api.Features.Company.UpdateCompany;
using HRsystem.Api.Features.Department;
using HRsystem.Api.Features.Department.CreateDepartment;
using HRsystem.Api.Features.Department.UpdateDepartment;
using HRsystem.Api.Features.Gov;
using HRsystem.Api.Features.Gov.CreateGov;
using HRsystem.Api.Features.Gov.UpdateGov;
using HRsystem.Api.Features.Groups.Create;
using HRsystem.Api.Features.Groups.DeleteGroup;
using HRsystem.Api.Features.Groups.GetALL;
using HRsystem.Api.Features.Groups.GetALlGroup;
using HRsystem.Api.Features.Groups.UpdateGroup;
using HRsystem.Api.Features.JobManagment;
using HRsystem.Api.Features.Project;
using HRsystem.Api.Features.Project.CreateProject;
using HRsystem.Api.Features.Project.UpdateProject;
using HRsystem.Api.Features.Shift;
using HRsystem.Api.Features.Shift;
using HRsystem.Api.Features.Shift.Endpoints;
using HRsystem.Api.Features.ShiftEndpoints;
using HRsystem.Api.Features.ShiftRule;
using HRsystem.Api.Features.ShiftRule;
using HRsystem.Api.Features.SystemAdmin.RolePermission;
using HRsystem.Api.Features.VacationRule;
using HRsystem.Api.Features.VacationRule.CreateVacationRule;
using HRsystem.Api.Features.VacationRule.UpdateVacationRule;
using HRsystem.Api.Features.VacationType.CreateVacationType;
using HRsystem.Api.Features.VacationType.UpdateVacationType;
using HRsystem.Api.Features.WorkLocation;
using HRsystem.Api.Features.WorkLocation.CreateWorkLocation;
using HRsystem.Api.Features.WorkLocation.UpdateWorkLocation;
using HRsystem.Api.Services;
using HRsystem.Api.Services.Auth;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.Tools;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Models;
using System.Text;






var builder = WebApplication.CreateBuilder(args);


// Read JWT settings from config
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);


builder.Services.AddDbContext<DBContextHRsystem>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<DBContextHRsystem>()
    .AddDefaultTokenProviders();




//builder.Services.AddValidatorsFromAssemblyContaining<CreateGroupValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<CreateShiftRuleValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<UpdateShiftRuleValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<CreateVacationTypeValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<UpdateVacationTypeValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<CreateVacationRuleValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<UpdateVacationRuleValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<UpdateWorkLocationValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<CreateWorkLocationValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<UpdateActivityStatusValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<CreateActivityStatusValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<UpdateActivityTypeValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<CreateActivityTypeValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<UpdateGovValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<CreateGovValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<UpdateCityCommandValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<CreateCityCommandValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<CreateDepartmentValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<UpdateDepartmentValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<CreateCompanyValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<UpdateCompanyValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<UpdateAuditLogValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<CreateAuditLogValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<CreateProjectValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<UpdateProjectValidator>();



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

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


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

app.Run();




 
