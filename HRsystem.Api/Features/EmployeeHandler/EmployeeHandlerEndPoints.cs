using FluentValidation;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using HRsystem.Api.Features.EmployeeHandler.Create;


namespace HRsystem.Api.Features.EmployeeHandler
{
    public static class EmployeeHandlerEndPoints
    {

        public static void MapEmployeeHandlerEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/EmployeesHandler")
                           .WithTags("Employees");

            // ✅ Create Employee
            group.MapPost("/CreateEmployee", [Authorize] async (
   IMediator mediator,
   IValidator<CreateEmployeeCommand> validator,
   CreateEmployeeCommand cmd) =>
            {
                var validationResult = await validator.ValidateAsync(cmd);

                if (!validationResult.IsValid)
                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = validationResult.Errors.Select(e => new ResponseErrorDTO
                        {
                            Property = e.PropertyName,
                            Error = e.ErrorMessage
                        }).ToList()
                    });

                var employeeId = await mediator.Send(cmd);

                return Results.Created(
                    $"/api/Employees/{employeeId}",
                    new ResponseResultDTO<object>
                    {
                        Success = true,
                        Message = "Employee created successfully",
                        Data = new { EmployeeId = employeeId }
                    });
            });

        }
    }
}


