using FluentValidation;
using HRsystem.Api.Features.Organization.Groups.Create;
using HRsystem.Api.Features.Organization.Groups.DeleteGroup;
using HRsystem.Api.Features.Organization.Groups.GetALlGroup;
using HRsystem.Api.Features.Organization.Groups.GetGroupById;
using HRsystem.Api.Features.Organization.Groups.UpdateGroup;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRsystem.Api.Features.Organization.Groups
{
    public static class GroupsEndPoint
    {
        public static void MapGroupsEndpoint(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Organization/groups").WithTags("Groups");

            // GET All Groups
            group.MapGet("/GetAllGroup", [Authorize] async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllGroupCommand());
                return Results.Ok(new { Success = true, Data = result });
            }).WithName("GetAllGroup");

            // GET Group by ID
            group.MapGet("/GetOneGroup/{id}", [Authorize] async (
                int id,
                ISender mediator,
                IValidator<GetGroupByIdCommand> validator) =>
            {
                var command = new GetGroupByIdCommand(id);
                var validationResult = await validator.ValidateAsync(command);

                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                    return Results.BadRequest(new
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = errors
                    });
                }

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
            }).WithName("GetGroup");

            // POST Create Group
            group.MapPost("/CreateGroup", [Authorize] async (
            [FromBody] CreateGroupRequest dto,
            ISender mediator,
            IValidator<CreateGroupCommand> validator) =>
            {
                var command = new CreateGroupCommand(dto.GroupName);
                var validationResult = await validator.ValidateAsync(command);

                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                    return Results.BadRequest(errors);
                }

                var groupId = await mediator.Send(command);
                return Results.Ok(new { Id = groupId, Message = "Group created successfully" });
            }).WithName("CreateGroup");

            // PUT Update Group
            group.MapPut("/UpdateGroup/{id}", [Authorize] async (
                int id,
                UpdateGroupDto body,
                ISender mediator) =>
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
            }).WithName("UpdateGroup");

            // DELETE Group
            group.MapDelete("/DeleteGroup/{id}", [Authorize] async (int id, ISender mediator) =>
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
            }).WithName("DeleteGroup");
        }
    }
}