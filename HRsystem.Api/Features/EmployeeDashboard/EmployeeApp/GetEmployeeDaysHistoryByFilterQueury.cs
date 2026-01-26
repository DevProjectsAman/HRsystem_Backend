using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.EmployeeDashboard.EmployeeApp
{
    public record GetEmployeeDaysHistoryByFilterQueury(DateTime From, DateTime To)
        : IRequest<List<EmployeeDaysHistoryDto>>;

    public class EmployeeDaysHistoryDto
    {
        public DateOnly DayDate { get; set; }
        public DateTime? FirstPunchIn { get; set; }
        public DateTime? LastPunchOut { get; set; }
        public statues Status { get; set; }
        public List<EmployeeRequestDto> Requests { get; set; } = new();
    }

    public class EmployeeRequestDto
    {
        public long ActivityId { get; set; }
        public string ActivityName { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;
    }

    public class GetEmployeeDaysHistoryByFilterQueuryHandler
        : IRequestHandler<GetEmployeeDaysHistoryByFilterQueury, List<EmployeeDaysHistoryDto>>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUserService;

        public GetEmployeeDaysHistoryByFilterQueuryHandler(DBContextHRsystem db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }

        public async Task<List<EmployeeDaysHistoryDto>> Handle(GetEmployeeDaysHistoryByFilterQueury request, CancellationToken ct)
        {
            var employeeId = _currentUserService.EmployeeID;
            var lang = _currentUserService.UserLanguage ?? "en";

            // 1️⃣ هات كل الـ Activities الخاصة بالموظف في الفترة
            var fromDate = request.From.Date;
            var toDate = request.To.Date.AddDays(1);

            var activities = await _db.TbEmployeeActivities
                .Include(a => a.ActivityType)
                .Include(a => a.Status)
                .Where(a =>
                    a.EmployeeId == employeeId &&
                    a.RequestDate >= fromDate &&
                    a.RequestDate < toDate)
                .ToListAsync(ct);

            // 1️⃣ هات كل الـ Activities الخاصة بالموظف في الفترة
            //var activities = await _db.TbEmployeeActivities
            //    .Include(a => a.ActivityType)
            //    .Include(a => a.Status)
            //    .Where(a => a.EmployeeId == employeeId &&
            //                a.RequestDate >= request.From &&
            //                a.RequestDate <= request.To )
            //    .ToListAsync(ct);

            // 2️⃣ هات كل الـ ActivityIds
            var activityIds = activities.Select(a => a.ActivityId).ToList();

            // 3️⃣ هات كل الـ Attendance اللي ليها ActivityId
            var attendances = await _db.TbEmployeeAttendances
                .Where(att => activityIds.Contains(att.ActivityId))
                .ToListAsync(ct);

            // 4️⃣ اعمل grouping حسب اليوم
            var groupedByDay = activities
                .GroupBy(a => a.RequestDate.Date)
                .OrderBy(g => g.Key)
                .ToList();

            var result = new List<EmployeeDaysHistoryDto>();

            foreach (var group in groupedByDay)
            {
                var day = group.Key;

                // attendance المرتبط بأول Activity في اليوم (أو أي Activity عندها Attendance)
                var attendance = attendances.FirstOrDefault(att =>
                {
                    var act = activities.FirstOrDefault(a => a.ActivityId == att.ActivityId);
                    return act != null && act.RequestDate.Date == day;
                });


                // لو attendance موجود خد منه المعلومات
                var dto = new EmployeeDaysHistoryDto
                {
                    DayDate = DateOnly.FromDateTime(day),
                    FirstPunchIn = attendance?.FirstPuchin,
                    LastPunchOut = attendance?.LastPuchout,
                    Status = attendance.AttStatues,
                    Requests = group.Select(a => new EmployeeRequestDto
                    {
                        ActivityId = a.ActivityId,
                        ActivityName = lang == "ar"
                            ? a.ActivityType.ActivityName.ar
                            : a.ActivityType.ActivityName.en,
                        StatusName = lang == "ar"
                            ? a.Status.StatusName.ar
                            : a.Status.StatusName.en
                    }).ToList()
                };

                result.Add(dto);
            }

            return result;
        }
   
    
    
    }
}
