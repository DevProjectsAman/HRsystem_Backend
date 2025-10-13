using HRsystem.Api.Features.EmployeeDashboard.GetApprovedActivites;
using HRsystem.Api.Features.EmployeeDashboard.GetRejectedActivities;
using MediatR;

namespace HRsystem.Api.Features.EmployeeDashboard.GetPendingActivities
{
    public static class GetPendingActivitiesEndPoints
    {
        public static void MapPendingActivitiesEndPoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/employee-dashboard").WithTags("Employee Dashboard");

            // Get pending activities for current user
            group.MapGet("/pending", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetPendingActivitiesQuery());
                if (result == null || !result.Any())
                    return Results.NotFound(new { Success = false, Message = "No pending activities found" });

                return Results.Ok(new { Success = true, Data = result });
            });

            group.MapGet("/NubmerOfpending", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetActivitiesStatusCountQuery());
                if (result == null )
                    return Results.NotFound(new { Success = false, Message = "No pending activities found" });

                return Results.Ok(new { Success = true, Data = result });
            });

            group.MapGet("/Approved", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetApprovedActivitiesQueury());
                if (result == null || !result.Any())
                    return Results.NotFound(new { Success = false, Message = "No Approved activities found" });

                return Results.Ok(new { Success = true, Data = result });
            });

            group.MapGet("/R", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetRejectedActivitiesQueury());
                if (result == null || !result.Any())
                    return Results.NotFound(new { Success = false, Message = "No Rejected activities found" });

                return Results.Ok(new { Success = true, Data = result });
            });


        }
    }
}
