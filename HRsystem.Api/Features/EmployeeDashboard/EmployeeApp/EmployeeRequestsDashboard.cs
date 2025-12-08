using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.EmployeeDashboard.EmployeeApp
{
    public class EmployeeLastActivityDto
    {
        public long ActivityId { get; set; }
        public int StatusId { get; set; }
        public DateTime RequestDate { get; set; }
    };
    public class EmployeeLastExcusesDto : EmployeeLastActivityDto
    {
        public int ActivityTypeId { get; set; } = 4;
        public string ActivityTypeName { get; set; }
    };

    public class EmployeeLastMissionDto : EmployeeLastActivityDto
    {
        public int ActivityTypeId { get; set; } = 5;
        public string ActivityTypeName { get; set; }
    };
    public class EmployeeLastVacationDto : EmployeeLastActivityDto
    {
        public int ActivityTypeId { get; set; } = 6;
        public string ActivityTypeName { get; set; }

        public int VacationTypeId { get; set; }
        public string VacationTypeName { get; set; }
    };
    public class EmployeeRequestsDashboardDto
    {
        public List<EmployeeLastExcusesDto> Excuses { get; set; }
        public List<EmployeeLastMissionDto> Missions { get; set; }
        public List<EmployeeLastVacationDto> Vacations { get; set; }
    };


    public record EmployeeRequestsDashboardQuery() : IRequest<EmployeeRequestsDashboardDto>;

    public class EmployeeRequestsDashboardHandler : IRequestHandler<EmployeeRequestsDashboardQuery, EmployeeRequestsDashboardDto>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;

        public EmployeeRequestsDashboardHandler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<EmployeeRequestsDashboardDto> Handle(EmployeeRequestsDashboardQuery request, CancellationToken ct)
        {
            var employeeId = _currentUser.EmployeeID;

            // -----------------------------------------
            // 1️⃣ Excuses — ActivityType = 4
            // -----------------------------------------
            var Excuses = await _db.TbEmployeeActivities
                .Where(a => a.EmployeeId == employeeId && a.ActivityTypeId == 6)
                .Include(a => a.ActivityType) // هنا ترجع جدول ActivityType كامل
                .OrderByDescending(a => a.RequestDate)
                .Take(2)
                .Select(a => new EmployeeLastExcusesDto
                {
                    ActivityId = a.ActivityId,
                    StatusId = a.StatusId,
                    RequestDate = a.RequestDate,
                    ActivityTypeName = _currentUser.UserLanguage == "ar"
                                                    ? a.ActivityType.ActivityName.ar
                                                    : a.ActivityType.ActivityName.en

                })
                .ToListAsync(ct);


            // -----------------------------------------
            // 2️⃣ Missions — ActivityType = 5
            // -----------------------------------------
            var missions = await _db.TbEmployeeActivities
                .Where(a => a.EmployeeId == employeeId && a.ActivityTypeId == 4)
                .Include(a => a.ActivityType) // هنا ترجع جدول ActivityType كامل
                .OrderByDescending(a => a.RequestDate)
                .Take(2)
                .Select(a => new EmployeeLastMissionDto
                {
                    ActivityId = a.ActivityId,
                    StatusId = a.StatusId,
                    RequestDate = a.RequestDate,
                    ActivityTypeName = _currentUser.UserLanguage == "ar"
                                                    ? a.ActivityType.ActivityName.ar
                                                    : a.ActivityType.ActivityName.en
                })
                .ToListAsync(ct);


            // -----------------------------------------
            // 3️⃣ Vacations — ActivityType = 6
            // ♦️ هنا نعمل JOIN على TbVacationType
            // -----------------------------------------
            //var vacations = await _db.TbEmployeeActivities
            //    .Where(a => a.EmployeeId == employeeId && a.ActivityTypeId == 6)
            //    .Include(a => a.ActivityType) // هنا ترجع جدول ActivityType كامل
            //    .Include(v => v.TbEmployeeVacations)
            //    .OrderByDescending(a => a.RequestDate)
            //    .Take(2)
            //    .Select(a => new EmployeeLastMissionDto
            //    {
            //        ActivityId = a.ActivityId,
            //        StatusId = a.StatusId,
            //        RequestDate = a.RequestDate,
            //        ActivityTypeName = _currentUser.UserLanguage == "ar"
            //                                        ? a.ActivityType.ActivityName.ar
            //                                        : a.ActivityType.ActivityName.en
            //    })
            //    .ToListAsync(ct);
            var vacations = await (from v in _db.TbEmployeeVacations
                                   join a in _db.TbEmployeeActivities
                                       on v.ActivityId equals a.ActivityId
                                   join t in _db.TbVacationTypes
                                       on v.VacationTypeId equals t.VacationTypeId
                                   where a.EmployeeId == employeeId
                                   orderby a.RequestDate descending
                                   select new EmployeeLastVacationDto
                                   {
                                       ActivityId = a.ActivityId,
                                       StatusId = a.StatusId,
                                       VacationTypeId = v.VacationTypeId,
                                       VacationTypeName = _currentUser.UserLanguage == "ar"
                                           ? t.VacationName.ar
                                           : t.VacationName.en,
                                       RequestDate = a.RequestDate
                                   }).Take(2).ToListAsync(ct);

            return new EmployeeRequestsDashboardDto
            {
                Excuses = Excuses,
                Missions = missions,
                Vacations = vacations
            };
        }
    }


}