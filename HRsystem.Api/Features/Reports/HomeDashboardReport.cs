
using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Features.Reports.DTO;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HRsystem.Api.Features.Reports
{
    public class HomeDashboardReport
    {
        // ====== QUERY ======
        public record GetDashboardReportQuery : IRequest<ResponseResultDTO<HomeDashboardReportDto>>;

        // ====== VALIDATOR ======
        public class GetDashboardReportQueryValidator : AbstractValidator<GetDashboardReportQuery>
        {
            public GetDashboardReportQueryValidator() { }
        }

        // ====== HANDLER ======
        public class GetDashboardReportHandler
            : IRequestHandler<GetDashboardReportQuery, ResponseResultDTO<HomeDashboardReportDto>>
        {
            private readonly DBContextHRsystem _db;

            public GetDashboardReportHandler(DBContextHRsystem db)
            {
                _db = db;
            }

            public async Task<ResponseResultDTO<HomeDashboardReportDto>> Handle(
                GetDashboardReportQuery request,
                CancellationToken cancellationToken)
            {
                try
                {
                    // ===== Employees =====
                    var totalEmployees = await _db.TbEmployees.CountAsync(cancellationToken);
                    var activeEmployees = await _db.TbEmployees.CountAsync(e => e.Status == "1", cancellationToken);
                    var inactiveEmployees = totalEmployees - activeEmployees;

                    // ===== Departments & Companies =====
                    var totalDepartments = await _db.TbDepartments.CountAsync(cancellationToken);
                    var totalCompanies = await _db.TbCompanies.CountAsync(cancellationToken);

                    // ===== Requests =====
                    var totalRequests = await _db.TbEmployeeActivities.CountAsync(cancellationToken);
                    var totalApprovedRequests = await _db.TbEmployeeActivities.CountAsync(r => r.StatusId == 7, cancellationToken);
                    var totalPendingRequests = await _db.TbEmployeeActivities.CountAsync(r => r.StatusId == 8, cancellationToken);

                    // ===== Today Attendance =====
                    var todayStart = new DateTime(2025, 10, 30);
                    var todayEnd = todayStart.AddDays(1);

                    var attendanceToday = await _db.TbEmployeeMonthlyReports
                        .Where(a => a.Date >= todayStart && a.Date < todayEnd)
                        .ToListAsync(cancellationToken);

                    var attendanceStatuses = await _db.TbAttendanceStatues.ToListAsync(cancellationToken);

                    var employees = await _db.TbEmployees
                        .Include(e => e.Department)
                        .Where(e => e.Department != null)
                        .ToListAsync(cancellationToken);

                    // ===== Group by Department =====
                    var departmentGroups = employees
                        .GroupBy(e => new { e.DepartmentId, e.Department.DepartmentName })
                        .ToList();

                    var todayDepartmentStatus = departmentGroups.Select(g =>
                    {
                        var deptEmployees = g.ToList();
                        int totalDeptEmployees = deptEmployees.Count;

                        // ===== Calculate percentages for each status =====
                        var statuses = attendanceStatuses.Select(s =>
                        {
                            int count = attendanceToday.Count(a =>
                                deptEmployees.Any(e => e.EmployeeId == a.EmployeeId) &&
                                a.EmployeeTodayStatuesId == s.AttendanceStatuesId
                            );

                            double percent = totalDeptEmployees == 0 ? 0 :
                                Math.Round((double)count / totalDeptEmployees * 100, 2);

                            return new DepartmentAttendanceStatusDto
                            {
                                StatusCode = s.AttendanceStatuesCode,
                                StatusName = s.AttendanceStatuesName?.en ?? s.AttendanceStatuesCode,
                                Percentage = percent
                            };
                        }).ToList();

                        return new DepartmentAttendanceDto
                        {
                            DepartmentId = g.Key.DepartmentId,
                            DepartmentName = g.Key.DepartmentName?.en ?? "Unknown",
                            Statuses = statuses
                        };
                    }).ToList();

                    // ===== DTO =====
                    var dto = new HomeDashboardReportDto
                    {
                        TotalEmployees = totalEmployees,
                        ActiveEmployees = activeEmployees,
                        InactiveEmployees = inactiveEmployees,
                        TotalDepartments = totalDepartments,
                        TotalCompanies = totalCompanies,
                        TotalRequests = totalRequests,
                        TotalApprovedRequests = totalApprovedRequests,
                        TotalPendingRequests = totalPendingRequests,
                        TodayDepartmentStatus = todayDepartmentStatus
                    };

                    return new ResponseResultDTO<HomeDashboardReportDto>
                    {
                        Success = true,
                        Data = dto,
                        Message = "Dashboard report loaded successfully"
                    };
                }
                catch (Exception ex)
                {
                    return new ResponseResultDTO<HomeDashboardReportDto>
                    {
                        Success = false,
                        Message = $"Failed to load dashboard report: {ex.Message}"
                    };
                }
            }
        }
    }
}
