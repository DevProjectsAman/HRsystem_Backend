using FluentValidation;
using HRsystem.Api.Features.Groups.GetGroupById;
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

                    return Results.BadRequest(new
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = errors
                    });
                }

                // ✅ Send to handler
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
            .WithName("GetGroup")
            .WithTags("Groups");
        }
    }

}
