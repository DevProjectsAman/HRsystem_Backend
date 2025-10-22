using HRsystem.Api.Features.EmployeeActivityDt.EmployeePunch;
using HRsystem.Api.Services.Reports;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRsystem.Api.Features.EmployeeDashboard.EmployeeMonthlyReport
{

    public static class EmployeeReportEndpoints
    {
        public static void MapEmployeeReportEndpoints(this IEndpointRouteBuilder app)
        {

            var group = app.MapGroup("/api/employee-Dashboard/report").WithTags("Employees reports");

            // Punch In
            group.MapPost("/reports", async (GetEmployeeMonthlyReport cmd, ISender mediator) =>
            {
                var result = await mediator.Send(cmd);
                return Results.Ok(new { Success = true, Data = result });
            });
        }
        /* [ApiController]
         [Route("api/reports")]
         public class ReportsController : ControllerBase
         {
             private readonly IEmployeeMonthlyReportService _reportService;

             public ReportsController(IEmployeeMonthlyReportService reportService)
             {   
                 _reportService = reportService;
             }

             [HttpPost("generate-daily")]
             public async Task<IActionResult> GenerateReport(CancellationToken ct)
             {
                 var result = await _reportService.GenerateMonthlyReportAsync(ct);
                 return Ok(new { Success = true, Message = result });
             }
         }
         */
    }
}
