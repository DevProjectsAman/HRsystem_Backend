using MediatR;

namespace HRsystem.Api.Features.EmployeeVacation
{
    public static class EmployeeVacationEndpoints
    {
        public static void MapVacationEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/vacations").WithTags("Vacations");

            // ✅ Request Vacation
            group.MapPost("/request", async (
                RequestVacationCommand command,
                ISender mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Ok(new
                {
                    Success = true,
                    Message = "Vacation requested successfully",
                    Data = result
                });
            });

            // ✅ Get Vacation Balance by Type
            group.MapGet("/balance/{vacationTypeId}", async (int vacationTypeId, ISender mediator) =>
            {
                var result = await mediator.Send(new GetVacationBalanceCommand(vacationTypeId));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Balance for vacation type {vacationTypeId} not found" })
                    : Results.Ok(new
                    {
                        Success = true,
                        Message = "Vacation balance retrieved successfully",
                        Data = result
                    });
            });
        }
    }
}
