//using HRsystem.Api.Services.Reports;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;

//public class EmployeeMonthlyReportScheduler : BackgroundService
//{
//    private readonly IServiceScopeFactory _scopeFactory;

//    public EmployeeMonthlyReportScheduler(IServiceScopeFactory scopeFactory)
//    {
//        _scopeFactory = scopeFactory;
//    }

//    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        while (!stoppingToken.IsCancellationRequested)
//        {
//            var now = DateTime.UtcNow;

//            // تحديد وقت التشغيل: 11:05 صباحًا
//            var nextRun = DateTime.Today.AddHours(13).AddMinutes(10);

//            // لو الوقت الحالي عدى 11:05 النهارده → شغلها بكرة
//            if (now > nextRun)
//                nextRun = nextRun.AddDays(1);

//            var delay = nextRun - now;

//            Console.WriteLine($"[Scheduler] Next run scheduled at: {nextRun}");

//            // انتظر لحد ميعاد التشغيل
//            await Task.Delay(delay, stoppingToken);

//            using var scope = _scopeFactory.CreateScope();
//            var reportService = scope.ServiceProvider.GetRequiredService<IEmployeeMonthlyReportService>();

//            try
//            {
//                Console.WriteLine($"[Scheduler] Running Employee Monthly Report at {DateTime.UtcNow}");
//                var result = await reportService.GenerateMonthlyReportAsync(stoppingToken);
//                Console.WriteLine($"[Scheduler] {result}");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"[Scheduler ERROR] {ex.Message}");
//            }
//        }
//    }

//}





using HRsystem.Api.Services.Reports;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class EmployeeMonthlyReportScheduler : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<EmployeeMonthlyReportScheduler> _logger;

    public EmployeeMonthlyReportScheduler(
        IServiceScopeFactory scopeFactory,
        ILogger<EmployeeMonthlyReportScheduler> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("EmployeeMonthlyReportScheduler started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var now = DateTime.UtcNow;

                // تحديد وقت التشغيل: 1:10 مساءً بتوقيت UTC
                var nextRun = DateTime.UtcNow.Date.AddHours(13).AddMinutes(40);

                // لو الوقت الحالي عدى الميعاد النهارده → شغلها بكرة
                if (now >= nextRun)
                    nextRun = nextRun.AddDays(1);

                var delay = nextRun - now;

                _logger.LogInformation("Next report scheduled at: {NextRun} (in {DelayHours:F2} hours)",
                    nextRun, delay.TotalHours);

                // انتظر لحد ميعاد التشغيل - ممكن يرمي TaskCanceledException لو البرنامج اتقفل
                await Task.Delay(delay, stoppingToken);

                // شغل الريبورت
                await GenerateReport(stoppingToken);
            }
            catch (TaskCanceledException)
            {
                // ده متوقع لما البرنامج يقفل - مش error
                _logger.LogInformation("EmployeeMonthlyReportScheduler is stopping (application shutdown)");
                break; // اخرج من اللوب
            }
            catch (Exception ex)
            {
                // أي error تاني غير متوقع
                _logger.LogError(ex, "Unexpected error in EmployeeMonthlyReportScheduler");

                // استنى 5 دقائق وحاول تاني
                try
                {
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    _logger.LogInformation("EmployeeMonthlyReportScheduler stopping during error recovery");
                    break;
                }
            }
        }

        _logger.LogInformation("EmployeeMonthlyReportScheduler stopped");
    }

    private async Task GenerateReport(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var reportService = scope.ServiceProvider.GetRequiredService<IEmployeeMonthlyReportService>();

        try
        {
            _logger.LogInformation("Running Employee Monthly Report at {Time}", DateTime.UtcNow);

            var result = await reportService.GenerateMonthlyReportAsync(stoppingToken);

            _logger.LogInformation("Report completed: {Result}", result);
        }
        catch (TaskCanceledException)
        {
            _logger.LogWarning("Report generation was cancelled");
            throw; // أعيد رمي الـ exception علشان يتعامل معاها في الـ outer catch
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating monthly report");
            // مش هنرمي الـ exception تاني علشان الـ scheduler يفضل شغال
        }
    }
}
