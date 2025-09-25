using HRsystem.Api.Features.ActivityStatus.DeleteActivityStatus;
using HRsystem.Api.Features.ActivityStatus.GetActivityStatusById;
using HRsystem.Api.Features.ActivityStatus.GetAllActivityStatuses;
using MediatR;
using FluentValidation;
using HRsystem.Api.Features.Lookups.ActivityStatus.CreateActivityStatus;
using HRsystem.Api.Features.Lookups.ActivityStatus.UpdateActivityStatus;

namespace HRsystem.Api.Features.Lookups.ActivityStatus
{
    public static class ActivityStatusEndpoints
    {
        public static void MapActivityStatusEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Lookups/activity-statuses").WithTags("Activity Status");

            // Get All
            group.MapGet("/ListOfActivityStatuses", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllActivityStatusesQuery());
                return Results.Ok(new { Success = true, Data = result });
            });

            // Get by Id
            group.MapGet("/GetOneActivityStatus/{id}", async (int id, ISender mediator) =>
            {
                if (id <= 0)
                    return Results.BadRequest(new { Success = false, Message = "Invalid StatusId" });

                var result = await mediator.Send(new GetActivityStatusByIdQuery(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Status {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Create
            group.MapPost("/CreateActivityStatus", async (CreateActivityStatusCommand cmd, ISender mediator, IValidator<CreateActivityStatusCommand> validator) =>
            {
                var validationResult = await validator.ValidateAsync(cmd);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new { Success = false, Errors = validationResult.Errors.Select(e => e.ErrorMessage) });

                var result = await mediator.Send(cmd);
                return Results.Created($"/api/activity-statuses/{result.StatusId}", new { Success = true, Data = result });
            });

            // Update
            group.MapPut("/UpdateActivityStatus/{id}", async (int id, UpdateActivityStatusCommand cmd, ISender mediator, IValidator<UpdateActivityStatusCommand> validator) =>
            {
                if (id != cmd.StatusId)
                    return Results.BadRequest(new { Success = false, Message = "Id mismatch" });

                var validationResult = await validator.ValidateAsync(cmd);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new { Success = false, Errors = validationResult.Errors.Select(e => e.ErrorMessage) });

                var result = await mediator.Send(cmd);
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Status {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Delete
            group.MapDelete("/DeleteActivityStatus/{id}", async (int id, ISender mediator) =>
            {
                if (id <= 0)
                    return Results.BadRequest(new { Success = false, Message = "Invalid StatusId" });

                var result = await mediator.Send(new DeleteActivityStatusCommand(id));
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"Status {id} not found" })
                    : Results.Ok(new { Success = true, Message = $"Status {id} deleted successfully" });
            });
        }
    }
}
