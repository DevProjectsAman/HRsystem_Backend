using HRsystem.Api.Features.WorkLocation.CreateWorkLocation;
using HRsystem.Api.Features.WorkLocation.GetAllWorkLocations;
using HRsystem.Api.Features.WorkLocation.GetWorkLocationById;
using HRsystem.Api.Features.WorkLocation.UpdateWorkLocation;
using HRsystem.Api.Features.WorkLocation.DeleteWorkLocation;
using MediatR;

namespace HRsystem.Api.Features.WorkLocation
{
    public static class WorkLocationEndpoints
    {
        public static void MapWorkLocationEndpoints(this IEndpointRouteBuilder app)
        {
            // Get all
            app.MapGet("/api/work-locations", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllWorkLocationsQuery());
                return Results.Ok(new { Success = true, Data = result });
            });

            // Get by Id
            app.MapGet("/api/work-locations/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetWorkLocationByIdQuery(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"WorkLocation {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Create
            app.MapPost("/api/work-locations", async (CreateWorkLocationCommand cmd, ISender mediator) =>
            {
                var result = await mediator.Send(cmd);
                return Results.Created($"/api/work-locations/{result.WorkLocationId}", new { Success = true, Data = result });
            });

            // Update
            app.MapPut("/api/work-locations/{id}", async (int id, UpdateWorkLocationCommand cmd, ISender mediator) =>
            {
                if (id != cmd.WorkLocationId)
                    return Results.BadRequest(new { Success = false, Message = "Id mismatch" });

                var result = await mediator.Send(cmd);
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"WorkLocation {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Delete
            app.MapDelete("/api/work-locations/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteWorkLocationCommand(id));
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"WorkLocation {id} not found" })
                    : Results.Ok(new { Success = true, Message = $"WorkLocation {id} deleted successfully" });
            });
        }
    }
}
