
using FluentValidation;
using HRsystem.Api.Features.SystemAdmin.Permissions;
using HRsystem.Api.Features.SystemAdmin.RolePermision;
using HRsystem.Api.Features.SystemAdmin.Roles;

namespace HRsystem.Api.Features.SystemAdmin;

public class AddRoleCommandValidator : AbstractValidator<AddRoleCommand>
{
    public AddRoleCommandValidator()
    {
        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("Role name is required.")
            .MinimumLength(3).WithMessage("Role name must be at least 3 characters.")
            .MaximumLength(100).WithMessage("Role name must not exceed 100 characters.");
    }
}

public class RoleExistsQueryValidator : AbstractValidator<RoleExistsQuery>
{
    public RoleExistsQueryValidator()
    {
        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("Role name is required.");
    }
}


#region PermissionValidator

public class AddPermissionCommandValidator : AbstractValidator<AddPermissionCommand>
{
    public AddPermissionCommandValidator()
    {
        RuleFor(x => x.Permission.PermissionName)
            .NotEmpty().WithMessage("Permission name is required");

        RuleFor(x => x.Permission.PermissionCatagory)
            .NotEmpty().WithMessage("Permission category is required");
    }
}

#endregion

public class AssignUserToRolesValidator : AbstractValidator<AssignUserToRolesCommand>
{
    public AssignUserToRolesValidator()
    {
        RuleFor(x => x.UserRoles.UserId).NotEmpty().WithMessage("User ID is required.");
        RuleFor(x => x.UserRoles.RoleNames)
            .NotNull().WithMessage("Roles are required.")
            .Must(list => list.Any()).WithMessage("At least one role must be assigned.");
    }
}