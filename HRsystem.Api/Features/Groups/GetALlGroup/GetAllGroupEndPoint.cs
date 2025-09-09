using FluentValidation;
using HRsystem.Api.Features.Groups.GetGroupById;
using MediatR;

namespace HRsystem.Api.Features.Groups.GetALlGroup
{
public static class GetAllGroupEndPoint
{
    public static void MapGetAllGroup(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/groups", async (ISender mediator) =>
        {
            var result = await mediator.Send(new GetAllGroupCommand());

            return Results.Ok(new
            {
                Success = true,
                Data = result
            });
        })
        .WithName("GetAllGroup")
        .WithTags("Groups");
    }
}


}
