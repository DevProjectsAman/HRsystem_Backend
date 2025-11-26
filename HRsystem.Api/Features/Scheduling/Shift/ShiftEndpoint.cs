using FluentValidation;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.Scheduling.Shift.CreateShift;
using HRsystem.Api.Features.Scheduling.Shift.DeleteShift;
using HRsystem.Api.Features.Scheduling.Shift.GetAllShifts;
using HRsystem.Api.Features.Scheduling.Shift.GetShiftById;
using HRsystem.Api.Features.Scheduling.Shift.UpdateShift;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace HRsystem.Api.Features.Scheduling.Shift
{
    public static class ShiftEndpoints
    {
        public static void MapShiftEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Scheduling/shifts").WithTags("Shifts");

            // Get all
            group.MapGet("/ListShifts/{id}", async (int id ,ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllShiftsQuery(id));
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

                return Results.Ok(new ResponseResultDTO<TbShift>()
                {
                    Success = true,
                    Message = "Shift created successfully",
                    Data = result
                });
            });

            // Update
            group.MapPut("/UpdateShift", async (UpdateShiftCommand command, ISender mediator, IValidator<UpdateShiftCommand> validator) =>
            {
                var validation = await validator.ValidateAsync(command);
                if (!validation.IsValid)
                    return Results.BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

                var result = await mediator.Send(command);
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"Shift {command.ShiftId} not found" })
                    : Results.Ok(new { Success = true, Message = $"Shift {command.ShiftId} updated successfully" });
            });

            // Delete
            group.MapDelete("/DeleteShift/{id}", async (int id, ISender mediator) =>
            {
                try
                {

                var result = await mediator.Send(new DeleteShiftCommand(id));
                return !result
                    ? Results.NotFound(new { Success = false, Message = $"Shift {id} not found" })
                    : Results.Ok(new { Success = true, Message = $"Shift {id} deleted successfully" });

                }
                catch (Exception ex)
                {

                    return Results.Ok(new ResponseResultDTO() { Success = false, Message = ex.Message });
                }
            });
        }
    }


}
