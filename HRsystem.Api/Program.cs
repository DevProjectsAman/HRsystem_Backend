using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.Entities;
using HRsystem.Api.Features.Auth.UserManagement;
using HRsystem.Api.Features.Company;
using HRsystem.Api.Features.Groups.Create;
using HRsystem.Api.Features.Groups.DeleteGroup;
using HRsystem.Api.Features.Groups.GetALL;
using HRsystem.Api.Features.Groups.GetALlGroup;
using HRsystem.Api.Features.Groups.UpdateGroup;
using HRsystem.Api.Features.JobManagment;
using HRsystem.Api.Features.Shift;
using HRsystem.Api.Features.ShiftRule;
using HRsystem.Api.Features.SystemAdmin.RolePermission;
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
using System.Text;
using Microsoft.OpenApi.Models;
using HRsystem.Api.Features.Shift;
using HRsystem.Api.Features.ShiftRule;






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


builder.Services.AddValidatorsFromAssemblyContaining<CreateGroupValidator>();

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

app.Run();



 
