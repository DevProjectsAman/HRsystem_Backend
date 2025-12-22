using FluentValidation;
using HRsystem.Api.Features.EmployeeHandler.Create;
using HRsystem.Api.Features.EmployeeHandler.Get;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


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
   IMediator mediator, [FromServices] IValidator<CreateEmployeeCommandNew> validator, [FromBody] CreateEmployeeCommandNew cmd) =>
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


            // --- Search Employee by NationalID / HR / Finance code ---
            group.MapGet("/SearchEmployee", [Authorize] async (IMediator mediator, string searchTerm) =>
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Search term is required."
                    });
                }

                var result = await mediator.Send(new SearchEmployeeQuery(searchTerm));

                if (!result.Success)
                {
                    return Results.NotFound(result);
                }

                return Results.Ok(result);
            })
            .WithName("SearchEmployee");
        }

    }
}



