using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRsystem.Api.Features.Groups.GetALL
{


    public static class GetGroupByIdEndPoint
    {
        public static void MapGetGroup(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/groups/{id}", async (
                int id,
                ISender mediator,
                IValidator<GetGroupByIdCommand> validator) =>
            {
                var command = new GetGroupByIdCommand(id);

                // ✅ Validate manually
                var validationResult = await validator.ValidateAsync(command);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors
                        .Select(e => new { e.PropertyName, e.ErrorMessage });
                    return Results.BadRequest(errors);
                }

                // ✅ Send to handler
                var result = await mediator.Send(command);

                return Results.Ok(result);
            })
            .WithName("GetGroup")
            .WithTags("Groups");
        }
    }

}
