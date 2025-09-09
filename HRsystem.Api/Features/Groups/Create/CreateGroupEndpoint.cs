using System.ComponentModel.DataAnnotations;
using FluentValidation;
using HRsystem.Api.Features.Groups.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRsystem.Api.Features.Groups.Create
{
    public static class CreateGroupEndpoint
    {
        public static void MapCreateGroupEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/groups", async (
             [FromBody] CreateGroupRequest dto,
             ISender mediator,
             IValidator<CreateGroupCommand> validator) => // 👈 أضفنا الـ validator هنا
            {
                var command = new CreateGroupCommand(dto.GroupName);

                var validationResult = await validator.ValidateAsync(command);

                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors
                        .Select(e => new { e.PropertyName, e.ErrorMessage });
                    return Results.BadRequest(errors);
                }

                var groupId = await mediator.Send(command);
                return Results.Ok(new { Id = groupId, Message = "Group created successfully" });
            }).WithName("CreateGroup")
            .WithTags("Groups"); ;
        }
    }
}

