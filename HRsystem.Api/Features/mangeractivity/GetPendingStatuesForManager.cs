using MediatR;

namespace HRsystem.Api.Features.mangeractivity
{
    public static class GetPendingStatuesForManager
    {
        public static void MapPendingStatuesForManager(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/activities").WithTags("Activities");

            // Get subordinates with pending activities
            group.MapGet("/subordinates/pending", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetSubordinatesPendingActivitiesQuery());
                if (result == null || !result.Any())
                    return Results.NotFound(new { Success = false, Message = "No subordinates or pending activities found" });

                return Results.Ok(new { Success = true, Data = result });
            });
        }
    }
}
