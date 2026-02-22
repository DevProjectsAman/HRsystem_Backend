using HRsystem.Api.Features.Lookups.ActivityType.GetAllActivityTypes;
using HRsystem.Api.Features.MyTeam.Get;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace HRsystem.Api.Features.MyTeam
{
    public static class MyTeamEndpoints
    {
        public static void MapMyTeamEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/MyTeam/").WithTags("Activity Types");

            // Get All
            group.MapGet("/GetMyTeamTree", [Authorize] async (ISender mediator) =>
            {

                var result = await mediator.Send(
                    new GetMyTeamTreeQuery());

                return Results.Ok(new { Success = true, Data = result });

            });

        }
    }
}
