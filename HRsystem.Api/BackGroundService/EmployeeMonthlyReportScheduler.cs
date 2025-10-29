using HRsystem.Api.Services.Reports;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class EmployeeMonthlyReportScheduler : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public EmployeeMonthlyReportScheduler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.Now;

            // تحديد وقت التشغيل: 11:05 صباحًا
            var nextRun = DateTime.Today.AddHours(13).AddMinutes(10);

            // لو الوقت الحالي عدى 11:05 النهارده → شغلها بكرة
            if (now > nextRun)
                nextRun = nextRun.AddDays(1);

            var delay = nextRun - now;

            Console.WriteLine($"[Scheduler] Next run scheduled at: {nextRun}");

            // انتظر لحد ميعاد التشغيل
            await Task.Delay(delay, stoppingToken);

            using var scope = _scopeFactory.CreateScope();
            var reportService = scope.ServiceProvider.GetRequiredService<IEmployeeMonthlyReportService>();

            try
            {
                Console.WriteLine($"[Scheduler] Running Employee Monthly Report at {DateTime.Now}");
                var result = await reportService.GenerateMonthlyReportAsync(stoppingToken);
                Console.WriteLine($"[Scheduler] {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Scheduler ERROR] {ex.Message}");
            }
        }
    }

}
