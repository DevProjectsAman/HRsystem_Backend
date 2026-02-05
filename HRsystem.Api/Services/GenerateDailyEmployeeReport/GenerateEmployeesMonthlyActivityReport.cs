using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.LookupCashing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.Json;
using static HRsystem.Api.Enums.EnumsList;

namespace HRsystem.Api.Services.GenerateDailyEmployeeReport
{
    public interface IGenerateEmployeeMonthlyReportService
    {
        Task<string> GenerateMonthlyActivityReportAsync(DateTime? targetDate = null, CancellationToken ct = default);
    }

    public class GenerateEmployeesMonthlyActivityReport : IGenerateEmployeeMonthlyReportService
    {
        private readonly DBContextHRsystem _db;
        private readonly IActivityTypeLookupCache _activityTypeCache;
        private readonly ILogger<GenerateEmployeesMonthlyActivityReport> _logger;

        // Cache activity type IDs
        private int _attendanceTypeId;
        private int _vacationTypeId;
        private int _missionTypeId;
        private int _excuseTypeId;

        public GenerateEmployeesMonthlyActivityReport(
            DBContextHRsystem db,
            IActivityTypeLookupCache activityTypeCache,
            ILogger<GenerateEmployeesMonthlyActivityReport> logger)
        {
            _db = db;
            _activityTypeCache = activityTypeCache;
            _logger = logger;
        }

        public async Task<string> GenerateMonthlyActivityReportAsync(DateTime? targetDate = null, CancellationToken ct = default)
        {
            try
            {
                var processDate = (targetDate ?? DateTime.UtcNow).Date;
                _logger.LogInformation("Starting monthly activity report generation for {Date}", processDate);

                // Initialize activity type IDs
                InitializeActivityTypeIds();

                // Get all employees with necessary includes
                var employees = await GetEmployeesWithDetailsAsync(ct);

                if (!employees.Any())
                {
                    _logger.LogWarning("No employees found for report generation");
                    return "No employees found to process";
                }

                var processedCount = 0;
                var errorCount = 0;

                foreach (var employee in employees)
                {
                    try
                    {
                        await ProcessEmployeeDailyReportAsync(employee, processDate, ct);
//                         await _db.SaveChangesAsync(ct);
                        processedCount++;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing employee {EmployeeId}", employee.EmployeeId);
                        errorCount++;
                    }
                }

                await _db.SaveChangesAsync(ct);

                var message = $"✅ Monthly report completed. Processed: {processedCount}, Errors: {errorCount}";
                _logger.LogInformation(message);
                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critical error in monthly report generation");
                throw;
            }
        }

        private void InitializeActivityTypeIds()
        {
            _attendanceTypeId = _activityTypeCache.GetIdByCode(ActivityCodes.Attendance);
            _vacationTypeId = _activityTypeCache.GetIdByCode(ActivityCodes.VacationRequest);
            _missionTypeId = _activityTypeCache.GetIdByCode(ActivityCodes.MissionRequest);
            _excuseTypeId = _activityTypeCache.GetIdByCode(ActivityCodes.ExcuseRequest);
        }

        private async Task<List<TbEmployee>> GetEmployeesWithDetailsAsync(CancellationToken ct)
        {
            return await _db.TbEmployees
                .Include(e => e.JobTitle)
                    .ThenInclude(j => j.JobLevel)
                .Include(e => e.TbRemoteWorkDays)
                .Include(e => e.TbWorkDays)
                .Where(e => e.IsActive) // Only active employees
                .ToListAsync(ct);
        }

        private async Task ProcessEmployeeDailyReportAsync(TbEmployee employee, DateTime processDate, CancellationToken ct)
        {
            // Determine day type (workday, remote, holiday)
            var dayInfo = await GetDayInformationAsync(employee, processDate, ct);

            // Get or create daily report
            var dailyReport = await GetOrCreateDailyReportAsync(employee, processDate, dayInfo, ct);

            // Set initial status based on day type
            SetInitialDayStatus(dailyReport, dayInfo);

            // Get and process all activities for the day
            var activities = await GetEmployeeActivitiesAsync(employee.EmployeeId, processDate, ct);

            foreach (var activity in activities)
            {
                await ProcessActivityAsync(dailyReport, activity, ct);
            }
        }

        private async Task<DayInformation> GetDayInformationAsync(TbEmployee employee, DateTime date, CancellationToken ct)
        {
            var dayName = date.ToString("dddd", new CultureInfo("en-US"));

            var workDays = employee.TbWorkDays?.WorkDaysNames ?? new List<string>();
            var remoteDays = employee.TbRemoteWorkDays?.RemoteWorkDaysNames ?? new List<string>();

            var isHoliday = await _db.TbHolidays
                .AnyAsync(h =>
                    h.IsActive &&
                    date >= h.StartDate.Date &&
                    date <= h.EndDate.Date &&
                    (!h.IsForChristiansOnly || employee.Religion == EnumReligionType.Christian),
                    ct);

            return new DayInformation
            {
                IsWorkday = workDays.Any(d => d.Equals(dayName, StringComparison.OrdinalIgnoreCase)),
                IsRemoteDay = remoteDays.Any(d => d.Equals(dayName, StringComparison.OrdinalIgnoreCase)),
                IsHoliday = isHoliday
            };
        }

        private async Task<TbEmployeeMonthlyReport> GetOrCreateDailyReportAsync(
            TbEmployee employee,
            DateTime date,
            DayInformation dayInfo,
            CancellationToken ct)
        {
            var existingReport = await _db.TbEmployeeMonthlyReports
                .FirstOrDefaultAsync(r => r.EmployeeId == employee.EmployeeId && r.Date == date, ct);

            if (existingReport != null)
                return existingReport;

            var newReport = new TbEmployeeMonthlyReport
            {
                EmployeeId = employee.EmployeeId,
                Date = date,
                CompanyId = employee.CompanyId,
                DepartmentId = employee.DepartmentId,
                ShiftId = employee.ShiftId,
                WorkDaysId = employee.WorkDaysId,
                RemoteWorkDaysId = employee.RemoteWorkDaysId,
                EnglishFullName = employee.EnglishFullName,
                ArabicFullName = employee.ArabicFullName,
                ContractTypeId = employee.ContractTypeId,
                EmployeeCodeFinance = employee.EmployeeCodeFinance,
                EmployeeCodeHr = employee.EmployeeCodeHr,
                JobTitleId = employee.JobTitleId,
                JobLevelId = employee.JobTitle?.JobLevelId ?? 0,
                ManagerId = employee.ManagerId,
                IsWorkday = dayInfo.IsWorkday,
                IsRemoteday = dayInfo.IsRemoteDay,
                IsHoliday = dayInfo.IsHoliday
            };

            _db.TbEmployeeMonthlyReports.Add(newReport);
            return newReport;
        }

        private void SetInitialDayStatus(TbEmployeeMonthlyReport report, DayInformation dayInfo)
        {
            if (dayInfo.IsHoliday)
            {
                report.EmployeeTodayStatuesId = 1; // Present (Holiday)
                report.TodayStatues = "Holiday";
            }
            else if (dayInfo.IsRemoteDay)
            {
                report.EmployeeTodayStatuesId = 1; // Present (Remote)
                report.TodayStatues = "RemoteDay";
            }
            else if (dayInfo.IsWorkday)
            {
                report.EmployeeTodayStatuesId = 2; // Absent (until proven otherwise)
                report.TodayStatues = "Workday";
            }
        }

        private async Task<List<TbEmployeeActivity>> GetEmployeeActivitiesAsync(
            int employeeId,
            DateTime date,
            CancellationToken ct)
        {
            return await _db.TbEmployeeActivities
                .Where(a => a.EmployeeId == employeeId && a.RequestDate.Date == date)
                .ToListAsync(ct);
        }

        private async Task ProcessActivityAsync(
            TbEmployeeMonthlyReport report,
            TbEmployeeActivity activity,
            CancellationToken ct)
        {
            // Update common activity fields
            report.ActivityId = activity.ActivityId;
            report.ActivityTypeId = activity.ActivityTypeId;
            report.RequestBy = activity.RequestBy;
            report.ApprovedBy = activity.ApprovedBy;
            report.RequestDate = activity.RequestDate;
            report.ApprovedDate = activity.ApprovedDate;

            // Process specific activity type
            if (activity.ActivityTypeId == _attendanceTypeId)
                await ProcessAttendanceAsync(report, activity.ActivityId, ct);
            else if (activity.ActivityTypeId == _vacationTypeId)
                await ProcessVacationAsync(report, activity.ActivityId, ct);
            else if (activity.ActivityTypeId == _missionTypeId)
                await ProcessMissionAsync(report, activity.ActivityId, ct);
            else if (activity.ActivityTypeId == _excuseTypeId)
                await ProcessExcuseAsync(report, activity.ActivityId, ct);
        }

        private async Task ProcessAttendanceAsync(
     TbEmployeeMonthlyReport report,
     long activityId,
     CancellationToken ct)
        {
            var attendance = await _db.TbEmployeeAttendances
                .FirstOrDefaultAsync(a => a.ActivityId == activityId, ct);

            if (attendance == null)
                return;

            report.AttendanceId = attendance.ActivityId;
            report.AttendanceDate = attendance.AttendanceDate;
            report.FirstPuchin = attendance.FirstPuchin;
            report.LastPuchout = attendance.LastPuchout;
            report.TotalHours = attendance.TotalHours;
            report.ActualWorkingHours = attendance.ActualWorkingHours;
            report.AttStatues = attendance.AttStatues;

            report.EmployeeTodayStatuesId = _attendanceTypeId; // Present

            if (!report.TodayStatues.Contains("Attendance"))
                report.TodayStatues += " Attendance";

            // ✅ Append attendance details instead of overwriting
            DailyReportDetailsHelper.AppendEvent(
                report,
                "Attendance",
                new Dictionary<string, object?>
                {
                    ["AttendanceId"] = attendance.AttendanceId,
                    ["AttendanceDate"] = attendance.AttendanceDate,
                    ["FirstPuchin"] = attendance.FirstPuchin,
                    ["LastPuchout"] = attendance.LastPuchout,
                    ["TotalHours"] = attendance.TotalHours,
                    ["ActualWorkingHours"] = attendance.ActualWorkingHours,
                    ["AttStatues"] = attendance.AttStatues
                });
        }

        private async Task ProcessVacationAsync(
      TbEmployeeMonthlyReport report,
      long activityId,
      CancellationToken ct)
        {
            var vacation = await _db.TbEmployeeVacations
                .FirstOrDefaultAsync(v => v.ActivityId == activityId, ct);

            if (vacation == null)
                return;

            var reportDate = DateOnly.FromDateTime(report.Date);

            // If report day is within vacation range
            if (reportDate >= vacation.StartDate &&
                reportDate <= vacation.EndDate)
            {
                report.EmployeeTodayStatuesId = _vacationTypeId; // Vacation

                if (!report.TodayStatues.Contains("Vacation"))
                    report.TodayStatues += " Vacation";

                // ✅ Append instead of overwrite
                DailyReportDetailsHelper.AppendEvent(
                    report,
                    "Vacation",
                    new Dictionary<string, object?>
                    {
                        ["VacationId"] = vacation.VacationId,
                        ["VacationTypeId"] = vacation.VacationTypeId,
                        ["StartDate"] = vacation.StartDate,
                        ["EndDate"] = vacation.EndDate,
                        ["DaysCount"] = vacation.DaysCount,
                        ["Notes"] = vacation.Notes
                    });
            }

            // Create entries only for multi-day vacations
          //  if (vacation.DaysCount > 1)
           // {
                await CreateVacationDaysEntriesAsync(report, vacation, ct);
           // }
        }

        private async Task CreateVacationDaysEntriesAsync(
            TbEmployeeMonthlyReport baseReport,
            TbEmployeeVacation vacation,
            CancellationToken ct)
        {
            for (int i = 0; i < vacation.DaysCount; i++)
            {
                var vacationDay = vacation.StartDate.AddDays(i).ToDateTime(TimeOnly.MinValue);

                // Skip if already exists or is the current day
                if (vacationDay.Date == baseReport.Date.Date) continue;

                var exists = await _db.TbEmployeeMonthlyReports
                    .AnyAsync(r => r.EmployeeId == baseReport.EmployeeId && r.Date.Date == vacationDay.Date, ct);

                if (exists) continue;

                var vacationDayReport = new TbEmployeeMonthlyReport
                {
                    EmployeeId = baseReport.EmployeeId,
                    Date = vacationDay,
                    CompanyId = baseReport.CompanyId,
                    DepartmentId = baseReport.DepartmentId,
                    ShiftId = baseReport.ShiftId,
                    WorkDaysId = baseReport.WorkDaysId,
                    RemoteWorkDaysId = baseReport.RemoteWorkDaysId,
                    EnglishFullName = baseReport.EnglishFullName,
                    ArabicFullName = baseReport.ArabicFullName,
                    ContractTypeId = baseReport.ContractTypeId,
                    EmployeeCodeFinance = baseReport.EmployeeCodeFinance,
                    EmployeeCodeHr = baseReport.EmployeeCodeHr,
                    JobTitleId = baseReport.JobTitleId,
                    JobLevelId = baseReport.JobLevelId,
                    ManagerId = baseReport.ManagerId,
                    ActivityId = vacation.ActivityId,
                    ActivityTypeId = baseReport.ActivityTypeId,
                    RequestBy = baseReport.RequestBy,
                    ApprovedBy = baseReport.ApprovedBy,
                    RequestDate = baseReport.RequestDate,
                    ApprovedDate = baseReport.ApprovedDate,
                    EmployeeTodayStatuesId = 3,
                    TodayStatues = "Vacation",
                    Details = baseReport.Details
                };

                _db.TbEmployeeMonthlyReports.Add(vacationDayReport);
            }
        }

        private async Task ProcessMissionAsync(TbEmployeeMonthlyReport report, long activityId, CancellationToken ct)
        {
            var mission = await _db.TbEmployeeMissions
                .FirstOrDefaultAsync(m => m.ActivityId == activityId, ct);

            if (mission == null) return;

            report.EmployeeTodayStatuesId = _missionTypeId; // On Mission
            if (!report.TodayStatues.Contains("Mission"))
                report.TodayStatues += " Mission";
            

            // ✅ Append instead of overwrite
            DailyReportDetailsHelper.AppendEvent(
                report,
                "Mission",
                new Dictionary<string, object?>
                {
                    ["MissionId"] = mission.MissionId,
                    ["StartDatetime"] = mission.StartDatetime,
                    ["EndDatetime"] = mission.EndDatetime,
                    ["MissionLocation"] = mission.MissionLocation,
                    ["MissionReason"] = mission.MissionReason
                });
        }

        private async Task ProcessExcuseAsync(TbEmployeeMonthlyReport report, long activityId, CancellationToken ct)
        {
            var excuse = await _db.TbEmployeeExcuses
                .FirstOrDefaultAsync(e => e.ActivityId == activityId, ct);

            if (excuse == null) return;

            report.EmployeeTodayStatuesId = _excuseTypeId; // Excused
            report.TodayStatues += " Excuse";
            if (!report.TodayStatues.Contains("Excuse"))
                report.TodayStatues += " Excuse";


            // ✅ Append instead of overwrite
            DailyReportDetailsHelper.AppendEvent(
                report,
                "Excuse",
                new Dictionary<string, object?>
                {
                    ["ExcuseId"] = excuse.ExcuseId,
                    ["ExcuseDate"] = excuse.ExcuseDate,
                    ["StartTime"] = excuse.StartTime,
                    ["EndTime"] = excuse.EndTime,
                    ["ExcuseReason"] = excuse.ExcuseReason
                });
        }




        // Helper class
        private class DayInformation
        {
            public bool IsWorkday { get; set; }
            public bool IsRemoteDay { get; set; }
            public bool IsHoliday { get; set; }
        }
    }

    // =============================================================================
    // BACKGROUND SERVICE - Runs automatically at midnight
    // =============================================================================
    public class MonthlyReportBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MonthlyReportBackgroundService> _logger;
      //  private readonly TimeSpan _executionTime = new(0, 0, 0); // Midnight
        private readonly TimeSpan _executionTime = new(23, 0, 0); // Midnight

        public MonthlyReportBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<MonthlyReportBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Monthly Report Background Service started");


            // --- ADD THIS FOR TESTING ---
            // This ensures the service runs once immediately on startup
            _logger.LogInformation("Running initial report generation on startup...");
         //   await GenerateReportAsync(stoppingToken);
            // ----------------------------



            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.UtcNow;
                    var nextRun = GetNextRunTime(now);
                    var delay = nextRun - now;

                    _logger.LogInformation("Next report generation scheduled at {Time}", nextRun);

                    await Task.Delay(delay, stoppingToken);

                    if (!stoppingToken.IsCancellationRequested)
                    {
                        await GenerateReportAsync(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in background service execution");
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // Retry after 5 minutes
                }
            }
        }

        private DateTime GetNextRunTime(DateTime from)
        {
            var nextRun = from.Date.Add(_executionTime);

            if (nextRun <= from)
            {
                nextRun = nextRun.AddDays(1);
            }

            return nextRun;
        }

        private async Task GenerateReportAsync(CancellationToken ct)
        {
            using var scope = _serviceProvider.CreateScope();
            var reportService = scope.ServiceProvider.GetRequiredService<IGenerateEmployeeMonthlyReportService>();

            try
            {
                _logger.LogInformation("Starting automated monthly report generation");
                var result = await reportService.GenerateMonthlyActivityReportAsync(null, ct);
                _logger.LogInformation("Report generation completed: {Result}", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate automated monthly report");
            }
        }
    }

    // =============================================================================
    // API ENDPOINT - For manual execution
    // =============================================================================
    /*
    // Add to your controller
    [HttpPost("api/reports/generate-monthly-activity")]
    [Authorize(Roles = "Admin,HR")]
    public async Task<IActionResult> GenerateMonthlyReport(
        [FromQuery] DateTime? targetDate,
        [FromServices] IEmployeeMonthlyReportService reportService)
    {
        try
        {
            var result = await reportService.GenerateMonthlyActivityReportAsync(targetDate);
            return Ok(new { success = true, message = result });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    */

    // =============================================================================
    // PROGRAM.CS REGISTRATION
    // =============================================================================
    /*
    // Add these lines in your Program.cs

    // Register the service
    builder.Services.AddScoped<IEmployeeMonthlyReportService, GenerateEmployeesMonthlyActivityReport>();

    // Register background service (runs at midnight)
    builder.Services.AddHostedService<MonthlyReportBackgroundService>();
    */
}