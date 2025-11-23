
//using FluentValidation;
//using HRsystem.Api.Database;
//using HRsystem.Api.Features.Reports.DTO;
//using HRsystem.Api.Shared.DTO;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace HRsystem.Api.Features.Reports
//{
//    public class HomeDashboardReport
//    {
//        // ====== QUERY ======
//        //public record GetDashboardReportQuery : IRequest<ResponseResultDTO<HomeDashboardReportDto>>;
//        public record GetDashboardReportQuery(
//        int? DepartmentId = null,       
//        DateTime? FromDate = null,      
//        DateTime? ToDate = null,    
//        int TopEmployeesCount = 5  
//       ): IRequest<ResponseResultDTO<HomeDashboardReportDto>>;

//        // ====== VALIDATOR ======
//        public class GetDashboardReportQueryValidator : AbstractValidator<GetDashboardReportQuery>
//        {
//            public GetDashboardReportQueryValidator() { }
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

//            public async Task<ResponseResultDTO<HomeDashboardReportDto>> Handle(
//                GetDashboardReportQuery request,
//                CancellationToken cancellationToken)
//            {
//                try
//                {
//                    // ===== Employees =====
//                    var totalEmployees = await _db.TbEmployees.CountAsync(cancellationToken);
//                    var activeEmployees = await _db.TbEmployees.CountAsync(e => e.Status == "1", cancellationToken);
//                    var inactiveEmployees = totalEmployees - activeEmployees;

//                    // ===== Departments & Companies =====
//                    var totalDepartments = await _db.TbDepartments.CountAsync(cancellationToken);
//                    var totalCompanies = await _db.TbCompanies.CountAsync(cancellationToken);

//                    // ===== Requests =====
//                    var totalRequests = await _db.TbEmployeeActivities.CountAsync(cancellationToken);
//                    var totalApprovedRequests = await _db.TbEmployeeActivities.CountAsync(r => r.StatusId == 7, cancellationToken);
//                    var totalPendingRequests = await _db.TbEmployeeActivities.CountAsync(r => r.StatusId == 8, cancellationToken);

//                    // ===== Today Attendance =====
//                    var todayStart = new DateTime(2025, 10, 30);
//                    var todayEnd = todayStart.AddDays(1);

//                    var attendanceToday = await _db.TbEmployeeMonthlyReports
//                        .Where(a => a.Date >= todayStart && a.Date < todayEnd)
//                        .ToListAsync(cancellationToken);

//                    var attendanceStatuses = await _db.TbAttendanceStatues.ToListAsync(cancellationToken);

//                    var employees = await _db.TbEmployees
//                        .Include(e => e.Department)
//                        .Where(e => e.Department != null)
//                        .ToListAsync(cancellationToken);

//                    // ===== Group by Department =====
//                    var departmentGroups = employees
//                        .GroupBy(e => new { e.DepartmentId, e.Department.DepartmentName })
//                        .ToList();

//                    var todayDepartmentStatus = departmentGroups.Select(g =>
//                    {
//                        var deptEmployees = g.ToList();
//                        int totalDeptEmployees = deptEmployees.Count;

//                        // ===== Calculate percentages for each status =====
//                        var statuses = attendanceStatuses.Select(s =>
//                        {
//                            int count = attendanceToday.Count(a =>
//                                deptEmployees.Any(e => e.EmployeeId == a.EmployeeId) &&
//                                a.EmployeeTodayStatuesId == s.AttendanceStatuesId
//                            );

//                            double percent = totalDeptEmployees == 0 ? 0 :
//                                Math.Round((double)count / totalDeptEmployees * 100, 2);

//                            return new DepartmentAttendanceStatusDto
//                            {
//                                StatusCode = s.AttendanceStatuesCode,
//                                StatusName = s.AttendanceStatuesName?.en ?? s.AttendanceStatuesCode,
//                                Percentage = percent
//                            };
//                        }).ToList();

//                        return new DepartmentAttendanceDto
//                        {
//                            DepartmentId = g.Key.DepartmentId,
//                            DepartmentName = g.Key.DepartmentName?.en ?? "Unknown",
//                            Statuses = statuses
//                        };
//                    }).ToList();

//                    // ===== DTO =====
//                    var dto = new HomeDashboardReportDto
//                    {
//                        TotalEmployees = totalEmployees,
//                        ActiveEmployees = activeEmployees,
//                        InactiveEmployees = inactiveEmployees,
//                        TotalDepartments = totalDepartments,
//                        TotalCompanies = totalCompanies,
//                        TotalRequests = totalRequests,
//                        TotalApprovedRequests = totalApprovedRequests,
//                        TotalPendingRequests = totalPendingRequests,
//                        TodayDepartmentStatus = todayDepartmentStatus
//                    };

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
        public record GetDashboardReportQuery(
            int? DepartmentId = null,
            string FromDay = null,   // "yyyy-MM-dd"
            string ToDay = null,     // "yyyy-MM-dd"
            int TopEmployeesCount = 5
        ) : IRequest<ResponseResultDTO<HomeDashboardReportDto>>;

        public class GetDashboardReportQueryValidator : AbstractValidator<GetDashboardReportQuery>
        {
            public GetDashboardReportQueryValidator()
            {
                RuleFor(x => x.FromDay)
                    .Must(d => DateTime.TryParseExact(d, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _))
                    .When(x => !string.IsNullOrEmpty(x.FromDay))
                    .WithMessage("FromDay must be a valid date in yyyy-MM-dd format");

                RuleFor(x => x.ToDay)
                    .Must(d => DateTime.TryParseExact(d, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _))
                    .When(x => !string.IsNullOrEmpty(x.ToDay))
                    .WithMessage("ToDay must be a valid date in yyyy-MM-dd format");
            }
        }

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
                    // تحويل الـ strings لـ DateTime
                    var fromDate = string.IsNullOrEmpty(request.FromDay)
                        ? DateTime.Today
                        : DateTime.ParseExact(request.FromDay, "yyyy-MM-dd", null);

                    var toDate = string.IsNullOrEmpty(request.ToDay)
                        ? DateTime.Today
                        : DateTime.ParseExact(request.ToDay, "yyyy-MM-dd", null);

                    if (toDate < fromDate)
                        return new ResponseResultDTO<HomeDashboardReportDto>
                        {
                            Success = false,
                            Message = "ToDay cannot be earlier than FromDay"
                        };

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

                    // ===== Attendance =====
                    var attendanceReports = await _db.TbEmployeeMonthlyReports
                        .Where(a => a.Date.Date >= fromDate.Date && a.Date.Date <= toDate.Date)
                        .ToListAsync(cancellationToken);

                    var attendanceStatuses = await _db.TbAttendanceStatues.ToListAsync(cancellationToken);

                    var employeesQuery = _db.TbEmployees.Include(e => e.Department).AsQueryable();
                    if (request.DepartmentId.HasValue)
                        employeesQuery = employeesQuery.Where(e => e.DepartmentId == request.DepartmentId);

                    var employees = await employeesQuery.ToListAsync(cancellationToken);

                    // ===== Group by Department =====
                    var departmentGroups = employees
                        .GroupBy(e => new { e.DepartmentId, e.Department.DepartmentName })
                        .ToList();

                    var totalDaysInPeriod = (toDate - fromDate).Days + 1;

                    var todayDepartmentStatus = departmentGroups.Select(g =>
                    {
                        var deptEmployees = g.ToList();
                        int totalDeptEmployees = deptEmployees.Count;

                        // ===== Department level statuses =====
                        var deptStatuses = attendanceStatuses.Select(s =>
                        {
                            int count = attendanceReports.Count(a =>
                                deptEmployees.Any(e => e.EmployeeId == a.EmployeeId) &&
                                a.EmployeeTodayStatuesId == s.AttendanceStatuesId
                            );

                            double percent = totalDeptEmployees * totalDaysInPeriod == 0 ? 0 :
                                Math.Round((double)count / (totalDeptEmployees * totalDaysInPeriod) * 100, 2);

                            return new DepartmentAttendanceStatusDto
                            {
                                StatusCode = s.AttendanceStatuesCode,
                                StatusName = s.AttendanceStatuesName?.en ?? s.AttendanceStatuesCode,
                                Percentage = percent
                            };
                        }).ToList();

                        // ===== Employee level =====
                        var employeesWithStatus = deptEmployees.Select(emp =>
                        {
                            var empReports = attendanceReports
                                .Where(a => a.EmployeeId == emp.EmployeeId)
                                .ToList();

                            var empStatuses = attendanceStatuses.Select(s =>
                            {
                                int count = empReports.Count(a => a.EmployeeTodayStatuesId == s.AttendanceStatuesId);
                                double percent = totalDaysInPeriod == 0 ? 0 : Math.Round((double)count / totalDaysInPeriod * 100, 2);

                                return new DepartmentAttendanceStatusDto
                                {
                                    StatusCode = s.AttendanceStatuesCode,
                                    StatusName = s.AttendanceStatuesName?.en ?? s.AttendanceStatuesCode,
                                    Percentage = percent
                                };
                            }).ToList();

                            return new EmployeeAttendanceStatusDto
                            {
                                EmployeeId = emp.EmployeeId,
                                EmployeeName = emp.EnglishFullName,
                                DepartmentId = emp.DepartmentId,
                                DepartmentName = emp.Department.DepartmentName?.en ?? "Unknown",
                                Statuses = empStatuses
                            };
                        })
                        .OrderByDescending(e => e.Statuses.FirstOrDefault(s => s.StatusCode == "absent")?.Percentage ?? 0)
                        .Take(request.TopEmployeesCount)
                        .ToList();

                        return new DepartmentAttendanceDto
                        {
                            DepartmentId = g.Key.DepartmentId,
                            DepartmentName = g.Key.DepartmentName?.en ?? "Unknown",
                            Statuses = deptStatuses,
                            Employees = employeesWithStatus
                        };
                    }).ToList();

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
