using System.Threading;
using System.Threading.Tasks;

namespace HRsystem.Api.Services.Reports
{
    public interface IEmployeeMonthlyReportService
    {
        Task<string> GenerateMonthlyReportAsync(CancellationToken ct);
    }
}
