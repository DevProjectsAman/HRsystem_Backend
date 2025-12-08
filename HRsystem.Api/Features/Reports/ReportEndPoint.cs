using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace HRsystem.Api.Features.Reports
{
    public static class ReportEndPoint
    {
        public static void MapReportEndPoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/reports")
                .WithTags("Reports")
                .WithOpenApi();

            // GET: api / reports / home - dashboard
            //group.MapGet("/home-dashboard", async (ISender mediator, CancellationToken cancellationToken) =>
            //{
            //    var result = await mediator.Send(new HomeDashboardReport.GetDashboardReportQuery(), cancellationToken);


            //    return Results.Ok(new
            //    {
            //        Success = result.Success,
            //        Message = result.Message,
            //        Data = result.Data
            //    });
            //})
            //.WithName("GetHomeDashboardReport")
            //.WithSummary("Get summarized statistics for Home Dashboard")
            //.WithDescription("Retrieves total employees, departments, requests, and other dashboard metrics.");

            //    group.MapGet("/home-dashboard", async (
            //          ISender mediator,
            //          [FromQuery] int? departmentId,
            //          [FromQuery] string? fromDay,
            //          [FromQuery] string? toDay,
            //          [FromQuery] int? topEmployeesCount,
            //          CancellationToken cancellationToken) =>
            //    {
            //        var query = new HomeDashboardReport.GetDashboardReportQuery(
            //            DepartmentId: departmentId,
            //            FromDay: fromDay,
            //            ToDay: toDay,
            //            TopEmployeesCount: topEmployeesCount ?? 5
            //        );

            //        var result = await mediator.Send(query, cancellationToken);

            //        return Results.Ok(new
            //        {
            //            Success = result.Success,
            //            Message = result.Message,
            //            Data = result.Data
            //        });
            //    })
            //      .WithName("GetHomeDashboardReport")
            //      .WithSummary("Get summarized statistics for Home Dashboard")
            //      .WithDescription("Retrieves total employees, departments, requests, and other dashboard metrics.");



            // ===================== Department-level Dashboard Endpoint =====================
            group.MapGet("/home-dashboard", async (
                    ISender mediator,
                    [FromQuery] int? departmentId,
                    [FromQuery] DateTime? fromDate,
                    [FromQuery] DateTime? toDate,
                    CancellationToken cancellationToken) =>
            {
                var query = new HomeDashboardReport.GetDashboardReportQuery(
                    DepartmentId: departmentId,
                    FromDate: fromDate,
                    ToDate: toDate
                );

                var result = await mediator.Send(query, cancellationToken);

                return Results.Ok(new
                {
                    Success = result.Success,
                    Message = result.Message,
                    Data = result.Data
                });
            })
            .WithName("GetHomeDashboardReport")
            .WithSummary("Get department-level attendance dashboard")
            .WithDescription("Retrieves department-level attendance percentages based on FromDate and ToDate");


            group.MapGet("/employee-attendance-report", async (
                    ISender mediator,
                    [FromQuery] int? departmentId,
                    [FromQuery] DateTime? fromDate,
                    [FromQuery] DateTime? toDate,
                    [FromQuery] int? topEmployeesCount,
                    CancellationToken cancellationToken) =>
            {
                var query = new EmployeeAttendanceReport.GetEmployeeAttendanceReportQuery(
                    DepartmentId: departmentId,
                    FromDate: fromDate,
                    ToDate: toDate,
                    TopEmployeesCount: topEmployeesCount ?? 5
                );

                var result = await mediator.Send(query, cancellationToken);

                return Results.Ok(new
                {
                    Success = result.Success,
                    Message = result.Message,
                    Data = result.Data
                });
            })
            .WithName("GetEmployeeAttendanceReport")
            .WithSummary("Get top employee attendance report")
            .WithDescription("Retrieves employee attendance percentages within a department and time period, ordered by most absent employees.");
        }
    }
}