using HRsystem.Api.Features.Shift.DeleteShift;
using HRsystem.Api.Features.Shift.GetAllShifts;
using HRsystem.Api.Features.Shift.GetShiftById;
using HRsystem.Api.Features.Shift.UpdateShift;
using MediatR;
using FluentValidation;

namespace HRsystem.Api.Features.Shift.Endpoints
{
    public static class ShiftEndpoints
    {
        public static void MapShiftEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/shifts").WithTags("Shifts");

            // Get all
            group.MapGet("/ListShifts", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllShiftsQuery());
                return Results.Ok(new { Success = true, Data = result });
            });

            // Get by Id
            group.MapGet("/GetOneShift/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetShiftByIdQuery(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Shift {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Create
            group.MapPost("/CreateShift", async (CreateShiftCommand command, ISender mediator, IValidator<CreateShiftCommand> validator) =>
            {
                var validation = await validator.ValidateAsync(command);
                if (!validation.IsValid)
                    return Results.BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

                var result = await mediator.Send(command);
                return Results.Created($"/api/shifts/{result}", new { Success = true, ShiftId = result });
            });

            // Update
            group.MapPut("/UpdateShift/{id}", async (int id, UpdateShiftCommand command, ISender mediator, IValidator<UpdateShiftCommand> validator) =>
            {
                if (id != command.ShiftId)
                    return Results.BadRequest(new { Success = false, Message = "Id mismatch" });

                var validation = await validator.ValidateAsync(command);
                if (!validation.IsValid)
                    return Results.BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

                var result = await mediator.Send(command);
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"Shift {id} not found" })
                    : Results.Ok(new { Success = true, Message = $"Shift {id} updated successfully" });
            });

            // Delete
            group.MapDelete("/DeleteShift/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteShiftCommand(id));
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"Shift {id} not found" })
                    : Results.Ok(new { Success = true, Message = $"Shift {id} deleted successfully" });
            });
        }
    }
}
