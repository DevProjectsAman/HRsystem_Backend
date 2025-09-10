using FluentValidation;
using HRsystem.Api.Database.Entities;
using HRsystem.Api.Features.SystemAdmin.DTO;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace HRsystem.Api.Features.SystemAdmin.UserRoles
{
    public static class UserRolePermissionsEndpoints
    {
        public static void MapRoleAssignmentEndpoints(this IEndpointRouteBuilder app)
        {


            app.MapPost("/api/systemadmin/UserRoles", async (IMediator mediator, AssignUserToRolesCommand command) =>
                await mediator.Send(command))
                .WithName("AssignUserToRoles")
                .WithTags("User Role Permission Management");
        }
    }


    #region Assign User to Roles

    public record AssignUserToRolesCommand(UserRolesDTO UserRoles)
        : IRequest<ResponseResultDTO>;

    public class AssignUserToRolesValidator : AbstractValidator<AssignUserToRolesCommand>
    {
        public AssignUserToRolesValidator()
        {
            RuleFor(x => x.UserRoles.UserId)
                .NotEmpty().WithMessage("User ID is required.");
            RuleFor(x => x.UserRoles.RoleNames)
                .NotEmpty().WithMessage("At least one role is required.");
        }
    }

    public class AssignUserToRolesHandler
        : IRequestHandler<AssignUserToRolesCommand, ResponseResultDTO>
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
                    Message = "User not found."
                };
            }

            // Remove old roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
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
            }

            // Add new roles
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
                Message = $"User '{user.UserName}' assigned to roles successfully."
            };
        }
    }

    #endregion
}
