using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.Entities;
using HRsystem.Api.Features.AccessManagment.SystemAdmin.DTO;

using HRsystem.Api.Services.Auth;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared;
using HRsystem.Api.Shared.DTO;
using HRsystem.Api.Shared.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.IdentityModel.Tokens.Jwt;



namespace HRsystem.Api.Features.AccessManagment.Auth.UserManagement
{
    public static class UserManagementEndpoints
    {
        public static void MapUserManagementEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/AccessManagement").WithTags("_UserManagement");

            // 🔹 Auth & User Management

            group.MapPost("/login", async (LoginCommand command, ISender mediator, IValidator<LoginCommand> validator) =>
            {
                var validation = await validator.ValidateAsync(command);
                if (!validation.IsValid)
                    return Results.BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

                var result = await mediator.Send(command);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            }).RequireRateLimiting("LoginPolicy"); 

            group.MapPost("/register", [Authorize] async (RegisterUserCommand command, ISender mediator, IValidator<RegisterUserCommand> validator) =>
            {
                var validation = await validator.ValidateAsync(command);
                if (!validation.IsValid)
                    return Results.BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

                var result = await mediator.Send(command);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });


            group.MapPost("/user-update", [Authorize] async (UpdateUserCommand command, ISender mediator, IValidator<UpdateUserCommand> validator) =>
            {
                var validation = await validator.ValidateAsync(command);
                if (!validation.IsValid)
                    return Results.BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

                var result = await mediator.Send(command);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });


            group.MapPost("/change-password", [Authorize] async (ChangePasswordCommand command, ISender mediator, IValidator<ChangePasswordCommand> validator) =>
            {
                var validation = await validator.ValidateAsync(command);
                if (!validation.IsValid)
                    return Results.BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

                var result = await mediator.Send(command);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });

            group.MapPost("/toggle-active", [Authorize] async (ToggleUserStatusCommand command, ISender mediator, IValidator<ToggleUserStatusCommand> validator) =>
            {
                var validation = await validator.ValidateAsync(command);
                if (!validation.IsValid)
                    return Results.BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

                var result = await mediator.Send(command);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });

            // 🔹 Role Assignment
            group.MapPost("/assign-roles", [Authorize] async (AssignUserToRolesCommand command, ISender mediator, IValidator<AssignUserToRolesCommand> validator) =>
            {
                var validation = await validator.ValidateAsync(command);
                if (!validation.IsValid)
                    return Results.BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

                var result = await mediator.Send(command);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });


            // 🔹 Get User Roles (Role IDs)
            group.MapPost("/users-roles", [Authorize] async (GetUserRolesCommand command, ISender mediator) =>
            {
                var result = await mediator.Send(command);

                return result.Success
                    ? Results.Ok(result)
                    : Results.NotFound(result);
            })
             .WithName("GetUserRoles")
             .Produces<ResponseResultDTO<List<int>>>(StatusCodes.Status200OK)
             .Produces<ResponseResultDTO<List<int>>>(StatusCodes.Status404NotFound);


            group.MapPost("/logout",
                   async (IMediator mediator) =>
                   {
                       var result = await mediator.Send(new LogoutCommand());
                       return Results.Ok(new { message = "Logged out successfully" });
                   })
               .RequireAuthorization()
               .WithTags("Auth")
               .WithName("Logout")
               .WithSummary("Logout from current session")
               .Produces(StatusCodes.Status200OK);

            group.MapPost("/logout-all",
                async (IMediator mediator) =>
                {
                    var count = await mediator.Send(new LogoutAllSessionsCommand());
                    return Results.Ok(new
                    {
                        message = $"Logged out from {count} session(s)",
                        sessionsTerminated = count
                    });
                })
            .RequireAuthorization()
            .WithTags("Auth")
            .WithName("LogoutAllSessions")
            .WithSummary("Logout from all sessions (all devices)")
            .Produces(StatusCodes.Status200OK);
        }

    }

    #region Login

    public record LoginCommand(string UserName, string Password) : IRequest<LoginResponse>;

    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtService _jwtService;
        private readonly ICurrentUserService _currentUser;
        private readonly DBContextHRsystem _dbContextHRsystem;
        private readonly ISecurityCacheService _securityCacheService;

        public LoginHandler(SignInManager<ApplicationUser> signInManager,
                            UserManager<ApplicationUser> userManager,
                            JwtService jwtService, ICurrentUserService currentUserService
            , DBContextHRsystem dbContextHRsystem, ISecurityCacheService securityCacheService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
            _currentUser = currentUserService;
            _dbContextHRsystem = dbContextHRsystem;
            _securityCacheService = securityCacheService;
        }


        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, lockoutOnFailure: true);
            if (!result.Succeeded || result.IsLockedOut)    //  even if it is locked out we return invalid login to avoid user enumeration
                return new LoginResponse(false, ResponseMessages.InvalidLogin);

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null || !user.IsActive)
                return new LoginResponse(false, ResponseMessages.InvalidLogin);


            var clientType = string.IsNullOrEmpty(_currentUser.X_ClientType) ? "web" : _currentUser.X_ClientType;
                        var DeviceId = string.IsNullOrEmpty(_currentUser.DeviceId) ? "web" : _currentUser.DeviceId;




            var oldSession = await _dbContextHRsystem.TbUserSession
                            .Where(s =>
                                s.UserId == user.Id &&
                                s.ClientType == clientType &&
                                s.IsActive)
                            .FirstOrDefaultAsync();

            if (oldSession != null)
            {
                oldSession.IsActive = false;
                oldSession.LastSeenAt = DateTime.UtcNow;
            }

            // 🔥 3. CLEAR THE CACHE using the helper
            // This ensures that the NEXT request the user makes will trigger a fresh DB check
            _securityCacheService.ClearUserCache(user.Id, clientType);

            var jti = Guid.NewGuid().ToString();
            var jwtToken = await _jwtService.GenerateTokenAsync(user,jti );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);
           // var jti = jwtToken.Jti;

            string refreshToken = GenerateRefreshTokenHelperr.GenerateRefreshToken();

            string refreshTokenHash = GenerateRefreshTokenHelperr.HashRefreshToken(refreshToken);


            _dbContextHRsystem.TbUserSession.Add(new TbUserSession
            {
                UserId = user.Id,
                ClientType = clientType,
                DeviceId = DeviceId,
                Jti = jti,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                LastSeenAt = DateTime.UtcNow,
                RefreshTokenHash = refreshTokenHash,
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7) // e.g., 7 days validity

            });

            await _dbContextHRsystem.SaveChangesAsync();

            LoginResponse res = new LoginResponse(true,
                ResponseMessages.SucessLogin,
                tokenString, refreshToken, user.UserFullName,jti, DateTime.UtcNow.AddDays(7));

            return res;
                 
        }





    }

    public record LoginResponse(
    bool Success,
    string Message,
    string? Token = null,
    string? RefreshToken = null,
    string? UserName = null,
    string? Jti=null,
    DateTime? ExpiresAt = null
);


    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        }
    }

    #endregion

    #region Register User

    public record RegisterUserCommand(
        string UserName,
        string Password,
        string FullName,
        int CompanyId,
        int EmployeeId,
        List<int> RoleIds
    ) : IRequest<ResponseResultDTO>;

    public class RegisterUserHandler
        : IRequestHandler<RegisterUserCommand, ResponseResultDTO>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RegisterUserHandler(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ResponseResultDTO> Handle(
            RegisterUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                UserName = request.UserName,
                UserFullName = request.FullName,
                CompanyId = request.CompanyId,
                EmployeeId = request.EmployeeId,
                IsActive = true,
                Email = $"{request.UserName}@demo.com"
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);

            if (!createResult.Succeeded)
            {
                return new ResponseResultDTO
                {
                    Success = false,
                    Message = string.Join(", ", createResult.Errors.Select(e => e.Description))
                };
            }

            // 🔹 Remove any roles (safe even if none exist)
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                    return Failed(removeResult);
            }

            // 🔹 Convert RoleIds → RoleNames
            var roleNames = await _roleManager.Roles
                .Where(r => request.RoleIds.Contains(r.Id))
                .Select(r => r.Name!)
                .ToListAsync(cancellationToken);

            if (!roleNames.Any())
            {
                return new ResponseResultDTO
                {
                    Success = false,
                    Message = "No valid roles found"
                };
            }

            // 🔹 Add new roles
            var addRolesResult = await _userManager.AddToRolesAsync(user, roleNames);
            if (!addRolesResult.Succeeded)
                return Failed(addRolesResult);

            return new ResponseResultDTO
            {
                Success = true,
                Message = "User created and roles assigned successfully"
            };
        }

        private static ResponseResultDTO Failed(IdentityResult result)
            => new()
            {
                Success = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            };
    }



    public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.FullName).NotEmpty();
        }
    }

    #endregion


    #region Update User

    public class UpdateUserHandler
      : IRequestHandler<UpdateUserCommand, ResponseResultDTO>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UpdateUserHandler(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ResponseResultDTO> Handle(
            UpdateUserCommand request,
            CancellationToken cancellationToken)
        {
            // 1️⃣ Find user
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
                return new ResponseResultDTO { Success = false, Message = "User not found" };

            // 2️⃣ Update basic fields
            if (!string.IsNullOrWhiteSpace(request.UserName))
                user.UserName = request.UserName;

            if (!string.IsNullOrWhiteSpace(request.FullName))
                user.UserFullName = request.FullName;

            user.Email = $"{user.UserName}@demo.com";

            // 3️⃣ Update user
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return Failed(updateResult);

            // 4️⃣ Update password (optional)
            if (!string.IsNullOrWhiteSpace(request.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var pwResult = await _userManager.ResetPasswordAsync(
                    user, token, request.NewPassword);

                if (!pwResult.Succeeded)
                    return Failed(pwResult);
            }



            if (!request.RoleIds.Any())
            {
                return new ResponseResultDTO
                {
                    Success = false,
                    Message = "No valid roles found"
                };

            }


            // 5️⃣ Reset roles (same logic as Register)
            var currentRoles = await _userManager.GetRolesAsync(user);
            var newRoleNames = new List<string>();

            foreach (var roleId in request.RoleIds)
            {
                var role = await _roleManager.FindByIdAsync(roleId.ToString());
                if (role != null)
                    newRoleNames.Add(role.Name!);
            }


            var rolesToRemove = currentRoles.Except(newRoleNames).ToList();
            var rolesToAdd = newRoleNames.Except(currentRoles).ToList();

            if (rolesToRemove.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            }

            if (rolesToAdd.Any())
            {
                await _userManager.AddToRolesAsync(user, rolesToAdd);
            }



            return new ResponseResultDTO
            {
                Success = true,
                Message = "User updated successfully"
            };
        }

        private static ResponseResultDTO Failed(IdentityResult result)
            => new()
            {
                Success = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            };
    }

    public class UpdateUserCommand : IRequest<ResponseResultDTO>
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? NewPassword { get; set; } // optional
        public List<int> RoleIds { get; set; } = [];
    }



    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);

            RuleFor(x => x.UserName)
                .NotEmpty()
                .When(x => x.UserName != null);

            RuleFor(x => x.FullName)
                .NotEmpty()
                .When(x => x.FullName != null);
        }
    }


    #endregion



    #region Change Password

    public record ChangePasswordCommand(string UserName, string CurrentPassword, string NewPassword)
        : IRequest<ResponseResultDTO>;

    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, ResponseResultDTO>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ChangePasswordHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResponseResultDTO> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                return new ResponseResultDTO { Success = false, Message = "User Not Found" };

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (result.Succeeded)
                return new ResponseResultDTO { Success = true, Message = "Password changed successfully" };

            return new ResponseResultDTO
            {
                Success = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            };
        }
    }

    public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.CurrentPassword).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6);
        }
    }

    #endregion

    #region Toggle User Active/Inactive

    public record ToggleUserStatusCommand(string UserName, bool IsActive) : IRequest<ResponseResultDTO>;

    public class ToggleUserStatusHandler : IRequestHandler<ToggleUserStatusCommand, ResponseResultDTO>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ToggleUserStatusHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResponseResultDTO> Handle(ToggleUserStatusCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                return new ResponseResultDTO { Success = false, Message = "User Not Found" };

            user.IsActive = request.IsActive;
            await _userManager.UpdateAsync(user);

            return new ResponseResultDTO
            {
                Success = true,
                Message = $"User {(request.IsActive ? "activated" : "deactivated")} successfully"
            };
        }
    }

    public class ToggleUserStatusValidator : AbstractValidator<ToggleUserStatusCommand>
    {
        public ToggleUserStatusValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();
        }
    }

    #endregion

    #region Assign Roles to User

    public record AssignUserToRolesCommand(UserRolesDTO UserRoles) : IRequest<ResponseResultDTO>;

    public class AssignUserToRolesValidator : AbstractValidator<AssignUserToRolesCommand>
    {
        public AssignUserToRolesValidator()
        {
            RuleFor(x => x.UserRoles.UserId).NotEmpty().WithMessage("User ID is required.");
            RuleFor(x => x.UserRoles.RoleNames).NotEmpty().WithMessage("At least one role is required.");
        }
    }

    public class AssignUserToRolesHandler : IRequestHandler<AssignUserToRolesCommand, ResponseResultDTO>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AssignUserToRolesHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResponseResultDTO> Handle(AssignUserToRolesCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserRoles.UserId.ToString());
            if (user == null)
                return new ResponseResultDTO { Success = false, Message = "User not found." };

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                    return new ResponseResultDTO
                    {
                        Success = false,
                        Message = $"Failed to remove current roles: {string.Join(", ", removeResult.Errors.Select(e => e.Description))}"
                    };
            }

            var addResult = await _userManager.AddToRolesAsync(user, request.UserRoles.RoleNames);
            if (!addResult.Succeeded)
                return new ResponseResultDTO
                {
                    Success = false,
                    Message = $"Failed to add user to roles: {string.Join(", ", addResult.Errors.Select(e => e.Description))}"
                };

            return new ResponseResultDTO
            {
                Success = true,
                Message = $"User '{user.UserName}' assigned to roles successfully."
            };
        }
    }

    #endregion


    #region GetUserRoles
    public record GetUserRolesCommand(int UserId)
     : IRequest<ResponseResultDTO<List<int>>>;

    public class GetUserRolesHandler
        : IRequestHandler<GetUserRolesCommand, ResponseResultDTO<List<int>>>
    {
        private readonly DBContextHRsystem _context;

        public GetUserRolesHandler(DBContextHRsystem context)
        {
            _context = context;
        }

        public async Task<ResponseResultDTO<List<int>>> Handle(
            GetUserRolesCommand request,
            CancellationToken cancellationToken)
        {
            // 1️⃣ Ensure user exists (optional but recommended)
            var userExists = await _context.Users
                .AnyAsync(u => u.Id == request.UserId, cancellationToken);

            if (!userExists)
            {
                return new ResponseResultDTO<List<int>>
                {
                    Success = false,
                    Message = "User not found",
                    Data = []
                };
            }

            // 2️⃣ Get Role IDs directly from UserRoles table
            var roleIds = await _context.UserRoles
                .Where(ur => ur.UserId == request.UserId)
                .Select(ur => ur.RoleId)
                .ToListAsync(cancellationToken);

            return new ResponseResultDTO<List<int>>
            {
                Success = true,
                Message = "User roles loaded successfully",
                Data = roleIds
            };
        }
    }


    #endregion
    #region  LogOut




    public record LogoutCommand : IRequest<bool>;

    public class LogoutHandler : IRequestHandler<LogoutCommand, bool>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;
        private readonly IMemoryCache _cache;

        public LogoutHandler(
            DBContextHRsystem db,
            ICurrentUserService currentUser,
            IMemoryCache cache)
        {
            _db = db;
            _currentUser = currentUser;
            _cache = cache;
        }

        public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            var clientType = _currentUser.X_ClientType;
            var deviceId = _currentUser.DeviceId;

            // Find and invalidate the current session
            var session = await _db.TbUserSession
                .FirstOrDefaultAsync(s =>
                    s.UserId == userId &&
                    s.ClientType == clientType &&
                    s.DeviceId == deviceId &&
                    s.IsActive,
                    cancellationToken);

            if (session != null)
            {
                session.IsActive = false;
                await _db.SaveChangesAsync(cancellationToken);

                // Clear security cache
                string cacheKey = $"user_sec_{userId}_{clientType}";
                _cache.Remove(cacheKey);
            }

            return true;
        }
    }

    // ============================================
    // LOGOUT ALL SESSIONS (All devices)
    // ============================================

    public record LogoutAllSessionsCommand : IRequest<int>;

    public class LogoutAllSessionsHandler : IRequestHandler<LogoutAllSessionsCommand, int>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;
        private readonly IMemoryCache _cache;

        public LogoutAllSessionsHandler(
            DBContextHRsystem db,
            ICurrentUserService currentUser,
            IMemoryCache cache)
        {
            _db = db;
            _currentUser = currentUser;
            _cache = cache;
        }

        public async Task<int> Handle(LogoutAllSessionsCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            var sessions = await _db.TbUserSession
                .Where(s => s.UserId == userId && s.IsActive)
                .ToListAsync(cancellationToken);

            foreach (var session in sessions)
            {
                session.IsActive = false;

                // Clear cache for each client type
                string cacheKey = $"user_sec_{userId}_{session.ClientType}";
                _cache.Remove(cacheKey);
            }

            await _db.SaveChangesAsync(cancellationToken);

            return sessions.Count;
        }
    }

    // ============================================
    // ENDPOINTS
    // ============================================


    #endregion






}
