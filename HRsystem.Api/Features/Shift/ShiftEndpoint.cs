using HRsystem.Api.Features.Shift;
using HRsystem.Api.Features.Shift.DeleteShift;
using HRsystem.Api.Features.Shift.GetAllShifts;
using HRsystem.Api.Features.Shift.GetShiftById;
using HRsystem.Api.Features.Shift.UpdateShift;

using MediatR;

namespace HRsystem.Api.Features.Shift
{
    public static class ShiftEndpoints
    {
        public static void MapShiftEndpoints(this IEndpointRouteBuilder app)
        {
            // Get all
            app.MapGet("/api/shifts", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllShiftsQuery());
                return Results.Ok(new { Success = true, Data = result });
            });

            // Get by Id
            app.MapGet("/api/shifts/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetShiftByIdQuery(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Shift {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Create
            app.MapPost("/api/shifts", async (CreateShiftCommand command, ISender mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Created($"/api/shifts/{result}", new { Success = true, ShiftId = result });
            });

            // Update
            app.MapPut("/api/shifts/{id}", async (int id, UpdateShiftCommand command, ISender mediator) =>
            {
                if (id != command.ShiftId)
                    return Results.BadRequest(new { Success = false, Message = "Id mismatch" });

                var result = await mediator.Send(command);
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"Shift {id} not found" })
                    : Results.Ok(new { Success = true, Message = $"Shift {id} updated successfully" });
            });

            // Delete
            app.MapDelete("/api/shifts/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteShiftCommand(id));
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"Shift {id} not found" })
                    : Results.Ok(new { Success = true, Message = $"Shift {id} deleted successfully" });
            });
        }
    }
}
