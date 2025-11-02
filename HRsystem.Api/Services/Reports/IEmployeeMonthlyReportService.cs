using HRsystem.Api.Features.EmployeeDashboard.EmployeeMonthlyReport;
using MediatR;

namespace HRsystem.Api.Services.Reports
{
    public interface IEmployeeMonthlyReportService
    {
        Task<string> GenerateMonthlyReportAsync(CancellationToken ct);
    }
    

    public class EmployeeMonthlyReportService : IEmployeeMonthlyReportService
    {
        private readonly IMediator _mediator;

        public EmployeeMonthlyReportService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<string> GenerateMonthlyReportAsync(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetEmployeeMonthlyReport(), ct);
            return result;
        }
    }

}
