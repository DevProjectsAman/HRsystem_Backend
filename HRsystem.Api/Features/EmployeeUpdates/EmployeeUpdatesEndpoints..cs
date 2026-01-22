using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRsystem.Api.Features.EmployeeUpdates
{
    public static class EmployeeUpdatesEndpoints

    {
       
 
            public static void MapEmployeeUpdatesEndpoints(this IEndpointRouteBuilder app)
            {
                var group = app.MapGroup("/api/EmployeeUpdates")
                               .WithTags("Employee Work Locations");

                // =====================================================
                // ✅ Get Employee Work Locations
                // =====================================================
                group.MapGet("/GetWorkLocationByEmployee/{employeeId:int}", [Authorize] async (
                    IMediator mediator,
                    int employeeId) =>
                {
                    if (employeeId <= 0)
                    {
                        return Results.BadRequest(new ResponseResultDTO
                        {
                            Success = false,
                            Message = "Invalid employee ID."
                        });
                    }

                    var data = await mediator.Send(
                        new EmployeeWorkLocations.GetEmployeeWorkLocationsQuery(employeeId));

                    return Results.Ok(new ResponseResultDTO<object>
                    {
                        Success = true,
                        Data = data
                    });
                })
                .WithName("GetEmployeeWorkLocations");


                // =====================================================
                // ✅ Update Employee Work Locations (Delete + Insert)
                // =====================================================
                group.MapPost("/UpdateEmployeeWorkLocations", [Authorize] async (
                    IMediator mediator,
                    [FromBody] EmployeeWorkLocations.UpdateEmployeeWorkLocationsCommand command) =>
                {
                    if (command.EmployeeId <= 0)
                    {
                        return Results.BadRequest(new ResponseResultDTO
                        {
                            Success = false,
                            Message = "Invalid employee ID."
                        });
                    }

                    await mediator.Send(command);

                    return Results.Ok(new ResponseResultDTO
                    {
                        Success = true,
                        Message = "Employee work locations updated successfully."
                    });
                })
                .WithName("UpdateEmployeeWorkLocations");
            }
        }
    }

 
