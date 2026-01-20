using global::HRsystem.Api.Database;
// Features/ActivityDashboard/GetActivityDashboard.cs
 
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.ActivityDashboard
{
   
 
        // ✅ DTOs
        public class ActivityDashboardDto
        {
            public List<ActivitySummaryDto> ActivitySummaries { get; set; } = new();
            public ActivityStatisticsDto Statistics { get; set; } = new();
        }

        public class ActivitySummaryDto
        {
            // Grouping fields
            public int? EmployeeId { get; set; }
            public string? EmployeeName { get; set; }
            public int? DepartmentId { get; set; }
            public string? DepartmentName { get; set; }

            // Activity counts by type
            public int AttendanceCount { get; set; }
            public int MissionCount { get; set; }
            public int ExcuseCount { get; set; }
            public int VacationCount { get; set; }
            public int TotalActivities { get; set; }

            // Status breakdown
            public Dictionary<string, int> StatusBreakdown { get; set; } = new();

            // Percentages
            public decimal AttendancePercentage { get; set; }
            public decimal MissionPercentage { get; set; }
            public decimal ExcusePercentage { get; set; }
            public decimal VacationPercentage { get; set; }
        }

        public class ActivityStatisticsDto
        {
            // Overall totals
            public int TotalActivities { get; set; }
            public int TotalAttendance { get; set; }
            public int TotalMissions { get; set; }
            public int TotalExcuses { get; set; }
            public int TotalVacations { get; set; }

            // Overall percentages
            public decimal AttendancePercentage { get; set; }
            public decimal MissionPercentage { get; set; }
            public decimal ExcusePercentage { get; set; }
            public decimal VacationPercentage { get; set; }

            // Status distribution
            public Dictionary<string, int> OverallStatusDistribution { get; set; } = new();
            public Dictionary<string, decimal> StatusPercentages { get; set; } = new();
        }

        // ✅ Query
        public record GetActivityDashboard(
            DateTime? StartDate,
            DateTime? EndDate,
            int? DepartmentId,
            int? EmployeeId,
            bool GroupByDepartment
        ) : IRequest<ActivityDashboardDto>;

        // ✅ Handler
        public class GetActivityDashboardHandler : IRequestHandler<GetActivityDashboard, ActivityDashboardDto>
        {
            private readonly DBContextHRsystem _db;

            public GetActivityDashboardHandler(DBContextHRsystem db)
            {
                _db = db;
            }

            public async Task<ActivityDashboardDto> Handle(GetActivityDashboard request, CancellationToken ct)
            {
                // Base query with includes
                var query = _db.TbEmployeeActivities
                    .Include(a => a.Employee)
                    .Include(a => a.ActivityType)
                    .Include(a => a.Status)
                    .AsQueryable();

                // Apply date filters
                if (request.StartDate.HasValue)
                    query = query.Where(a => a.RequestDate >= request.StartDate.Value);

                if (request.EndDate.HasValue)
                    query = query.Where(a => a.RequestDate <= request.EndDate.Value);

                // Apply department filter
                if (request.DepartmentId.HasValue)
                    query = query.Where(a => a.Employee.DepartmentId == request.DepartmentId.Value);

                // Apply employee filter
                if (request.EmployeeId.HasValue)
                    query = query.Where(a => a.EmployeeId == request.EmployeeId.Value);

                var activities = await query.ToListAsync(ct);

                var summaries = request.GroupByDepartment
                    ? await GetDepartmentSummariesAsync(activities, ct)
                    : GetEmployeeSummaries(activities);

                return new ActivityDashboardDto
                {
                    ActivitySummaries = summaries,
                    Statistics = CalculateStatistics(activities)
                };
            }

            private List<ActivitySummaryDto> GetEmployeeSummaries(List<Database.DataTables.TbEmployeeActivity> activities)
            {
                var grouped = activities.GroupBy(a => new
                {
                    a.EmployeeId,
                    a.Employee.EnglishFullName,
                    a.Employee.DepartmentId
                });

                var summaries = new List<ActivitySummaryDto>();

                foreach (var group in grouped)
                {
                    var summary = new ActivitySummaryDto
                    {
                        EmployeeId = group.Key.EmployeeId,
                        EmployeeName = group.Key.EnglishFullName,
                        DepartmentId = group.Key.DepartmentId
                    };

                    CalculateActivityCounts(summary, group.ToList());
                    summaries.Add(summary);
                }

                return summaries.OrderBy(s => s.EmployeeName).ToList();
            }

            private async Task<List<ActivitySummaryDto>> GetDepartmentSummariesAsync(
                List<Database.DataTables.TbEmployeeActivity> activities, CancellationToken ct)
            {
                var grouped = activities.GroupBy(a => a.Employee.DepartmentId);

                var summaries = new List<ActivitySummaryDto>();

                foreach (var group in grouped)
                {
                    var deptName = await _db.TbDepartments
                        .Where(d => d.DepartmentId == group.Key)
                        .Select(d => d.DepartmentName)
                        .FirstOrDefaultAsync(ct);

                    var summary = new ActivitySummaryDto
                    {
                        DepartmentId = group.Key,
                        DepartmentName = deptName.en
                    };

                    CalculateActivityCounts(summary, group.ToList());
                    summaries.Add(summary);
                }

                return summaries.OrderBy(s => s.DepartmentName).ToList();
            }

            private void CalculateActivityCounts(ActivitySummaryDto summary, List<Database.DataTables.TbEmployeeActivity> activities)
            {
                // Count by activity type
                summary.AttendanceCount = activities.Count(a =>
                    a.ActivityType.ActivityName.en.Contains("Attendance", StringComparison.OrdinalIgnoreCase));

                summary.MissionCount = activities.Count(a =>
                    a.ActivityType.ActivityName.en.Contains("Mission", StringComparison.OrdinalIgnoreCase));

                summary.ExcuseCount = activities.Count(a =>
                    a.ActivityType.ActivityName.en.Contains("Excuse", StringComparison.OrdinalIgnoreCase));

                summary.VacationCount = activities.Count(a =>
                    a.ActivityType.ActivityName.en.Contains("Vacation", StringComparison.OrdinalIgnoreCase));

                summary.TotalActivities = activities.Count;

                // Calculate percentages
                if (summary.TotalActivities > 0)
                {
                    summary.AttendancePercentage = Math.Round(
                        (decimal)summary.AttendanceCount / summary.TotalActivities * 100, 2);

                    summary.MissionPercentage = Math.Round(
                        (decimal)summary.MissionCount / summary.TotalActivities * 100, 2);

                    summary.ExcusePercentage = Math.Round(
                        (decimal)summary.ExcuseCount / summary.TotalActivities * 100, 2);

                    summary.VacationPercentage = Math.Round(
                        (decimal)summary.VacationCount / summary.TotalActivities * 100, 2);
                }

                // Status breakdown
                summary.StatusBreakdown = activities
                    .GroupBy(a => a.Status.StatusName.en)
                    .ToDictionary(g => g.Key, g => g.Count());
            }

            private ActivityStatisticsDto CalculateStatistics(List<Database.DataTables.TbEmployeeActivity> activities)
            {
                var stats = new ActivityStatisticsDto
                {
                    TotalActivities = activities.Count,
                    TotalAttendance = activities.Count(a =>
                        a.ActivityType.ActivityName.en.Contains("Attendance", StringComparison.OrdinalIgnoreCase)),
                    TotalMissions = activities.Count(a =>
                        a.ActivityType.ActivityName.en.Contains("Mission", StringComparison.OrdinalIgnoreCase)),
                    TotalExcuses = activities.Count(a =>
                        a.ActivityType.ActivityName.en.Contains("Excuse", StringComparison.OrdinalIgnoreCase)),
                    TotalVacations = activities.Count(a =>
                        a.ActivityType.ActivityName.en.Contains("Vacation", StringComparison.OrdinalIgnoreCase))
                };

                // Calculate overall percentages
                if (stats.TotalActivities > 0)
                {
                    stats.AttendancePercentage = Math.Round(
                        (decimal)stats.TotalAttendance / stats.TotalActivities * 100, 2);

                    stats.MissionPercentage = Math.Round(
                        (decimal)stats.TotalMissions / stats.TotalActivities * 100, 2);

                    stats.ExcusePercentage = Math.Round(
                        (decimal)stats.TotalExcuses / stats.TotalActivities * 100, 2);

                    stats.VacationPercentage = Math.Round(
                        (decimal)stats.TotalVacations / stats.TotalActivities * 100, 2);
                }

                // Overall status distribution
                stats.OverallStatusDistribution = activities
                    .GroupBy(a => a.Status.StatusName.en)
                    .ToDictionary(g => g.Key, g => g.Count());

                // Status percentages
                foreach (var status in stats.OverallStatusDistribution)
                {
                    if (stats.TotalActivities > 0)
                    {
                        stats.StatusPercentages[status.Key] = Math.Round(
                            (decimal)status.Value / stats.TotalActivities * 100, 2);
                    }
                }

                return stats;
            }
        }
    }

