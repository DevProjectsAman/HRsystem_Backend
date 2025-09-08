using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.Entities;
using HRsystem.Api.Features.SystemAdmin.DTO;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace HRsystem.Api.Features.SystemAdmin.RolePermision
{
    public static class UserRolePermissions
    {
        public static void MapRoleAssignmentEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/roles/assign-permission", async (IMediator mediator, AssignPermissionToRoleCommand command) =>
                await mediator.Send(command))
                .WithName("AssignPermissionToRole")
                .WithTags("Role Management");

            app.MapPost("/api/roles/assign-user-roles", async (IMediator mediator, AssignUserToRolesCommand command) =>
                await mediator.Send(command))
                .WithName("AssignUserToRoles")
                .WithTags("Role Management");
        }
    }

    #region Add Permission to Role

    public record AssignPermissionToRoleCommand(int RoleId, int PermissionId) : IRequest<ResponseResultDTO>;

    public class AssignPermissionToRoleHandler : IRequestHandler<AssignPermissionToRoleCommand, ResponseResultDTO>
    {
        private readonly DBContextHRsystem _dbContext;
        private readonly ICurrentUserService _currentUser;

        public AssignPermissionToRoleHandler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _dbContext = db;
            _currentUser = currentUser;
        }

        public async Task<ResponseResultDTO> Handle(AssignPermissionToRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var rolePermission = new AspRolePermissions
                {
                    RoleId = request.RoleId,
                    PermissionId = request.PermissionId,
                     CreatedAt= DateTime.UtcNow,
                    CreatedBy = _currentUser.UserId
                     

                };

                await _dbContext.AspRolePermissions.AddAsync(rolePermission, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return new ResponseResultDTO
                {
                    Success = true,
                    Message = "Saved Successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResponseResultDTO
                {
                    Success = false,
                    Message = ex.InnerException.Message
                };
            }
        }
    }

    public class AssignPermissionToRoleValidator : AbstractValidator<AssignPermissionToRoleCommand>
    {
        public AssignPermissionToRoleValidator()
        {
            RuleFor(x => x.RoleId).NotEmpty().WithMessage("Role ID is required.");
            RuleFor(x => x.PermissionId).NotEmpty().WithMessage("Permission ID is required.");
        }
    }

    #endregion

    #region Add User to Roles

    public record AssignUserToRolesCommand(UserRolesDTO UserRoles) : IRequest<ResponseResultDTO>;

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
            {
                return new ResponseResultDTO
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeResult.Succeeded)
            {
                var errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
                return new ResponseResultDTO
                {
                    Success = false,
                    Message = $"Failed to remove current roles: {errors}"
                };
            }

            var addResult = await _userManager.AddToRolesAsync(user, request.UserRoles.RoleNames);

            if (!addResult.Succeeded)
            {
                var errors = string.Join(", ", addResult.Errors.Select(e => e.Description));
                return new ResponseResultDTO
                {
                    Success = false,
                    Message = $"Failed to add user to roles: {errors}"
                };
            }

            return new ResponseResultDTO
            {
                Success = true,
                Message = $"User {user.UserName} assigned to roles successfully"
            };
        }
    }

   

    #endregion

}

