// Features/ActivityDashboard/GetActivityTrends.cs
using HRsystem.Api.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.ActivityDashboard
{
    // ✅ DTO
    public class ActivityTrendDto
    {
        public string Period { get; set; } = string.Empty;
        public int AttendanceCount { get; set; }
        public int MissionCount { get; set; }
        public int ExcuseCount { get; set; }
        public int VacationCount { get; set; }
        public int TotalCount { get; set; }
    }

    public enum TrendGrouping
    {
        Daily,
        Weekly,
        Monthly
    }

    // ✅ Query
    public record GetActivityTrends(
        DateTime? StartDate,
        DateTime? EndDate,
        int? DepartmentId,
        int? EmployeeId,
        TrendGrouping Grouping
    ) : IRequest<List<ActivityTrendDto>>;

    // ✅ Handler
    public class GetActivityTrendsHandler : IRequestHandler<GetActivityTrends, List<ActivityTrendDto>>
    {
        private readonly DBContextHRsystem _db;

        public GetActivityTrendsHandler(DBContextHRsystem db)
        {
            _db = db;
        }

        public async Task<List<ActivityTrendDto>> Handle(GetActivityTrends request, CancellationToken ct)
        {
            var query = _db.TbEmployeeActivities
                .Include(a => a.ActivityType)
                .Include(a => a.Employee)
                .AsQueryable();

            // Apply filters
            if (request.StartDate.HasValue)
                query = query.Where(a => a.RequestDate >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                query = query.Where(a => a.RequestDate <= request.EndDate.Value);

            if (request.DepartmentId.HasValue)
                query = query.Where(a => a.Employee.DepartmentId == request.DepartmentId.Value);

            if (request.EmployeeId.HasValue)
                query = query.Where(a => a.EmployeeId == request.EmployeeId.Value);

            var activities = await query.ToListAsync(ct);

            return request.Grouping switch
            {
                TrendGrouping.Daily => GroupByDay(activities),
                TrendGrouping.Weekly => GroupByWeek(activities),
                TrendGrouping.Monthly => GroupByMonth(activities),
                _ => GroupByDay(activities)
            };
        }

        private List<ActivityTrendDto> GroupByDay(List<Database.DataTables.TbEmployeeActivity> activities)
        {
            return activities
                .GroupBy(a => a.RequestDate.Date)
                .Select(g => new ActivityTrendDto
                {
                    Period = g.Key.ToString("yyyy-MM-dd"),
                    AttendanceCount = g.Count(a => a.ActivityType.ActivityName.en.Contains("Attendance", StringComparison.OrdinalIgnoreCase)),
                    MissionCount = g.Count(a => a.ActivityType.ActivityName.en.Contains("Mission", StringComparison.OrdinalIgnoreCase)),
                    ExcuseCount = g.Count(a => a.ActivityType.ActivityName.en.Contains("Excuse", StringComparison.OrdinalIgnoreCase)),
                    VacationCount = g.Count(a => a.ActivityType.ActivityName.en.Contains("Vacation", StringComparison.OrdinalIgnoreCase)),
                    TotalCount = g.Count()
                })
                .OrderBy(t => t.Period)
                .ToList();
        }

        private List<ActivityTrendDto> GroupByWeek(List<Database.DataTables.TbEmployeeActivity> activities)
        {
            return activities
                .GroupBy(a => new
                {
                    Year = a.RequestDate.Year,
                    Week = GetWeekOfYear(a.RequestDate)
                })
                .Select(g => new ActivityTrendDto
                {
                    Period = $"{g.Key.Year}-W{g.Key.Week:D2}",
                    AttendanceCount = g.Count(a => a.ActivityType.ActivityName.en.Contains("Attendance", StringComparison.OrdinalIgnoreCase)),
                    MissionCount = g.Count(a => a.ActivityType.ActivityName.en.Contains("Mission", StringComparison.OrdinalIgnoreCase)),
                    ExcuseCount = g.Count(a => a.ActivityType.ActivityName.en.Contains("Excuse", StringComparison.OrdinalIgnoreCase)),
                    VacationCount = g.Count(a => a.ActivityType.ActivityName.en.Contains("Vacation", StringComparison.OrdinalIgnoreCase)),
                    TotalCount = g.Count()
                })
                .OrderBy(t => t.Period)
                .ToList();
        }

        private List<ActivityTrendDto> GroupByMonth(List<Database.DataTables.TbEmployeeActivity> activities)
        {
            return activities
                .GroupBy(a => new { a.RequestDate.Year, a.RequestDate.Month })
                .Select(g => new ActivityTrendDto
                {
                    Period = $"{g.Key.Year}-{g.Key.Month:D2}",
                    AttendanceCount = g.Count(a => a.ActivityType.ActivityName.en.Contains("Attendance", StringComparison.OrdinalIgnoreCase)),
                    MissionCount = g.Count(a => a.ActivityType.ActivityName.en.Contains("Mission", StringComparison.OrdinalIgnoreCase)),
                    ExcuseCount = g.Count(a => a.ActivityType.ActivityName.en.Contains("Excuse", StringComparison.OrdinalIgnoreCase)),
                    VacationCount = g.Count(a => a.ActivityType.ActivityName.en.Contains("Vacation", StringComparison.OrdinalIgnoreCase)),
                    TotalCount = g.Count()
                })
                .OrderBy(t => t.Period)
                .ToList();
        }

        private int GetWeekOfYear(DateTime date)
        {
            var cal = System.Globalization.CultureInfo.CurrentCulture.Calendar;
            return cal.GetWeekOfYear(date,
                System.Globalization.CalendarWeekRule.FirstDay,
                DayOfWeek.Sunday);
        }
    }
}