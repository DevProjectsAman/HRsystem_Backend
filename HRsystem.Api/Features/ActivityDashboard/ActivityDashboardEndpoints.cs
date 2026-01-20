// Features/ActivityDashboard/ActivityDashboardEndpoints.cs
using HRsystem.Api.Shared;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRsystem.Api.Features.ActivityDashboard
{
    public static class ActivityDashboardEndpoints
    {
        public static void MapActivityDashboardEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/ActivityDashboard")
                           .WithTags("Activity Dashboard");

            // ✅ Get Dashboard Summary (Main Endpoint)
            group.MapPost("/Summary", [Authorize] async (
                IMediator mediator,
                [FromBody] GetActivityDashboardRequest request) =>
            {
                try
                {
                    var query = new GetActivityDashboard(
                        request.StartDate,
                        request.EndDate,
                        request.DepartmentId,
                        request.EmployeeId,
                        request.GroupByDepartment
                    );

                    var result = await mediator.Send(query);

                    return Results.Ok(new ResponseResultDTO<ActivityDashboardDto>
                    {
                        Success = true,
                        Message = "Dashboard data retrieved successfully",
                        Data = result
                    });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = ex.Message
                    });
                }
            })
            .WithName("GetActivityDashboardSummary");

            // ✅ Get Dashboard By Employee
            group.MapGet("/ByEmployee", [Authorize] async (
                IMediator mediator,
                [FromQuery] DateTime? startDate,
                [FromQuery] DateTime? endDate,
                [FromQuery] int? departmentId) =>
            {
                try
                {
                    var query = new GetActivityDashboard(
                        startDate,
                        endDate,
                        departmentId,
                        null,
                        false  // Group by employee
                    );

                    var result = await mediator.Send(query);

                    return Results.Ok(new ResponseResultDTO<ActivityDashboardDto>
                    {
                        Success = true,
                        Message = "Employee activity data retrieved successfully",
                        Data = result
                    });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = ex.Message
                    });
                }
            })
            .WithName("GetDashboardByEmployee");

            // ✅ Get Dashboard By Department
            group.MapGet("/ByDepartment", [Authorize] async (
                IMediator mediator,
                [FromQuery] DateTime? startDate,
                [FromQuery] DateTime? endDate) =>
            {
                try
                {
                    var query = new GetActivityDashboard(
                        startDate,
                        endDate,
                        null,
                        null,
                        true  // Group by department
                    );

                    var result = await mediator.Send(query);

                    return Results.Ok(new ResponseResultDTO<ActivityDashboardDto>
                    {
                        Success = true,
                        Message = "Department activity data retrieved successfully",
                        Data = result
                    });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = ex.Message
                    });
                }
            })
            .WithName("GetDashboardByDepartment");

            // ✅ Get Specific Employee Activities
            group.MapGet("/Employee/{employeeId}", [Authorize] async (
                IMediator mediator,
                int employeeId,
                [FromQuery] DateTime? startDate,
                [FromQuery] DateTime? endDate) =>
            {
                try
                {
                    var query = new GetActivityDashboard(
                        startDate,
                        endDate,
                        null,
                        employeeId,
                        false
                    );

                    var result = await mediator.Send(query);

                    if (result.ActivitySummaries.Count == 0)
                    {
                        return Results.NotFound(new ResponseResultDTO
                        {
                            Success = false,
                            Message = $"No activities found for employee {employeeId}"
                        });
                    }

                    return Results.Ok(new ResponseResultDTO<ActivityDashboardDto>
                    {
                        Success = true,
                        Message = "Employee activities retrieved successfully",
                        Data = result
                    });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = ex.Message
                    });
                }
            })
            .WithName("GetEmployeeActivities");

            // ✅ Get Activity Trends
            group.MapPost("/Trends", [Authorize] async (
                IMediator mediator,
                [FromBody] GetActivityTrendsRequest request) =>
            {
                try
                {
                    var query = new GetActivityTrends(
                        request.StartDate,
                        request.EndDate,
                        request.DepartmentId,
                        request.EmployeeId,
                        request.Grouping
                    );

                    var result = await mediator.Send(query);

                    return Results.Ok(new ResponseResultDTO<List<ActivityTrendDto>>
                    {
                        Success = true,
                        Message = "Activity trends retrieved successfully",
                        Data = result
                    });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = ex.Message
                    });
                }
            })
            .WithName("GetActivityTrends");

            // ✅ Get Activity Trends By Grouping Type (GET version)
            group.MapGet("/Trends/{grouping}", [Authorize] async (
                IMediator mediator,
                TrendGrouping grouping,
                [FromQuery] DateTime? startDate,
                [FromQuery] DateTime? endDate,
                [FromQuery] int? departmentId,
                [FromQuery] int? employeeId) =>
            {
                try
                {
                    var query = new GetActivityTrends(
                        startDate,
                        endDate,
                        departmentId,
                        employeeId,
                        grouping
                    );

                    var result = await mediator.Send(query);

                    return Results.Ok(new ResponseResultDTO<List<ActivityTrendDto>>
                    {
                        Success = true,
                        Message = $"{grouping} trends retrieved successfully",
                        Data = result
                    });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = ex.Message
                    });
                }
            })
            .WithName("GetActivityTrendsByGrouping");
        }
    }

    // ✅ Request DTOs
    public record GetActivityDashboardRequest(
        DateTime? StartDate,
        DateTime? EndDate,
        int? DepartmentId,
        int? EmployeeId,
        bool GroupByDepartment
    );

    public record GetActivityTrendsRequest(
        DateTime? StartDate,
        DateTime? EndDate,
        int? DepartmentId,
        int? EmployeeId,
        TrendGrouping Grouping
    );
}