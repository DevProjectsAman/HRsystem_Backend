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
    public class EmployeeAttendanceReport
    {
        // ===================== QUERY =====================
        public record GetEmployeeAttendanceReportQuery(
            int? DepartmentId = null,
            DateTime? FromDate = null,
            DateTime? ToDate = null,
            int TopEmployeesCount = 5
        ) : IRequest<ResponseResultDTO<List<EmployeeAttendanceReportDto>>>;

        // ===================== VALIDATOR =====================
        public class GetEmployeeAttendanceReportQueryValidator
            : AbstractValidator<GetEmployeeAttendanceReportQuery>
        {
            public GetEmployeeAttendanceReportQueryValidator()
            {
                RuleFor(x => x.FromDate)
                    .LessThanOrEqualTo(x => x.ToDate)
                    .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
                    .WithMessage("FromDate must be before ToDate");
            }
        }

        // ===================== HANDLER =====================
        public class GetEmployeeAttendanceReportHandler
            : IRequestHandler<GetEmployeeAttendanceReportQuery, ResponseResultDTO<List<EmployeeAttendanceReportDto>>>
        {
            private readonly DBContextHRsystem _db;

            public GetEmployeeAttendanceReportHandler(DBContextHRsystem db)
            {
                _db = db;
            }

            public async Task<ResponseResultDTO<List<EmployeeAttendanceReportDto>>> Handle(
                GetEmployeeAttendanceReportQuery request,
                CancellationToken cancellationToken)
            {
                try
                {
                    var fromDate = request.FromDate?.Date ?? DateTime.Today;
                    var toDate = request.ToDate?.Date ?? DateTime.Today;
                    var totalDays = (toDate - fromDate).Days + 1;

                    // ===== Employees =====
                    var employeesQuery = _db.TbEmployees
                        .Include(e => e.Department)
                        .Where(e => e.Department != null)
                        .AsQueryable();

                    if (request.DepartmentId.HasValue)
                        employeesQuery = employeesQuery
                            .Where(e => e.DepartmentId == request.DepartmentId);

                    var employees = await employeesQuery.ToListAsync(cancellationToken);

                    // ===== Attendance =====
                    var attendanceReports = await _db.TbEmployeeMonthlyReports
                        .Where(a => a.Date >= fromDate && a.Date <= toDate)
                        .ToListAsync(cancellationToken);

                    var attendanceStatuses = await _db.TbAttendanceStatues
                        .ToListAsync(cancellationToken);

                    // ===== Employee-level report =====
                    var employeeReports = employees.Select(emp =>
                    {
                        var empReports = attendanceReports
                            .Where(a => a.EmployeeId == emp.EmployeeId)
                            .ToList();

                        var empStatuses = attendanceStatuses.Select(s =>
                        {
                            int count = empReports.Count(a => a.EmployeeTodayStatuesId == s.AttendanceStatuesId);
                            double percent = totalDays == 0 ? 0 : Math.Round((double)count / totalDays * 100, 2);

                            return new EmployeeAttendanceStatusDto
                            {
                                StatusCode = s.AttendanceStatuesCode,
                                StatusName = s.AttendanceStatuesName?.en ?? s.AttendanceStatuesCode,
                                Percentage = percent
                            };
                        }).ToList();

                        return new EmployeeAttendanceReportDto
                        {
                            EmployeeId = emp.EmployeeId,
                            EmployeeName = emp.EnglishFullName,
                            DepartmentId = emp.DepartmentId,
                            DepartmentName = emp.Department.DepartmentName?.en ?? "Unknown",
                            Statuses = empStatuses
                        };
                    })
                    // ترتيب حسب أكتر نسبة absent
                    .OrderByDescending(e => e.Statuses.FirstOrDefault(s => s.StatusCode == "absent")?.Percentage ?? 0)
                    .Take(request.TopEmployeesCount)
                    .ToList();

                    return new ResponseResultDTO<List<EmployeeAttendanceReportDto>>
                    {
                        Success = true,
                        Data = employeeReports,
                        Message = "Employee attendance report loaded successfully"
                    };
                }
                catch (Exception ex)
                {
                    return new ResponseResultDTO<List<EmployeeAttendanceReportDto>>
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }
            }
        }
    }
}

