using HRsystem.Api.Features.EmployeeRequest.EmployeeVacation;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
//using static HRsystem.Api.Features.employeevacations.EmployeeVacations;

namespace HRsystem.Api.Features.employeevacations
{
    public static class EmployeeVacationsEndPoints
    {
        public static void MapEmployeeVacationsEndPoints(this IEndpointRouteBuilder app)
        {
            //var group = app.MapGroup("/api/vacations").WithTags("Vacations");
            var group = app.MapGroup("/api/employee-requests").WithTags("Employee Requests");

            group.MapGet("/mybalances", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetEmployeeVacationsQuery());
                return result == null || !result.Any()
                    ? Results.NotFound(new { Success = false, Message = "No balances found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            //var group = app.MapGroup("/api/vacations").WithTags("Vacations");
            //var group = app.MapGroup("/api/employee-requests").WithTags("Employee Requests");

            // ✅ Request Vacation
            group.MapPost("/vacation-request", async (
                RequestVacationCommand command,
                ISender mediator) =>
            {
                //var result = await mediator.Send(command);
                //return Results.Ok(new
                //{
                //    Success = true,
                //    Message = "Vacation requested successfully",
                //    Data = result
                //});
                try
                {
                    var result = await mediator.Send(command);

                    return Results.Ok(new { Success = true, Message = "Vacation requested successfully", Data = result });
                }
                catch (Exception ex)
                {
                    return Results.Ok(new
                    {
                        success = false,
                        message = ex.Message,
                        data = (object)null
                    });
                }
            });


            // ✅ Get Vacation Balance by Type
            group.MapGet("/get-balance/{vacationTypeId}", async (int vacationTypeId, ISender mediator) =>
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
    
