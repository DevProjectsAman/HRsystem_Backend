using FluentValidation;
using HRsystem.Api.Shared.DTO;
using MediatR;

namespace HRsystem.Api.Features.EmployeeHandler
{
    public static class EmployeeHandlerEndPoints
    {

        public static void MapEmployeeHandlerEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/EmployeesHandler")
                           .WithTags("Employees");

            // ✅ Create Employee
            group.MapPost("/CreateEmployee", async (
                CreateEmployeeCommand cmd,
                ISender mediator,
                IValidator<CreateEmployeeCommand> validator) =>
            {
                var validationResult = await validator.ValidateAsync(cmd);

                if (!validationResult.IsValid)
                {
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
                }

                var employeeId = await mediator.Send(cmd);

                return Results.Created(
                    $"/api/Employees/{employeeId}",
                    new ResponseResultDTO<object>
                    {
                        Success = true,
                        Message = "Employee created successfully",
                        Data = new
                        {
                            EmployeeId = employeeId
                        }
                    });
            });
        }
    }
}


