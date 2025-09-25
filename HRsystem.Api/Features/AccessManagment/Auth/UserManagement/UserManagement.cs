using FluentValidation;
using HRsystem.Api.Database.Entities;
using HRsystem.Api.Features.SystemAdmin.DTO;
using HRsystem.Api.Services;
using HRsystem.Api.Shared;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace HRsystem.Api.Features.AccessManagment.Auth.UserManagement
{
    public static class UserManagementEndpoints
    {
        public static void MapUserManagementEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/usermanagement").WithTags("_UserManagement");

            // 🔹 Auth & User Management
            group.MapPost("/login", async (LoginCommand command, ISender mediator, IValidator<LoginCommand> validator) =>
            {
                var validation = await validator.ValidateAsync(command);
                if (!validation.IsValid)
                    return Results.BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

                var result = await mediator.Send(command);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });

            group.MapPost("/register", async (RegisterUserCommand command, ISender mediator, IValidator<RegisterUserCommand> validator) =>
            {
                var validation = await validator.ValidateAsync(command);
                if (!validation.IsValid)
                    return Results.BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

                var result = await mediator.Send(command);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });

            group.MapPost("/change-password", async (ChangePasswordCommand command, ISender mediator, IValidator<ChangePasswordCommand> validator) =>
            {
                var validation = await validator.ValidateAsync(command);
                if (!validation.IsValid)
                    return Results.BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

                var result = await mediator.Send(command);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });

            group.MapPost("/toggle-active", async (ToggleUserStatusCommand command, ISender mediator, IValidator<ToggleUserStatusCommand> validator) =>
            {
                var validation = await validator.ValidateAsync(command);
                if (!validation.IsValid)
                    return Results.BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

                var result = await mediator.Send(command);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });

            // 🔹 Role Assignment
            group.MapPost("/assign-roles", async (AssignUserToRolesCommand command, ISender mediator, IValidator<AssignUserToRolesCommand> validator) =>
            {
                var validation = await validator.ValidateAsync(command);
                if (!validation.IsValid)
                    return Results.BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

                var result = await mediator.Send(command);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            });
        }
    }

    #region Login

    public record LoginCommand(string UserName, string Password) : IRequest<LoginResponse>;

    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtService _jwtService;

        public LoginHandler(SignInManager<ApplicationUser> signInManager,
                            UserManager<ApplicationUser> userManager,
                            JwtService jwtService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);
            if (!result.Succeeded)
                return new LoginResponse(false, ResponseMessages.InvalidLogin);

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null || !user.IsActive)
                return new LoginResponse(false, ResponseMessages.InvalidLogin);

            var roles = await _userManager.GetRolesAsync(user);
            var jwtToken = await _jwtService.GenerateTokenAsync(user, roles);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return new LoginResponse(true, ResponseMessages.SucessLogin, tokenString, user.UserName);
        }
    }

    public record LoginResponse(bool Success, string Message, string? Token = null, string? UserName = null);

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

    public record RegisterUserCommand(string UserName, string Password, string FullName, int CompanyId, int EmployeeId)
        : IRequest<ResponseResultDTO>;

    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, ResponseResultDTO>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterUserHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResponseResultDTO> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
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

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
                return new ResponseResultDTO { Success = true, Message = "User created successfully" };

            return new ResponseResultDTO
            {
                Success = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            };
        }
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
}
