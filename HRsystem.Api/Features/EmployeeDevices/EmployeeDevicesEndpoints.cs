using FluentValidation;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace HRsystem.Api.Features.EmployeeDevices
{
    public static class EmployeeDevicesEndpoints
    {
        public static void MapEmployeeDevicesEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/EmployeeDevices")
                           .WithTags("Employee Devices")
                           .RequireAuthorization();

            // ============================================================
            // ✅ 1. Check if current device is registered & active
            // ============================================================
            group.MapGet("/CheckCurrentDevice", [Authorize]
            async (IMediator mediator) =>
                {
                    var exists = await mediator.Send(new CheckEmployeeDeviceQuery());

                    return new ResponseResultDTO<bool>
                    {
                        Success = true,
                        StatusCode = exists ? 200 : 409,
                        Message = exists
                            ? "Device is registered and active"
                            : "Device is not registered",
                        Data = exists
                    };



                })
                .WithName("CheckEmployeeDevice");

            // ============================================================
            // ✅ 2. Add new device for current employee
            // ============================================================
            group.MapPost("/AddDevice", [Authorize]
            async (
                    IMediator mediator,
                    IValidator<AddEmployeeDeviceCommand> validator,
                    AddEmployeeDeviceCommand cmd) =>
                {
                    var validation = await validator.ValidateAsync(cmd);
                    if (!validation.IsValid)
                    {
                        return Results.Ok(new ResponseResultDTO
                        {
                            Success = false,
                            Message = "Validation failed",
                            Errors = validation.Errors.Select(e =>
                                new ResponseErrorDTO
                                {
                                    Property = e.PropertyName,
                                    Error = e.ErrorMessage
                                }).ToList()
                        });
                    }

                    var result = await mediator.Send(cmd);

                    if (!result)
                    {
                        return Results.Ok(new ResponseResultDTO
                        {
                            Success = false,
                            Message = "This device is already registered"
                        });
                    }

                    return Results.Created("/api/EmployeeDevices/AddDevice",
                        new ResponseResultDTO
                        {
                            Success = true,
                            Message = "Device registered successfully"
                        });
                })
                .WithName("AddEmployeeDevice");

            // ============================================================
            // ✅ 3. Reset (Deactivate) employee active device
            // ============================================================
            group.MapPut("/ResetDevice", [Authorize]
            async (IMediator mediator, ResetEmployeeDeviceCommand cmd) =>
                {
                    var result = await mediator.Send(cmd);

                    if (!result)
                    {
                        return Results.NotFound(new ResponseResultDTO
                        {
                            Success = false,
                            Message = "No Active device found for this employee"
                        });
                    }

                    return Results.Ok(new ResponseResultDTO
                    {
                        Success = true,
                        Message = "Employee device reset successfully"
                    });
                })
                .WithName("ResetEmployeeDevice");



            group.MapGet("/ListEmployeeDevices/{employeeId:int}", [Authorize]
            async (IMediator mediator, int employeeId) =>
        {
            var result = await mediator.Send(
                new ListEmployeeDevicesQuery(employeeId));

            return Results.Ok(new ResponseResultDTO<List<EmployeeDeviceDto>>
            {
                Success = true,
                Message = "Employee devices retrieved successfully",
                Data = result
            });
        })
        .WithName("ListEmployeeDevices");



        }
    }
}
