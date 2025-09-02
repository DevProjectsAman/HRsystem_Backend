using HRsystem.Api.Database.Entities;
using HRsystem.Api.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using HRsystem.Api.Features.Auth.Login;
using FluentValidation;
using HRsystem.Api.Features.Auth.ChangePassword;
using HRsystem.Api.Services;
using MediatR;
using HRsystem.Api.Features.SystemAdmin.Roles;
using HRsystem.Api.Features.SystemAdmin.RolePermision;
using HRsystem.Api.Features.SystemAdmin.Permissions;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Services.Auth;
using Microsoft.AspNetCore.Authorization;




var builder = WebApplication.CreateBuilder(args);


// Read JWT settings from config
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);


builder.Services.AddDbContext<IdentityDbContextHR>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<IdentityDbContextHR>()
    .AddDefaultTokenProviders();



// Register DbContext with connection string from appsettings.json
builder.Services.AddDbContext<BusinessDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

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


builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


// Register your services
builder.Services.AddScoped<JwtService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

 
builder.Services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandlerService>();


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


app.MapChangePassword();
app.MapRoleManagement();
app.MapRoleAssignmentEndpoints();
app.MapPermissionEndpoints();
app.MapLogin(); // from LoginEndpoint.cs




app.Run();



 
