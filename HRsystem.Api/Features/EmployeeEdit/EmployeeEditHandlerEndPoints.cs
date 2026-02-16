using FluentValidation;
using HRsystem.Api.Features.EmployeeEdit.GetEmployeeData;
using HRsystem.Api.Features.EmployeeEdit.UpdateEmployee;
using HRsystem.Api.Features.EmployeeHandler.Create;
using HRsystem.Api.Features.EmployeeHandler.Get;
using HRsystem.Api.Features.EmployeeHandler.GetList;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRsystem.Api.Features.EmployeeEdit
{
  

   
        public static class EmployeeHandlerEndPoints
        {
            public static void MapEmployeeEditHandlerEndpoints(this IEndpointRouteBuilder app)
            {
                var group = app.MapGroup("/api/EmployeesEditHandler")
                               .WithTags("Employees");

              

                #region Get Employee Data

                
 

                // --- Get Employee Full Details ---
                group.MapGet("/{employeeId}/Details", [Authorize] async (
                    IMediator mediator,
                    int employeeId) =>
                {
                    var result = await mediator.Send(new GetEmployeeFullDetailsQuery(employeeId));

                    if (!result.Success)
                        return Results.NotFound(result);

                    return Results.Ok(result);
                })
                .WithName("GetEmployeeFullDetailsNew");

                #endregion

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
