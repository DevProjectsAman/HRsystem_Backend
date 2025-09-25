using MediatR;

namespace HRsystem.Api.Features.Groups.DeleteGroup
{
    public static class DeleteGroupEndPoint
    {
        public static void MapDeleteGroup(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/delete_groups/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteGroupCommand(id));

                if (!result)
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
                    Message = $"Group with ID {id} deleted successfully"
                });
            })
            .WithName("DeleteGroup")
            .WithTags("Groups");
        }
    }
}
