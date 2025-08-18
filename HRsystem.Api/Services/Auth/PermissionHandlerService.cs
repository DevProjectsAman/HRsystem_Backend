using HRsystem.Api.Database;
using HRsystem.Api.Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace HRsystem.Api.Services.Auth;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string[]? AllowedRoles { get; }
    public string? Permission { get; }

    public PermissionRequirement(string? role, string? permission)
    {
        AllowedRoles = role?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        Permission = permission;
    }
}



public class PermissionHandlerService : AuthorizationHandler<PermissionRequirement>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly HRsystemDbContext _dbContext;

    public PermissionHandlerService(UserManager<ApplicationUser> userManager, HRsystemDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var userIdStr = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            return;

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return;

        var userRoles = await _userManager.GetRolesAsync(user);

        // ✅ Case 1: Only Roles required (any one matches)
        if (requirement.AllowedRoles?.Any() == true && string.IsNullOrEmpty(requirement.Permission))
        {
            if (userRoles.Any(r => requirement.AllowedRoles.Contains(r)))
                context.Succeed(requirement);
            return;
        }

        // ✅ Case 2: Only Permission required (any role has it)
        if ((requirement.AllowedRoles?.Any() != true) && !string.IsNullOrWhiteSpace(requirement.Permission))
        {
            foreach (var userRole in userRoles)
            {
                var hasPermission = await _dbContext.AspRolePermissions.AnyAsync(rp =>
                    rp.Role.Name == userRole &&
                    rp.Permission.PermissionName == requirement.Permission);

                if (hasPermission)
                {
                    context.Succeed(requirement);
                    return;
                }
            }
        }

        // ✅ Case 3: Roles + Permission required (any role in list + has permission)
        if (requirement.AllowedRoles?.Any() == true && !string.IsNullOrWhiteSpace(requirement.Permission))
        {
            foreach (var allowedRole in requirement.AllowedRoles)
            {
                if (userRoles.Contains(allowedRole))
                {
                    var hasPermission = await _dbContext.AspRolePermissions.AnyAsync(rp =>
                        rp.Role.Name == allowedRole &&
                        rp.Permission.PermissionName == requirement.Permission);

                    if (hasPermission)
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }
            }
        }
    }
}
