using Google;
using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Services.DeviceEnforcement
{
    public class DeviceEnforcementMiddleware
    {
        private readonly RequestDelegate _next;

        public DeviceEnforcementMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            ICurrentUserService currentUser,
            DBContextHRsystem db)
        {


            if (context.Request.Path.StartsWithSegments("/api/AccessManagement/Login"))
            {
                await _next(context);
                return;
            }


            // Skip for unauthenticated users (login endpoint)
            //if (!currentUser.IsAuthenticated || currentUser.EmployeeID == null)
            //{
            //    await _next(context);
            //    return;
            //}

            var clientType = context.Request.Headers["X-ClientType"]
        .FirstOrDefault()?.ToLower();

            // Web client → bypass device enforcement
            if (clientType == "web")
            {
                await _next(context);
                return;
            }

            //  Must Be mobile client
            if (clientType != "mobile")
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Invalid client type");
                return;
            }
            // Read device header
            var deviceId = context.Request.Headers["X-Device-Id"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(deviceId))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Device ID is required");
                return;
            }

            // Load employee
            var employee = await db.TbEmployees
                .Where(e => e.EmployeeId == currentUser.EmployeeID)
                .Select(e => new { e.SerialMobile })
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            // First login → register device
            if (string.IsNullOrEmpty(employee.SerialMobile))
            {
                var emp = await db.TbEmployees.FindAsync(currentUser.EmployeeID);
                emp.SerialMobile = deviceId;
                await db.SaveChangesAsync();

                await _next(context);
                return;
            }

            // Device mismatch → block
            if (employee.SerialMobile != deviceId)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized device");
                return;
            }

            // Device OK → continue
            await _next(context);
        }
    }

}
