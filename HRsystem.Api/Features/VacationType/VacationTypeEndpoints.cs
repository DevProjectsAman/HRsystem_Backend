using HRsystem.Api.Features.VacationType.CreateVacationType;
using HRsystem.Api.Features.VacationType.GetAllVacationTypes;
using HRsystem.Api.Features.VacationType.GetVacationTypeById;
using HRsystem.Api.Features.VacationType.UpdateVacationType;
using HRsystem.Api.Features.VacationType.DeleteVacationType;
using MediatR;

namespace HRsystem.Api.Features.VacationType
{
    public static class VacationTypeEndpoints
    {
        public static void MapVacationTypeEndpoints(this IEndpointRouteBuilder app)
        {
            // Get all
            app.MapGet("/api/vacation-types", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllVacationTypesQuery());
                return Results.Ok(new { Success = true, Data = result });
            });

            // Get by Id
            app.MapGet("/api/vacation-types/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetVacationTypeByIdQuery(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"VacationType {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Create
            app.MapPost("/api/vacation-types", async (CreateVacationTypeCommand cmd, ISender mediator) =>
            {
                var result = await mediator.Send(cmd);
                return Results.Created($"/api/vacation-types/{result.VacationTypeId}", new { Success = true, Data = result });
            });

            // Update
            app.MapPut("/api/vacation-types/{id}", async (int id, UpdateVacationTypeCommand cmd, ISender mediator) =>
            {
                if (id != cmd.VacationTypeId)
                    return Results.BadRequest(new { Success = false, Message = "Id mismatch" });

                var result = await mediator.Send(cmd);
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"VacationType {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Delete
            app.MapDelete("/api/vacation-types/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteVacationTypeCommand(id));
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"VacationType {id} not found" })
                    : Results.Ok(new { Success = true, Message = $"VacationType {id} deleted successfully" });
            });
        }
    }
}
