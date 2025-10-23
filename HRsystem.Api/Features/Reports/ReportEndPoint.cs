using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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

            // GET: api/reports/home-dashboard
            group.MapGet("/home-dashboard", async (ISender mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new HomeDashboardReport.GetDashboardReportQuery(), cancellationToken);

            
                return Results.Ok(new
                {
                    Success = result.Success,
                    Message = result.Message,
                    Data = result.Data
                });
            })
            .WithName("GetHomeDashboardReport")
            .WithSummary("Get summarized statistics for Home Dashboard")
            .WithDescription("Retrieves total employees, departments, requests, and other dashboard metrics.");
        }
    }
}
