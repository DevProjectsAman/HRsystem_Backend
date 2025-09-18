using HRsystem.Api.Features.EmployeeActivityDt.EmployeePunch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRsystem.Api.Features.EmployeeAttendance
{
    public static class EmployeePunchEndpoints
    {
        public static void MapEmployeePunchEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/employee/punch").WithTags("Employee Punch");

            // Punch In
            group.MapPost("/in", async (PunchInCommand cmd, ISender mediator) =>
            {
                var result = await mediator.Send(cmd);
                return Results.Ok(new { Success = true, Data = result });
            });

            // Punch Out
            group.MapPost("/out", async (PunchOutCommand cmd, ISender mediator) =>
            {
                var result = await mediator.Send(cmd);
                return Results.Ok(new { Success = true, Data = result });
            });
        }
    }
}
