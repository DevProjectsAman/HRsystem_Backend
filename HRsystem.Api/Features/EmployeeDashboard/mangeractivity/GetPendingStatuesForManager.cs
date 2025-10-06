using HRsystem.Api.Features.EmployeeDashboard.ManagerActivity;
using MediatR;

namespace HRsystem.Api.Features.EmployeeDashboard.mangeractivity
{
    public static class GetPendingStatuesForManager
    {
        public static void MapPendingStatuesForManager(this IEndpointRouteBuilder app)
        {
           // var group = app.MapGroup("/api/activities").WithTags("Activities");
            var group = app.MapGroup("/api/employee-dashboard").WithTags("Employee Dashboard");

            // Get subordinates with pending activities
            group.MapGet("/subordinates/pending", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetSubordinatesPendingActivitiesQuery());
                if (result == null || !result.Any())
                    return Results.NotFound(new { Success = false, Message = "No subordinates or pending activities found" });

                return Results.Ok(new { Success = true, Data = result });
            });

            group.MapGet("/subordinates/Numberofpending", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetNumberOfPendingReqForManager());
                if (result == null)
                    return Results.NotFound(new { Success = false, Message = "No subordinates or pending activities found" });

                return Results.Ok(new { Success = true, Data = result });
            });


            group.MapGet("/subordinates/IsManager", async (ISender mediator) =>
            {
                var result = await mediator.Send(new CheckManagerQuery());
                if (result == null)
                    return Results.NotFound(new { Success = false, Message = "Employee Isn't A Manager" });

                return Results.Ok(new { Success = true, Data = result });
            });

        }
    }
}
