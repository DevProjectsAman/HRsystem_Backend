
using System.Security.Claims;
using System.Text.Json;

namespace HRsystem.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool HasPermission(this ClaimsPrincipal user, string permission)
        {
            var permissionsClaim = user.FindFirst("permissions")?.Value;

            if (string.IsNullOrEmpty(permissionsClaim))
                return false;

            // Check for superuser permission
            if (permissionsClaim == "admin.all" || permissionsClaim.Contains("admin.all"))
                return true;

            // Check specific permissions
            return permissionsClaim.Split(',').Contains(permission);
        }

        public static bool HasPermissions(this ClaimsPrincipal user, params string[] requiredPermissions)
        {
            var permissionsClaim = user.FindFirst("permissions")?.Value;

            if (string.IsNullOrEmpty(permissionsClaim))
                return false;

            // Superuser check
            if (permissionsClaim == "admin.all" || permissionsClaim.Contains("admin.all"))
                return true;

            var permissions = permissionsClaim.Split(',');
            return requiredPermissions.All(p => permissions.Contains(p));
        }

        public static bool HasAnyPermission(this ClaimsPrincipal user, params string[] permissions)
        {
            var permissionsClaim = user.FindFirst("permissions")?.Value;

            if (string.IsNullOrEmpty(permissionsClaim))
                return false;

            // Superuser check
            if (permissionsClaim == "admin.all" || permissionsClaim.Contains("admin.all"))
                return true;

            var userPermissions = permissionsClaim.Split(',');
            return permissions.Any(p => userPermissions.Contains(p));
        }

        public static List<string> GetPermissions(this ClaimsPrincipal user)
        {
            var permissionsClaim = user.FindFirst("permissions")?.Value;

            if (string.IsNullOrEmpty(permissionsClaim))
                return new List<string>();

            return permissionsClaim.Split(',').ToList();
        }

        // Optional: Check if user is superuser
        public static bool IsSuperUser(this ClaimsPrincipal user)
        {
            var permissionsClaim = user.FindFirst("permissions")?.Value;
            return permissionsClaim == "admin.all" || permissionsClaim?.Contains("admin.all") == true;
        }


    }
}
