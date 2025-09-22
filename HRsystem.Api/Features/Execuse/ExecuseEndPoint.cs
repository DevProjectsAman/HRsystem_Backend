using HRsystem.Api.Features.Excuse.CreateExcuse;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace HRsystem.Api.Features.Excuse
{
    public static class ExcuseEndPoint
    {
        public static void MapExcuseEndPoint(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/excuseRequest").WithTags("requests");

            group.MapPost("/CreateExcuse", [Authorize] async (CreateExcuseCommand command, ISender mediator) =>
            {
                var result = await mediator.Send(command);

                return Results.Ok(new
                {
                    Success = true,
                    Message = "Excuse requested successfully",
                    Data = result
                });
            });
        }
    }
}