using FluentValidation;
using HRsystem.Api.Features.EmployeeHandler.Create;
using HRsystem.Api.Features.EmployeeHandler.Get;
using HRsystem.Api.Features.EmployeeHandler.GetEmployeeForEdit;
using HRsystem.Api.Features.EmployeeHandler.GetList;
using HRsystem.Api.Features.EmployeeHandler.UpdateEmployee;
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
                    return Results.Ok(new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Search term is required."
                    });
                }

                var result = await mediator.Send(new SearchEmployeeQuery(searchTerm));

                

                return Results.Ok(result);
            })
            .WithName("SearchEmployee");


            // --- Get Employees List (Optional Department + Pagination) ---
            group.MapGet("/GetEmployeesList", [Authorize] async (
                IMediator mediator,
                int? departmentId,
                string? Search,
                string? SortBy,
                string? SortDirection,
                int pageNumber = 1,
                int pageSize = 10) =>
                        {
                var result = await mediator.Send(
                    new GetEmployeesListQuery(departmentId, Search, SortBy, SortDirection, pageNumber, pageSize)
                );

                return Results.Ok(result);
            })
            .WithName("GetEmployees");


            group.MapGet("/GetAllEmployeesList", [Authorize] async (
               IMediator mediator,
               int? departmentId
               ) =>
            {
                var result = await mediator.Send(
                    new GetAllEmployeesListQuery(departmentId)
                );

                return Results.Ok(result);
            })
           .WithName("GetAllEmployees");


            // --- Get Employee For Edit ---
            group.MapGet("/GetEmployeeForEdit/{employeeId:int}", [Authorize] async (
                IMediator mediator,
                int employeeId) =>
            {
                var result = await mediator.Send(
                    new GetEmployeeForEditQuery(employeeId)
                );


                var x = result;


                return Results.Ok(  result);
            })
            .WithName("GetEmployeeForEdit");


            #region Update Employee Sections

            // --- Update Basic Data ---
            group.MapPut("/{employeeId}/BasicData", [Authorize] async (
                IMediator mediator,
                [FromServices] IValidator<UpdateEmployeeBasicDataCommand> validator,
                int employeeId,
                [FromBody] UpdateEmployeeBasicDataCommand cmd) =>
            {
                if (employeeId != cmd.EmployeeId)
                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Employee ID mismatch"
                    });

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

                var result = await mediator.Send(cmd);
                return Results.Ok(result);
            })
            .WithName("UpdateEmployeeBasicDataNew");

            // --- Update Extra Data ---
            group.MapPut("/{employeeId}/ExtraData", [Authorize] async (
                IMediator mediator,
                [FromServices] IValidator<UpdateEmployeeExtraDataCommand> validator,
                int employeeId,
                [FromBody] UpdateEmployeeExtraDataCommand cmd) =>
            {
                if (employeeId != cmd.EmployeeId)
                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Employee ID mismatch"
                    });

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

                var result = await mediator.Send(cmd);
                return Results.Ok(result);
            })
            .WithName("UpdateEmployeeExtraDataNew");

            // --- Update Organization ---
            group.MapPut("/{employeeId}/Organization", [Authorize] async (
                IMediator mediator,
                [FromServices] IValidator<UpdateEmployeeOrganizationCommand> validator,
                int employeeId,
                [FromBody] UpdateEmployeeOrganizationCommand cmd) =>
            {
                if (employeeId != cmd.EmployeeId)
                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Employee ID mismatch"
                    });

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

                var result = await mediator.Send(cmd);
                return Results.Ok(result);
            })
            .WithName("UpdateEmployeeOrganizationNew");

            // --- Update Hiring Info ---
            group.MapPut("/{employeeId}/Hiring", [Authorize] async (
                IMediator mediator,
                [FromServices] IValidator<UpdateEmployeeHiringCommand> validator,
                int employeeId,
                [FromBody] UpdateEmployeeHiringCommand cmd) =>
            {
                if (employeeId != cmd.EmployeeId)
                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Employee ID mismatch"
                    });

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

                var result = await mediator.Send(cmd);
                return Results.Ok(result);
            })
            .WithName("UpdateEmployeeHiringNew");

            // --- Update Work Locations ---
            group.MapPut("/{employeeId}/WorkLocations", [Authorize] async (
                IMediator mediator,
                [FromServices] IValidator<UpdateEmployeeWorkLocationsCommandNew> validator,
                int employeeId,
                [FromBody] UpdateEmployeeWorkLocationsCommandNew cmd) =>
            {
                if (employeeId != cmd.EmployeeId)
                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Employee ID mismatch"
                    });

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

                var result = await mediator.Send(cmd);
                return Results.Ok(result);
            })
            .WithName("UpdateEmployeeWorkLocationsNew");

            // --- Update Projects ---
            group.MapPut("/{employeeId}/Projects", [Authorize] async (
                IMediator mediator,
                [FromServices] IValidator<UpdateEmployeeProjectsCommand> validator,
                int employeeId,
                [FromBody] UpdateEmployeeProjectsCommand cmd) =>
            {
                if (employeeId != cmd.EmployeeId)
                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Employee ID mismatch"
                    });

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

                var result = await mediator.Send(cmd);
                return Results.Ok(result);
            })
            .WithName("UpdateEmployeeProjectsNew");

            // --- Update Shift & Work Days ---
            group.MapPut("/{employeeId}/ShiftWorkDays", [Authorize] async (
                IMediator mediator,
                [FromServices] IValidator<UpdateEmployeeShiftWorkDaysCommand> validator,
                int employeeId,
                [FromBody] UpdateEmployeeShiftWorkDaysCommand cmd) =>
            {
                if (employeeId != cmd.EmployeeId)
                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Employee ID mismatch"
                    });

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

                var result = await mediator.Send(cmd);
                return Results.Ok(result);
            })
            .WithName("UpdateEmployeeShiftWorkDaysNew");

            // --- Update Vacation Balances ---
            group.MapPut("/{employeeId}/VacationBalances", [Authorize] async (
                IMediator mediator,
                [FromServices] IValidator<UpdateEmployeeVacationBalancesCommand> validator,
                int employeeId,
                [FromBody] UpdateEmployeeVacationBalancesCommand cmd) =>
            {
                if (employeeId != cmd.EmployeeId)
                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Employee ID mismatch"
                    });

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

                var result = await mediator.Send(cmd);
                return Results.Ok(result);
            })
            .WithName("UpdateEmployeeVacationBalancesNew");

            // --- Update Full Employee (Bulk Update) ---
            group.MapPut("/{employeeId}/Full", [Authorize] async (
                IMediator mediator,
                int employeeId,
                [FromBody] UpdateEmployeeFullCommand cmd) =>
            {
                if (employeeId != cmd.EmployeeId)
                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Employee ID mismatch"
                    });

                var result = await mediator.Send(cmd);
                return Results.Ok(result);
            })
            .WithName("UpdateEmployeeFullNew");

            #endregion



        }


    }
}



