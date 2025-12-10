using Google.Cloud.Iam.V1;
using HRsystem.Api.Database;
using HRsystem.Api.Features.EmployeeDashboard.GetAllActivities;
using HRsystem.Api.Features.EmployeeDashboard.GetPendingActivities;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.EmployeeDashboard.GetApprovedActivites
{
    public record GetApprovedActivitiesQueury() : IRequest<List<ApprovedActivityDto>>;

    public class ApprovedActivityDto
    {
        public long ActivityId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string StatusType { get; set; }
        public string RequestType { get; set; }   // vacation | mission | excuse
        public string ActivityName { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateTime CreatedAt { get; set; }
        public string EmployeeName { get; set; }
        public string? Location { get; set; }     // missions only
        public string? Notes { get; set; }
    }

    public class GetApprovedActivitiesQueuryHandler : IRequestHandler<GetApprovedActivitiesQueury, List<ApprovedActivityDto>>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUserService;
        public GetApprovedActivitiesQueuryHandler(DBContextHRsystem db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }
        public async Task<List<ApprovedActivityDto>> Handle(GetApprovedActivitiesQueury request, CancellationToken ct)
        {
            var employeeId = _currentUserService.EmployeeID;
            var language = _currentUserService.UserLanguage;
            var lastMonthDate = DateTime.UtcNow.AddDays(-30);
            const int ApprovedStatusId = 7; // غيّر الرقم حسب الـ StatusId بتاع الـ Pending عندك

            var activities = await _db.TbEmployeeActivities
                .Include(a => a.Status)
                .Include(a => a.Employee)
                .Include(a => a.ActivityType)
                .Include(a => a.TbEmployeeVacations)
                .Include(a => a.TbEmployeeMissions)
                .Include(a => a.TbEmployeeExcuses)
                .Where(a =>
                    a.EmployeeId == employeeId &&
                    a.RequestDate >= lastMonthDate &&
                    (a.StatusId == ApprovedStatusId) &&
                    a.ActivityTypeId != 1
                )
                .Select(a => new
                {
                    a.ActivityId,
                    a.ActivityTypeId,
                    EmployeeName = a.Employee.EnglishFullName,
                    ActivityName = language == "ar"
                        ? a.ActivityType.ActivityName.ar
                        : a.ActivityType.ActivityName.en,
                    StatusName = language == "ar"
                        ? a.Status.StatusName.ar
                        : a.Status.StatusName.en,
                    StatusId = a.StatusId,
                    CreatedAt = a.RequestDate,
                    Vacations = a.TbEmployeeVacations,
                    Missions = a.TbEmployeeMissions,
                    Excuses = a.TbEmployeeExcuses
                })
                .ToListAsync(ct);

            var result = activities.Select(a =>
            {
                DateTime from = default;
                DateTime to = default;
                string requestType = "";
                string? location = null;
                string? Notes = null;

                switch (a.ActivityTypeId)
                {
                    case 5: // Vacation
                        var v = a.Vacations.FirstOrDefault();
                        requestType = "vacation";
                        from = v.StartDate.ToDateTime(TimeOnly.MinValue);
                        to = v.EndDate.ToDateTime(TimeOnly.MinValue);
                        Notes = v.Notes;
                        break;

                    case 4: // Mission
                        var m = a.Missions.FirstOrDefault();
                        requestType = "mission";
                        from = m.StartDatetime.Date;
                        to = m.EndDatetime.Date;
                        location = m.MissionLocation;
                        Notes = m.MissionReason;
                        break;

                    case 6: // Excuse
                        var e = a.Excuses.FirstOrDefault();
                        requestType = "excuse";
                        from = DateTime.Today.Add(e.StartTime.ToTimeSpan());
                        to = DateTime.Today.Add(e.EndTime.ToTimeSpan());
                        Notes = e.ExcuseReason;
                        break;
                }

                return new ApprovedActivityDto
                {
                    ActivityId = a.ActivityId,
                    StatusId = a.StatusId,
                    StatusName = a.StatusName,
                    StatusType = a.StatusName,
                    RequestType = requestType,
                    ActivityName = a.ActivityName,
                    From = from,
                    To = to,
                    CreatedAt = a.CreatedAt,
                    EmployeeName = a.EmployeeName,
                    Location = location,
                    Notes= Notes
                };
            })
            .OrderByDescending(x => x.CreatedAt)
            .ToList();

            return result; // <-- this was missing
            //var activities = await _db.TbEmployeeActivities
            //                         .Include(a => a.Status)
            //                         .Include(a => a.Employee)
            //                         .Include(a => a.ActivityType)
            //                         .Where(a => a.EmployeeId == employeeId
            //                                  && a.StatusId == ApprovedStatusId
            //                                  && a.RequestDate >= lastMonthDate
            //                                  && a.ActivityTypeId != 1)
            //                         .Select(a => new
            //                         {
            //                             a.ActivityId,
            //                             EmployeeName = a.Employee.EnglishFullName,
            //                             ActivityName = language == "ar"
            //                                 ? a.ActivityType.ActivityName.ar
            //                                 : a.ActivityType.ActivityName.en,
            //                             StatusName = language == "ar"
            //                                 ? a.Status.StatusName.ar
            //                                 : a.Status.StatusName.en,
            //                             CreatedAt = a.RequestDate
            //                         })
            //                         .ToListAsync(ct);

            //// Compute 'From' in C# safely
            //var result = activities.Select(a => new ApprovedActivityDto
            //{
            //    ActivityId = a.ActivityId,
            //    EmployeeName = a.EmployeeName,
            //    ActivityName = a.ActivityName,
            //    StatusName = a.StatusName,
            //    CreatedAt = a.CreatedAt,
            //    Duration = (int)(DateTime.UtcNow - a.CreatedAt).TotalDays == 0
            //        ? "Today"
            //        : (int)(DateTime.UtcNow - a.CreatedAt).TotalDays == 1
            //            ? "1 day"
            //            : ((int)(DateTime.UtcNow - a.CreatedAt).TotalDays) + " days"
            //})
            //.OrderByDescending(x => x.CreatedAt)
            //.ToList();

            //return result;

        }
    }
}

 
