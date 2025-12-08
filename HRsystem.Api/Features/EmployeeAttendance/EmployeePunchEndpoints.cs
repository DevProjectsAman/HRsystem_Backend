using HRsystem.Api.Features.EmployeeActivityDt.EmployeePunch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRsystem.Api.Features.EmployeeAttendance
{
    public static class EmployeePunchEndpoints
    {
        public static void MapEmployeePunchEndpoints(this IEndpointRouteBuilder app)
        {

            var group = app.MapGroup("/api/employee-activities/Attendance")
                           .WithTags("Employee Activities");

            // Punch In
            group.MapPost("/in", async (PunchInCommand cmd, ISender mediator) =>
            {
                try
                {
                    var result = await mediator.Send(cmd);

                    return Results.Ok(new { Success = true, Data = result });
                }
                catch (Exception ex)
                {
                    return Results.Ok(new
                    {
                        success = false,
                        message = ex.Message,
                        data = (object)null
                    });
                }
            });

            // Punch Out
            group.MapPost("/out", async (PunchOutCommand cmd, ISender mediator) =>
            {
                try
                {
                    var result = await mediator.Send(cmd);

                    return Results.Ok(new { Success = true, Data = result });
                }
                catch (Exception ex)
                {
                    return Results.Ok(new
                    {
                        success = false,
                        message = ex.Message,
                        data = (object)null
                    });
                }
            });

        }
    }
}
