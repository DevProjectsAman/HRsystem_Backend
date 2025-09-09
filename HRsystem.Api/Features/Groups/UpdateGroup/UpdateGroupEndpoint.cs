using MediatR;
using System.Text.RegularExpressions;

namespace HRsystem.Api.Features.Groups.UpdateGroup
{
    public static class UpdateGroupEndpoint
    {
        public static void MapUpdateGroup(this IEndpointRouteBuilder app)
        {
            app.MapPut("/api/update_groups/{id}", async (int id, UpdateGroupDto body, ISender mediator) =>
            {
                var command = new UpdateGroupCommand(id, body.NewGroupName);

                var result = await mediator.Send(command);

                if (result == null)
                {
                    return Results.NotFound(new
                    {
                        Success = false,
                        Message = $"Group with ID {id} not found"
                    });
                }

                return Results.Ok(new
                {
                    Success = true,
                    Data = result
                });
            })
            .WithName("UpdateGroup")
            .WithTags("Groups");
        }
    }

}
