using MediatR;

namespace HRsystem.Api.Features.GetPendingActivities
{
    public static class GetPendingActivitiesEndPoints
    {
        public static void MapActivityEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/activities").WithTags("Activities");

            // Get pending activities for current user
            group.MapGet("/pending", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetPendingActivitiesQuery());
                if (result == null || !result.Any())
                    return Results.NotFound(new { Success = false, Message = "No pending activities found" });

                return Results.Ok(new { Success = true, Data = result });
            });
        }
    }
}
