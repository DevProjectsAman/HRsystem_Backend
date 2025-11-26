using HRsystem.Api.Database;
using HRsystem.Api.Features.EmployeeDashboard.GetPendingActivities;
using HRsystem.Api.Features.EmployeeDashboard.ManagerActivity;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.ExceptionHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HRsystem.Api.Features.EmployeeDashboard.EmployeeApp
{
    public class CheckHistoryDto
    {
        public DateTime PunchDate { get; set; }

        public statues AttStatues { get; set; } // for know if ontime or late

        public DateTime? PunchTime { get; set; }

        public decimal? TotalHours { get; set; }


        [MaxLength(25)]
        public string PunchType { get; set; }
    }
    public record EmployeeFullDashboardQuery() : IRequest<EmployeeFullDashboardDto>;

    public class EmployeeFullDashboardDto
    {
        // 🧾 General Info
        public EmployeeInfoDto EmployeeInfo { get; set; }
        public EmployeeGetShiftDto ShiftInfo { get; set; }

        // 🏖️ Balances
        public EmployeeAnnualBalanceDto AnnualBalance { get; set; }
        public EmployeeCasualBalanceDto CasualBalance { get; set; }

        // 📊 Activities Status
        public ActivitiesStatusCountDto ActivityStatus { get; set; }


        public CheckHistoryDto  CheckHistory { get; set; }
        // 👨‍💼 Manager Info (if user manages others)
        public bool IsManager { get; set; }
        public RequestStatusesOfManagerDto? ManagerRequestsStatus { get; set; }
    }

    public class EmployeeFullDashboardHandler : IRequestHandler<EmployeeFullDashboardQuery, EmployeeFullDashboardDto>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;

        public EmployeeFullDashboardHandler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<EmployeeFullDashboardDto> Handle(EmployeeFullDashboardQuery request, CancellationToken ct)
        {
            var employeeId = _currentUser.EmployeeID;

            // ✅ 1. Get Employee Info
            var employee = await _db.TbEmployees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId, ct);

            if (employee == null)
                throw new NotFoundException("Employee not found", employeeId);

            var info = new EmployeeInfoDto
            {
                EmployeeName = employee.ArabicFullName,
                EmployeeId = employee.EmployeeId,
                EmployeeDepartement = _currentUser.UserLanguage == "ar"
                    ? employee.Department?.DepartmentName.ar
                    : employee.Department?.DepartmentName.en
            };

            // ✅ 2. Shift Info + Attendance Status
            var shift = await _db.TbShifts.FirstOrDefaultAsync(s => s.ShiftId == employee.ShiftId, ct);
            //if (shift == null)
            //    throw new NotFoundException("Shift not found", employee.ShiftId ?? 0);

            var now = TimeOnly.FromDateTime(DateTime.Now);
            var shiftInfo = new EmployeeGetShiftDto
            {
                StartTime = shift.StartTime,
                EndTime = shift.EndTime,
                GracePeriodMinutes = shift.GracePeriodMinutes,
                IsFlexible = shift.IsFlexible,
                MaxStartTime = shift.MaxStartTime,
                Statues = shift.IsFlexible == true
                    ? ((shift.MaxStartTime ?? new TimeOnly(0, 0)).AddMinutes(shift.GracePeriodMinutes) < now
                        ? statues.Late
                        : statues.OnTime)
                    : (shift.StartTime.AddMinutes(shift.GracePeriodMinutes) <= now
                        ? statues.Late
                        : statues.OnTime)
            };

            // ✅ 3. Vacation Balances
            var annualBalance = await _db.TbEmployeeVacationBalances
                .FirstOrDefaultAsync(b => b.EmployeeId == employeeId && b.VacationTypeId == 1, ct);
            var casualBalance = await _db.TbEmployeeVacationBalances
                .FirstOrDefaultAsync(b => b.EmployeeId == employeeId && b.VacationTypeId == 6, ct);

            var annualBalanceDto = new EmployeeAnnualBalanceDto
            {
                UsedBalance = annualBalance?.UsedDays ?? 0,
                RemainBalance = annualBalance?.RemainingDays ?? 0
            };

            var casualBalanceDto = new EmployeeCasualBalanceDto
            {
                UsedBalance = casualBalance?.UsedDays ?? 0,
                RemainBalance = casualBalance?.RemainingDays ?? 0
            };

            // ✅ 4. Employee Activity Stats
            const int ApprovedStatusId = 7;
            const int RejectedStatusId = 8;
            const int PendingStatusId = 10;

            var activityStatuses = await _db.TbEmployeeActivities
                .Where(a => a.EmployeeId == employeeId)
                .Select(a => a.StatusId)
                .ToListAsync(ct);

            var activityDto = new ActivitiesStatusCountDto
            {
                ApprovedCount = activityStatuses.Count(a => a == ApprovedStatusId),
                RejectedCount = activityStatuses.Count(a => a == RejectedStatusId),
                PendingCount = activityStatuses.Count(a => a == PendingStatusId)
            };

            // ✅ 5. Check if Manager + Get Managed Employees Requests
            var isManager = await _db.TbEmployees.AnyAsync(e => e.ManagerId == employeeId, ct);
            RequestStatusesOfManagerDto? managerRequests = null;

            if (isManager)
            {
                var managedRequests = await _db.TbEmployeeActivities
                    .Where(a => a.Employee.ManagerId == employeeId)
                    .Select(a => a.StatusId)
                    .ToListAsync(ct);

                managerRequests = new RequestStatusesOfManagerDto
                {
                    ApprovedCount = managedRequests.Count(a => a == ApprovedStatusId),
                    RejectedCount = managedRequests.Count(a => a == RejectedStatusId),
                    PendingCount = managedRequests.Count(a => a == PendingStatusId)
                };
            }

            var checkhistory = new CheckHistoryDto ();
            var today = DateTime.Now.Date;
            var activity = await _db.TbEmployeeActivities
                .FirstOrDefaultAsync(a => a.EmployeeId == employee.EmployeeId && a.RequestDate.Date == today && a.ActivityTypeId == 1, ct);

            if( activity == null)
            {
                //  there aren't any activites today 
            }
            else
            {
                        var attendance = await _db.TbEmployeeAttendances
                                .FirstOrDefaultAsync(a => a.ActivityId == activity.ActivityId && a.AttendanceDate == today, ct);
                     checkhistory = new CheckHistoryDto
                    {
                        AttStatues = attendance.AttStatues ,
                        PunchDate = attendance.AttendanceDate,
                        TotalHours = attendance.TotalHours,
                     };
                    var lastPunch = await _db.TbEmployeeAttendancePunches
                        .Where(p => p.AttendanceId == attendance.AttendanceId)
                        .OrderByDescending(p => p.PunchTime)
                        .FirstOrDefaultAsync(ct);
                    checkhistory.PunchTime = lastPunch?.PunchTime;
                    checkhistory.PunchType = lastPunch?.PunchType;
            }
            // ✅ Final DTO
            return new EmployeeFullDashboardDto
            {
                EmployeeInfo = info,
                ShiftInfo = shiftInfo,
                AnnualBalance = annualBalanceDto,
                CasualBalance = casualBalanceDto,
                ActivityStatus = activityDto,
                IsManager = isManager,
                ManagerRequestsStatus = managerRequests,
                CheckHistory = checkhistory
            };
        }
    }
}
