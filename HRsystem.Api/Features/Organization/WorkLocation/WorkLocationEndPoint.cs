using FluentValidation;
using HRsystem.Api.Features.Organization.WorkLocation.CreateWorkLocation;
using HRsystem.Api.Features.Organization.WorkLocation.DeleteWorkLocation;
using HRsystem.Api.Features.Organization.WorkLocation.GetAllWorkLocations;
using HRsystem.Api.Features.Organization.WorkLocation.GetWorkLocationById;
using HRsystem.Api.Features.Organization.WorkLocation.UpdateWorkLocation;
using MediatR;

namespace HRsystem.Api.Features.Organization.WorkLocation
{
    public static class WorkLocationEndpoints
    {
        public static void MapWorkLocationEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Organization/WorkLocation").WithTags("WorkLocations");

            // Get all
            group.MapGet("/List", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllWorkLocationsQuery());
                return Results.Ok(new { Success = true, Data = result });
            });

            // Get by Id
            group.MapGet("/GetOne", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetWorkLocationByIdQuery(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"WorkLocation {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Create
            group.MapPost("/Create", async (CreateWorkLocationCommand cmd, ISender mediator, IValidator<CreateWorkLocationCommand> validator) =>
            {
                var validation = await validator.ValidateAsync(cmd);
                if (!validation.IsValid)
                    return Results.BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

                var result = await mediator.Send(cmd);
                return Results.Created($"/api/work-locations/{result.WorkLocationId}", new { Success = true, Data = result });
            });

            // Update
            group.MapPut("/Update", async (int id, UpdateWorkLocationCommand cmd, ISender mediator, IValidator<UpdateWorkLocationCommand> validator) =>
            {
                if (id != cmd.WorkLocationId)
                    return Results.BadRequest(new { Success = false, Message = "Id mismatch" });

                var validation = await validator.ValidateAsync(cmd);
                if (!validation.IsValid)
                    return Results.BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

                var result = await mediator.Send(cmd);
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"WorkLocation {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Delete
            group.MapDelete("/Delete", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteWorkLocationCommand(id));
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"WorkLocation {id} not found" })
                    : Results.Ok(new { Success = true, Message = $"WorkLocation {id} deleted successfully" });
            });
        }
    }
}
