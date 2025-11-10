//using FluentValidation;
//using HRsystem.Api.Database;
//using HRsystem.Api.Shared.DTO;
//using HRsystem.Api.Features.Reports.DTO;
//using MediatR;
//using Microsoft.EntityFrameworkCore;

//namespace HRsystem.Api.Features.Reports
//{
//    public class HomeDashboardReport
//    {
//        // ====== QUERY ======
//        public record GetDashboardReportQuery : IRequest<ResponseResultDTO<HomeDashboardReportDto>>;

//        // ====== VALIDATOR ======
//        public class GetDashboardReportQueryValidator : AbstractValidator<GetDashboardReportQuery>
//        {
//            public GetDashboardReportQueryValidator()
//            {
//                // Add validation rules here if needed
//            }
//        }

//        // ====== HANDLER ======
//        public class GetDashboardReportHandler
//            : IRequestHandler<GetDashboardReportQuery, ResponseResultDTO<HomeDashboardReportDto>>
//        {
//            private readonly DBContextHRsystem _db;

//            public GetDashboardReportHandler(DBContextHRsystem db)
//            {
//                _db = db;
//            }

//            public async Task<ResponseResultDTO<HomeDashboardReportDto>> Handle(GetDashboardReportQuery request, CancellationToken cancellationToken)
//            {
//                try
//                {
//                    // === Employees ===
//                    var totalEmployees = await _db.TbEmployees.CountAsync(cancellationToken);
//                    var activeEmployees = await _db.TbEmployees.CountAsync(e => e.Status == "1", cancellationToken);
//                    var inactiveEmployees = totalEmployees - activeEmployees;

//                    // === Departments & Companies ===
//                    var totalDepartments = await _db.TbDepartments.CountAsync(cancellationToken);
//                    var totalCompanies = await _db.TbCompanies.CountAsync(cancellationToken);

//                    // === Requests ===
//                    var totalRequests = await _db.TbEmployeeActivities.CountAsync(cancellationToken);
//                    var totalApprovedRequests = await _db.TbEmployeeActivities.CountAsync(r => r.StatusId == 7, cancellationToken);
//                    var totalPendingRequests = await _db.TbEmployeeActivities.CountAsync(r => r.StatusId == 8, cancellationToken);

//                    // === DTO Mapping ===
//                    var dto = new HomeDashboardReportDto
//                    {
//                        TotalEmployees = totalEmployees,
//                        ActiveEmployees = activeEmployees,
//                        InactiveEmployees = inactiveEmployees,
//                        TotalDepartments = totalDepartments,
//                        TotalCompanies = totalCompanies,
//                        TotalRequests = totalRequests,
//                        TotalApprovedRequests = totalApprovedRequests,
//                        TotalPendingRequests = totalPendingRequests
//                    };

//                    // === Return Result ===
//                    return new ResponseResultDTO<HomeDashboardReportDto>
//                    {
//                        Success = true,
//                        Data = dto,
//                        Message = "Dashboard report loaded successfully"
//                    };
//                }
//                catch (Exception ex)
//                {
//                    return new ResponseResultDTO<HomeDashboardReportDto>
//                    {
//                        Success = false,
//                        Message = $"Failed to load dashboard report: {ex.Message}"
//                    };
//                }
//            }
//        }
//    }
//}


using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Shared.DTO;
using HRsystem.Api.Features.Reports.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Reports
{
    public class HomeDashboardReport
    {
        // ====== QUERY ======
        public record GetDashboardReportQuery : IRequest<ResponseResultDTO<HomeDashboardReportDto>>;

        // ====== VALIDATOR ======
        public class GetDashboardReportQueryValidator : AbstractValidator<GetDashboardReportQuery>
        {
            public GetDashboardReportQueryValidator()
            {
                // Add validation rules here if needed
            }
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

            public async Task<ResponseResultDTO<HomeDashboardReportDto>> Handle(GetDashboardReportQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    // === Employees ===
                    var totalEmployees = await _db.TbEmployees.CountAsync(cancellationToken);
                    var activeEmployees = await _db.TbEmployees.CountAsync(e => e.Status == "1", cancellationToken);
                    var inactiveEmployees = totalEmployees - activeEmployees;

                    // === Departments & Companies ===
                    var totalDepartments = await _db.TbDepartments.CountAsync(cancellationToken);
                    var totalCompanies = await _db.TbCompanies.CountAsync(cancellationToken);

                    // === Requests ===
                    var totalRequests = await _db.TbEmployeeActivities.CountAsync(cancellationToken);
                    var totalApprovedRequests = await _db.TbEmployeeActivities.CountAsync(r => r.StatusId == 7, cancellationToken);
                    var totalPendingRequests = await _db.TbEmployeeActivities.CountAsync(r => r.StatusId == 8, cancellationToken);

                    // === Today Attendance by Department (Percent) ===
                    var today = DateTime.UtcNow.Date;

                    // كل سجلات الغياب/حضور اليوم
                    var attendanceToday = await _db.TbEmployeeMonthlyReports
                        .Where(a => a.Date.Date == today)
                        .ToListAsync(cancellationToken);

                    var employees = await _db.TbEmployees
                        .Include(e => e.Department)
                        .ToListAsync(cancellationToken);

                    var departmentGroups = employees
                        .GroupBy(e => new { e.DepartmentId, e.Department.DepartmentName })
                        .ToList();

                    // حساب النسب لكل إدارة
                    var todayDepartmentStatus = departmentGroups.Select(g =>
                    {
                        var deptEmployees = g.ToList();
                        var deptAttendance = attendanceToday.Count(a =>
                            deptEmployees.Any(e => e.EmployeeId == a.EmployeeId && a.EmployeeTodayStatuesId != 2)); // الحضور
                        var deptAbsence = attendanceToday.Count(a =>
                            deptEmployees.Any(e => e.EmployeeId == a.EmployeeId && a.EmployeeTodayStatuesId == 2)); // الغياب

                        var totalDept = deptAttendance + deptAbsence;

                        double attendancePercent = totalDept == 0 ? 0 : Math.Round((double)deptAttendance / totalDept * 100, 2);
                        double absencePercent = totalDept == 0 ? 0 : Math.Round((double)deptAbsence / totalDept * 100, 2);

                        return new DepartmentAttendanceDto
                        {
                            DepartmentId = g.Key.DepartmentId,
                            DepartmentName = g.Key.DepartmentName?.en ?? "",
                            AttendancePercentage = attendancePercent,
                            AbsencePercentage = absencePercent
                        };
                    }).ToList();

                    // === DTO Mapping ===
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

                    // === Return Result ===
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