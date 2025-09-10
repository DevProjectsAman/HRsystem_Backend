using HRsystem.Api.Features.ActivityType.CreateActivityType;
using HRsystem.Api.Features.ActivityType.UpdateActivityType;
using HRsystem.Api.Features.ActivityType.GetAllActivityTypes;
using HRsystem.Api.Features.ActivityType.GetActivityTypeById;
using HRsystem.Api.Features.ActivityType.DeleteActivityType;
using MediatR;
using FluentValidation;

namespace HRsystem.Api.Features.ActivityType
{
    public static class ActivityTypeEndpoints
    {
        public static void MapActivityTypeEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/activity-types").WithTags("Activity Types");

            // Get All
            group.MapGet("/ListOfActivityTypes", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllActivityTypesQuery());
                return Results.Ok(new { Success = true, Data = result });
            });

            // Get by Id
            group.MapGet("/GetOneActivityType/{id}", async (int id, ISender mediator) =>
            {
                if (id <= 0)
                    return Results.BadRequest(new { Success = false, Message = "Invalid ActivityTypeId" });

                var result = await mediator.Send(new GetActivityTypeByIdQuery(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Activity Type {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Create
            group.MapPost("/CreateActivityType", async (CreateActivityTypeCommand cmd, ISender mediator, IValidator<CreateActivityTypeCommand> validator) =>
            {
                var validationResult = await validator.ValidateAsync(cmd);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new { Success = false, Errors = validationResult.Errors.Select(e => e.ErrorMessage) });

                var result = await mediator.Send(cmd);
                return Results.Created($"/api/activity-types/{result.ActivityTypeId}", new { Success = true, Data = result });
            });

            // Update
            group.MapPut("/UpdateActivityType/{id}", async (int id, UpdateActivityTypeCommand cmd, ISender mediator, IValidator<UpdateActivityTypeCommand> validator) =>
            {
                if (id != cmd.ActivityTypeId)
                    return Results.BadRequest(new { Success = false, Message = "Id mismatch" });

                var validationResult = await validator.ValidateAsync(cmd);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new { Success = false, Errors = validationResult.Errors.Select(e => e.ErrorMessage) });

                var result = await mediator.Send(cmd);
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Activity Type {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Delete
            group.MapDelete("/DeleteActivityType/{id}", async (int id, ISender mediator) =>
            {
                if (id <= 0)
                    return Results.BadRequest(new { Success = false, Message = "Invalid ActivityTypeId" });

                var result = await mediator.Send(new DeleteActivityTypeCommand(id));
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"Activity Type {id} not found" })
                    : Results.Ok(new { Success = true, Message = $"Activity Type {id} deleted successfully" });
            });
        }
    }
}
