

//using HRsystem.Api.Database.DataTables;
//using HRsystem.Api.Database;
//using HRsystem.Api.Features.Reports.DTO;
//using HRsystem.Api.Services.CurrentUser;
//using HRsystem.Api.Shared.DTO;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using FluentValidation;

//namespace HRsystem.Api.Features.Reports
//{
//    public class EmployeeActivityReportByManager
//    {
//        // ===== Query =====
//        public record GetEmployeeActivityReportByManagerQuery(
//            DateTime? FromDate = null,
//            DateTime? ToDate = null
//        ) : IRequest<ResponseResultDTO<EmployeeActivityReportByManagerDto>>;

//        // ===== Validator =====
//        public class GetEmployeeActivityReportByManagerQueryValidator
//            : AbstractValidator<GetEmployeeActivityReportByManagerQuery>
//        {
//            public GetEmployeeActivityReportByManagerQueryValidator()
//            {
//                RuleFor(x => x.FromDate)
//                    .LessThanOrEqualTo(x => x.ToDate)
//                    .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
//                    .WithMessage("FromDate must be before ToDate");
//            }
//        }

//        // ===== Handler =====
//        public class GetEmployeeActivityReportByManagerQueryHandler
//            : IRequestHandler<GetEmployeeActivityReportByManagerQuery, ResponseResultDTO<EmployeeActivityReportByManagerDto>>
//        {
//            private readonly DBContextHRsystem _db;
//            private readonly ICurrentUserService _currentUser;

//            public GetEmployeeActivityReportByManagerQueryHandler(DBContextHRsystem db, ICurrentUserService currentUser)
//            {
//                _db = db;
//                _currentUser = currentUser;
//            }

//            public async Task<ResponseResultDTO<EmployeeActivityReportByManagerDto>> Handle(
//                GetEmployeeActivityReportByManagerQuery request,
//                CancellationToken cancellationToken)
//            {
//                try
//                {
//                    var managerId = _currentUser.EmployeeID;
//                    var fromDate = request.FromDate?.Date ?? DateTime.Today;
//                    var toDate = request.ToDate?.Date ?? DateTime.Today;

//                    // ===== employees under manager =====
//                    var employees = await _db.TbEmployees
//                        .Where(e => e.ManagerId == managerId && e.IsActive)
//                        .ToListAsync(cancellationToken);

//                    if (!employees.Any())
//                        return new ResponseResultDTO<EmployeeActivityReportByManagerDto>
//                        {
//                            Success = true,
//                            Data = new EmployeeActivityReportByManagerDto()
//                        };

//                    var employeeIds = employees.Select(e => e.EmployeeId).ToList();

//                    // ===== Get activities in range =====
//                    var activities = await _db.TbEmployeeActivities
//                        .Include(a => a.Employee)
//                        .Include(a => a.ActivityType)
//                        .Where(a => employeeIds.Contains(a.EmployeeId)
//                                 && a.RequestDate.Date >= fromDate
//                                 && a.RequestDate.Date <= toDate)
//                        .ToListAsync(cancellationToken);

//                    // ===== Build rows per employee per day =====
//                    var rows = new List<EmployeeActivityRowDto>();
//                    var summaryDict = new Dictionary<string, int>();

//                    for (var date = fromDate; date <= toDate; date = date.AddDays(1))
//                    {
//                        foreach (var e in employees)
//                        {
//                            var act = activities.FirstOrDefault(a => a.EmployeeId == e.EmployeeId && a.RequestDate.Date == date);
//                            if (act != null)
//                            {
//                                // ===== Activity exists =====
//                                rows.Add(new EmployeeActivityRowDto
//                                {
//                                    DayId = null,
//                                    Date = act.RequestDate,
//                                    EmployeeId = e.EmployeeId,
//                                    EnglishFullName = e.EnglishFullName,
//                                    ArabicFullName = e.ArabicFullName,
//                                    ContractTypeId = e.ContractTypeId,
//                                    EmployeeCodeFinance = e.EmployeeCodeFinance,
//                                    EmployeeCodeHr = e.EmployeeCodeHr,
//                                    JobTitleId = e.JobTitleId,
//                                    JobLevelId = e.JobLevelId,
//                                    ManagerId = e.ManagerId,
//                                    CompanyId = e.CompanyId,
//                                    DepartmentId = e.DepartmentId,
//                                    ShiftId = e.ShiftId,
//                                    WorkDaysId = e.WorkDaysId,
//                                    RemoteWorkDaysId = e.RemoteWorkDaysId,
//                                    ActivityId = act.ActivityId,
//                                    ActivityTypeId = act.ActivityTypeId,
//                                    EmployeeTodayStatuesId = 0,
//                                    RequestBy = act.RequestBy,
//                                    ApprovedBy = act.ApprovedBy,
//                                    RequestDate = act.RequestDate,
//                                    ApprovedDate = act.ApprovedDate,
//                                    AttendanceId = null,
//                                    AttendanceDate = null,
//                                    FirstPuchin = null,
//                                    AttStatues = null,
//                                    LastPuchout = null,
//                                    TotalHours = null,
//                                    ActualWorkingHours = null,
//                                    IsHoliday = false,
//                                    IsWorkday = true,
//                                    IsRemoteday = false,
//                                    TodayStatues = act.ActivityType.ActivityCode,
//                                    Details = null
//                                });

//                                if (!summaryDict.ContainsKey(act.ActivityType.ActivityCode))
//                                    summaryDict[act.ActivityType.ActivityCode] = 0;
//                                summaryDict[act.ActivityType.ActivityCode]++;
//                            }
//                            else
//                            {
//                                // ===== Absent =====
//                                rows.Add(new EmployeeActivityRowDto
//                                {
//                                    DayId = null,
//                                    Date = date,
//                                    EmployeeId = e.EmployeeId,
//                                    EnglishFullName = e.EnglishFullName,
//                                    ArabicFullName = e.ArabicFullName,
//                                    ContractTypeId = e.ContractTypeId,
//                                    EmployeeCodeFinance = e.EmployeeCodeFinance,
//                                    EmployeeCodeHr = e.EmployeeCodeHr,
//                                    JobTitleId = e.JobTitleId,
//                                    JobLevelId = e.JobLevelId,
//                                    ManagerId = e.ManagerId,
//                                    CompanyId = e.CompanyId,
//                                    DepartmentId = e.DepartmentId,
//                                    ShiftId = e.ShiftId,
//                                    WorkDaysId = e.WorkDaysId,
//                                    RemoteWorkDaysId = e.RemoteWorkDaysId,
//                                    ActivityId = null,
//                                    ActivityTypeId = null,
//                                    EmployeeTodayStatuesId = 0,
//                                    RequestBy = null,
//                                    ApprovedBy = null,
//                                    RequestDate = null,
//                                    ApprovedDate = null,
//                                    AttendanceId = null,
//                                    AttendanceDate = null,
//                                    FirstPuchin = null,
//                                    AttStatues = null,
//                                    LastPuchout = null,
//                                    TotalHours = null,
//                                    ActualWorkingHours = null,
//                                    IsHoliday = false,
//                                    IsWorkday = true,
//                                    IsRemoteday = false,
//                                    TodayStatues = "ABSENT",
//                                    Details = null
//                                });

//                                if (!summaryDict.ContainsKey("ABSENT"))
//                                    summaryDict["ABSENT"] = 0;
//                                summaryDict["ABSENT"]++;
//                            }
//                        }
//                    }

//                    // ===== Build summary =====
//                    var summary = summaryDict.Select(kvp => new ActivityTypeCountDto
//                    {
//                        ActivityTypeCode = kvp.Key,
//                        ActivityTypeName = kvp.Key,
//                        Count = kvp.Value
//                    }).ToList();

//                    return new ResponseResultDTO<EmployeeActivityReportByManagerDto>
//                    {
//                        Success = true,
//                        Data = new EmployeeActivityReportByManagerDto
//                        {
//                            Rows = rows,
//                            Summary = summary
//                        }
//                    };
//                }
//                catch (Exception ex)
//                {
//                    return new ResponseResultDTO<EmployeeActivityReportByManagerDto>
//                    {
//                        Success = false,
//                        Message = ex.Message
//                    };
//                }
//            }
//        }
//    }
//}

//using HRsystem.Api.Database.DataTables;
//using HRsystem.Api.Database;
//using HRsystem.Api.Features.Reports.DTO;
//using HRsystem.Api.Services.CurrentUser;
//using HRsystem.Api.Shared.DTO;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using FluentValidation;

//namespace HRsystem.Api.Features.Reports
//{
//    public class EmployeeActivityReportByManager
//    {
//        // ===== Query =====
//        public record GetEmployeeActivityReportByManagerQuery(
//            DateTime? FromDate = null,
//            DateTime? ToDate = null
//        ) : IRequest<ResponseResultDTO<EmployeeActivityReportByManagerDto>>;

//        // ===== Validator =====
//        public class GetEmployeeActivityReportByManagerQueryValidator
//            : AbstractValidator<GetEmployeeActivityReportByManagerQuery>
//        {
//            public GetEmployeeActivityReportByManagerQueryValidator()
//            {
//                RuleFor(x => x.FromDate)
//                    .LessThanOrEqualTo(x => x.ToDate)
//                    .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
//                    .WithMessage("FromDate must be before ToDate");
//            }
//        }

//        // ===== Handler =====
//        public class GetEmployeeActivityReportByManagerQueryHandler
//            : IRequestHandler<GetEmployeeActivityReportByManagerQuery, ResponseResultDTO<EmployeeActivityReportByManagerDto>>
//        {
//            private readonly DBContextHRsystem _db;
//            private readonly ICurrentUserService _currentUser;

//            public GetEmployeeActivityReportByManagerQueryHandler(DBContextHRsystem db, ICurrentUserService currentUser)
//            {
//                _db = db;
//                _currentUser = currentUser;
//            }

//            public async Task<ResponseResultDTO<EmployeeActivityReportByManagerDto>> Handle(
//                GetEmployeeActivityReportByManagerQuery request,
//                CancellationToken cancellationToken)
//            {
//                try
//                {
//                    var managerId = _currentUser.EmployeeID;
//                    var fromDate = request.FromDate?.Date ?? DateTime.Today;
//                    var toDate = request.ToDate?.Date ?? DateTime.Today;

//                    // ===== employees under manager =====
//                    var employees = await _db.TbEmployees
//                        .Where(e => e.ManagerId == managerId && e.IsActive)
//                        .ToListAsync(cancellationToken);

//                    if (!employees.Any())
//                        return new ResponseResultDTO<EmployeeActivityReportByManagerDto>
//                        {
//                            Success = true,
//                            Data = new EmployeeActivityReportByManagerDto()
//                        };

//                    var employeeIds = employees.Select(e => e.EmployeeId).ToList();

//                    // ===== Get monthly reports (attendance) in range =====
//                    var reports = await _db.TbEmployeeMonthlyReports
//                        .Where(r => r.EmployeeId.HasValue
//                                    && employeeIds.Contains(r.EmployeeId.Value)
//                                    && r.Date.Date >= fromDate
//                                    && r.Date.Date <= toDate)
//                        .Include(r => r.EmployeeTodayStatues)
//                        .ToListAsync(cancellationToken);

//                    // ===== Build rows per employee per day =====
//                    var rows = new List<EmployeeActivityRowDto>();
//                    var summaryDict = new Dictionary<string, int>();

//                    for (var date = fromDate; date <= toDate; date = date.AddDays(1))
//                    {
//                        foreach (var e in employees)
//                        {
//                            // ===== find report for this employee on this date =====
//                            var rep = reports.FirstOrDefault(r => r.EmployeeId == e.EmployeeId && r.Date.Date == date);

//                            if (rep != null)
//                            {
//                                var statuesName = rep.EmployeeTodayStatuesId > 0
//                                    ? _db.TbAttendanceStatues
//                                        .Where(s => s.AttendanceStatuesId == rep.EmployeeTodayStatuesId)
//                                        .Select(s => s.AttendanceStatuesCode)
//                                        .FirstOrDefault()
//                                    : "UNKNOWN";

//                                rows.Add(new EmployeeActivityRowDto
//                                {
//                                    DayId = rep.DayId,
//                                    Date = rep.Date,
//                                    EmployeeId = e.EmployeeId,
//                                    EnglishFullName = e.EnglishFullName,
//                                    ArabicFullName = e.ArabicFullName,
//                                    ContractTypeId = e.ContractTypeId,
//                                    EmployeeCodeFinance = e.EmployeeCodeFinance,
//                                    EmployeeCodeHr = e.EmployeeCodeHr,
//                                    JobTitleId = e.JobTitleId,
//                                    JobLevelId = e.JobLevelId,
//                                    ManagerId = e.ManagerId,
//                                    CompanyId = e.CompanyId,
//                                    DepartmentId = e.DepartmentId,
//                                    ShiftId = e.ShiftId,
//                                    WorkDaysId = e.WorkDaysId,
//                                    RemoteWorkDaysId = e.RemoteWorkDaysId,
//                                    ActivityId = rep.ActivityId,
//                                    ActivityTypeId = rep.ActivityTypeId,
//                                    EmployeeTodayStatuesId = rep.EmployeeTodayStatuesId,
//                                    RequestBy = rep.RequestBy,
//                                    ApprovedBy = rep.ApprovedBy,
//                                    RequestDate = rep.RequestDate,
//                                    ApprovedDate = rep.ApprovedDate,
//                                    AttendanceId = rep.AttendanceId,
//                                    AttendanceDate = rep.AttendanceDate,
//                                    FirstPuchin = rep.FirstPuchin,
//                                    AttStatues = rep.AttStatues,
//                                    LastPuchout = rep.LastPuchout,
//                                    TotalHours = rep.TotalHours,
//                                    ActualWorkingHours = rep.ActualWorkingHours,
//                                    IsHoliday = rep.IsHoliday,
//                                    IsWorkday = rep.IsWorkday,
//                                    IsRemoteday = rep.IsRemoteday,
//                                    TodayStatues = statuesName ?? "UNKNOWN",
//                                    Details = rep.Details
//                                });

//                                if (!summaryDict.ContainsKey(statuesName))
//                                    summaryDict[statuesName] = 0;
//                                summaryDict[statuesName]++;
//                            }
//                        }
//                    }

//                    // ===== Build summary =====
//                    var summary = summaryDict.Select(kvp => new EmployeeActivitySummaryDto
//                    {
//                        ActivityTypeCode = kvp.Key,
//                        ActivityTypeName = kvp.Key,
//                        Count = kvp.Value
//                    }).ToList();

//                    return new ResponseResultDTO<EmployeeActivityReportByManagerDto>
//                    {
//                        Success = true,
//                        Data = new EmployeeActivityReportByManagerDto
//                        {
//                            Rows = rows,
//                            Summary = summary
//                        }
//                    };
//                }
//                catch (Exception ex)
//                {
//                    return new ResponseResultDTO<EmployeeActivityReportByManagerDto>
//                    {
//                        Success = false,
//                        Message = ex.Message
//                    };
//                }
//            }
//        }
//    }
//}


//using HRsystem.Api.Database;
//using HRsystem.Api.Database.DataTables;
//using HRsystem.Api.Features.Reports.DTO;
//using HRsystem.Api.Services.CurrentUser;
//using HRsystem.Api.Shared.DTO;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using FluentValidation;

//namespace HRsystem.Api.Features.Reports
//{
//    public class EmployeeActivityReportByManager
//    {
//        // ================= Query =================
//        public record GetEmployeeActivityReportByManagerQuery(
//            DateTime? FromDate = null,
//            DateTime? ToDate = null
//        ) : IRequest<ResponseResultDTO<EmployeeActivityReportByManagerDto>>;

//        // ================= Validator =================
//        public class GetEmployeeActivityReportByManagerQueryValidator
//            : AbstractValidator<GetEmployeeActivityReportByManagerQuery>
//        {
//            public GetEmployeeActivityReportByManagerQueryValidator()
//            {
//                RuleFor(x => x.FromDate)
//                    .LessThanOrEqualTo(x => x.ToDate)
//                    .When(x => x.FromDate.HasValue && x.ToDate.HasValue);
//            }
//        }

//        // ================= Handler =================
//        public class GetEmployeeActivityReportByManagerQueryHandler
//            : IRequestHandler<GetEmployeeActivityReportByManagerQuery, ResponseResultDTO<EmployeeActivityReportByManagerDto>>
//        {
//            private readonly DBContextHRsystem _db;
//            private readonly ICurrentUserService _currentUser;

//            public GetEmployeeActivityReportByManagerQueryHandler(
//                DBContextHRsystem db,
//                ICurrentUserService currentUser)
//            {
//                _db = db;
//                _currentUser = currentUser;
//            }

//            public async Task<ResponseResultDTO<EmployeeActivityReportByManagerDto>> Handle(
//                GetEmployeeActivityReportByManagerQuery request,
//                CancellationToken cancellationToken)
//            {
//                var managerId = _currentUser.EmployeeID;
//                var fromDate = request.FromDate?.Date;
//                var toDate = request.ToDate?.Date;

//                // ===== Base query (READ ONLY) =====
//                var query = _db.TbEmployeeMonthlyReports
//                    .AsNoTracking()
//                    .Where(r => r.ManagerId == managerId);

//                if (fromDate.HasValue)
//                    query = query.Where(r => r.Date >= fromDate.Value);

//                if (toDate.HasValue)
//                    query = query.Where(r => r.Date <= toDate.Value);

//                // ===== Load data =====
//                var data = await query.ToListAsync(cancellationToken);

//                // ===== Rows =====
//                var rows = data.Select(r => new EmployeeActivityRowDto
//                {
//                    DayId = r.DayId,
//                    Date = r.Date,
//                    EmployeeId = (int)r.EmployeeId,
//                    EnglishFullName = r.EnglishFullName,
//                    ArabicFullName = r.ArabicFullName,
//                    ContractTypeId = r.ContractTypeId,
//                    EmployeeCodeFinance = r.EmployeeCodeFinance,
//                    EmployeeCodeHr = r.EmployeeCodeHr,
//                    JobTitleId = (int)r.JobTitleId,
//                    JobLevelId = (int)r.JobLevelId,
//                    ManagerId = (int)r.ManagerId,
//                    CompanyId = (int)r.CompanyId,
//                    DepartmentId = (int)r.DepartmentId,
//                    ShiftId = (int)r.ShiftId,
//                    WorkDaysId = r.WorkDaysId,
//                    RemoteWorkDaysId = r.RemoteWorkDaysId,
//                    ActivityId = r.ActivityId,
//                    ActivityTypeId = r.ActivityTypeId,
//                    EmployeeTodayStatuesId = r.EmployeeTodayStatuesId,
//                    RequestBy = r.RequestBy,
//                    ApprovedBy = r.ApprovedBy,
//                    RequestDate = r.RequestDate,
//                    ApprovedDate = r.ApprovedDate,
//                    AttendanceId = r.AttendanceId,
//                    AttendanceDate = r.AttendanceDate,
//                    FirstPuchin = r.FirstPuchin,
//                    AttStatues = (int)r.AttStatues,
//                    LastPuchout = r.LastPuchout,
//                    TotalHours = r.TotalHours,
//                    ActualWorkingHours = r.ActualWorkingHours,
//                    IsHoliday = r.IsHoliday,
//                    IsWorkday = r.IsWorkday,
//                    IsRemoteday = r.IsRemoteday,
//                    TodayStatues = r.TodayStatues,
//                    Details = r.Details
//                }).ToList();

//                // ===== Summary (NO CALCULATION) =====
//                var summary = data
//                    .GroupBy(r => r.EmployeeTodayStatuesId)
//                    .Select(g => new ActivitySummaryDto
//                    {
//                        ActivityTypeCode = g.Key.ToString(),
//                        ActivityTypeName = g.First().TodayStatues,
//                        Count = g.Count()
//                    })
//                    .ToList();

//                return new ResponseResultDTO<EmployeeActivityReportByManagerDto>
//                {
//                    Success = true,
//                    Data = new EmployeeActivityReportByManagerDto
//                    {
//                        Rows = rows,
//                        Summary = summary
//                    }
//                };
//            }
//        }
//    }
//}



using HRsystem.Api.Database;
using HRsystem.Api.Features.Reports.DTO;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace HRsystem.Api.Features.Reports
{
    public class EmployeeActivityReportByManager
    {
        // ================= Query =================
        public record GetEmployeeActivityReportByManagerQuery(
            DateTime? FromDate = null,
            DateTime? ToDate = null
        ) : IRequest<ResponseResultDTO<EmployeeActivityReportByManagerDto>>;

        // ================= Validator =================
        public class GetEmployeeActivityReportByManagerQueryValidator
            : AbstractValidator<GetEmployeeActivityReportByManagerQuery>
        {
            public GetEmployeeActivityReportByManagerQueryValidator()
            {
                RuleFor(x => x.FromDate)
                    .LessThanOrEqualTo(x => x.ToDate)
                    .When(x => x.FromDate.HasValue && x.ToDate.HasValue);
            }
        }

        // ================= Handler =================
        public class GetEmployeeActivityReportByManagerQueryHandler
            : IRequestHandler<GetEmployeeActivityReportByManagerQuery, ResponseResultDTO<EmployeeActivityReportByManagerDto>>
        {
            private readonly DBContextHRsystem _db;
            private readonly ICurrentUserService _currentUser;

            public GetEmployeeActivityReportByManagerQueryHandler(
                DBContextHRsystem db,
                ICurrentUserService currentUser)
            {
                _db = db;
                _currentUser = currentUser;
            }

            public async Task<ResponseResultDTO<EmployeeActivityReportByManagerDto>> Handle(
                GetEmployeeActivityReportByManagerQuery request,
                CancellationToken cancellationToken)
            {
                var managerId = _currentUser.EmployeeID;
                var today = DateTime.UtcNow.Date;

                var fromDate = request.FromDate?.Date ?? today;
                var toDate = request.ToDate?.Date ?? today;

                var rows = new List<EmployeeActivityRowDto>();

                // =====================================================
                // 1️⃣ Past days (قبل النهارده) → Monthly Report
                // =====================================================
                if (fromDate < today)
                {
                    var pastToDate = toDate < today ? toDate : today.AddDays(-1);

                    var pastData = await _db.TbEmployeeMonthlyReports
                        .AsNoTracking()
                        .Where(r =>
                            r.ManagerId == managerId &&
                            r.Date >= fromDate &&
                            r.Date <= pastToDate)
                        .ToListAsync(cancellationToken);

                    rows.AddRange(pastData.Select(r => new EmployeeActivityRowDto
                    {
                        DayId = r.DayId,
                        Date = r.Date,
                        EmployeeId = r.EmployeeId ?? 0,
                        EnglishFullName = r.EnglishFullName ?? null,
                        ArabicFullName = r.ArabicFullName ?? null,
                        ContractTypeId = r.ContractTypeId ?? null,
                        EmployeeCodeFinance = r.EmployeeCodeFinance ?? null,
                        EmployeeCodeHr = r.EmployeeCodeHr ?? null,
                        JobTitleId = r.JobTitleId ?? 0,
                        JobLevelId = r.JobLevelId ?? 0,
                        ManagerId = r.ManagerId ?? 0,
                        CompanyId = r.CompanyId ?? 0,
                        DepartmentId = r.DepartmentId ?? 0,
                        ShiftId = r.ShiftId ?? 0,
                        WorkDaysId = r.WorkDaysId ?? 0,
                        RemoteWorkDaysId = r.RemoteWorkDaysId ?? 0,
                        ActivityId = r.ActivityId ?? 0,
                        ActivityTypeId = r.ActivityTypeId ?? 0,
                        EmployeeTodayStatuesId = r.EmployeeTodayStatuesId ,
                        RequestBy = r.RequestBy ?? null,
                        ApprovedBy = r.ApprovedBy,
                        RequestDate = r.RequestDate,
                        ApprovedDate = r.ApprovedDate,
                        AttendanceId = r.AttendanceId,
                        AttendanceDate = r.AttendanceDate,
                        FirstPuchin = r.FirstPuchin,
                        AttStatues = r.AttStatues.HasValue ? (int?)r.AttStatues.Value : null,
                        LastPuchout = r.LastPuchout,
                        TotalHours = r.TotalHours,
                        ActualWorkingHours = r.ActualWorkingHours,
                        IsHoliday = r.IsHoliday,
                        IsWorkday = r.IsWorkday,
                        IsRemoteday = r.IsRemoteday,
                        TodayStatues = r.TodayStatues,
                        Details = r.Details
                    }));
                }

                // =====================================================
                // 2️⃣ Today logic
                // =====================================================
                if (fromDate <= today && toDate >= today)
                {
                    var employees = await _db.TbEmployees
                        .AsNoTracking()
                        .Where(e => e.ManagerId == managerId)
                        .ToListAsync(cancellationToken);

                    var todayActivities = await _db.TbEmployeeActivities
                        .AsNoTracking()
                        .Where(a => a.RequestDate.Date == today)
                        .ToListAsync(cancellationToken);

                    foreach (var emp in employees)
                    {
                        var empActivities = todayActivities
                            .Where(a => a.EmployeeId == emp.EmployeeId)
                            .ToList();

                        // ❌ No activities → ABSENT
                        if (!empActivities.Any())
                        {
                            rows.Add(new EmployeeActivityRowDto
                            {
                                Date = today,
                                EmployeeId = emp.EmployeeId,
                                EnglishFullName = emp.EnglishFullName,
                                ArabicFullName = emp.ArabicFullName,
                                JobTitleId = (int)emp.JobTitleId,
                                JobLevelId = (int)emp.JobLevelId,
                                ManagerId = (int)emp.ManagerId,
                                CompanyId = (int)emp.CompanyId,
                                DepartmentId = (int)emp.DepartmentId,
                                ShiftId = (int)emp.ShiftId ,
                                EmployeeTodayStatuesId = 2, // Absent
                                TodayStatues = "Absent",
                                IsWorkday = true
                            });

                            continue;
                        }

                        // ✔ Has activity
                        foreach (var act in empActivities)
                        {
                            rows.Add(new EmployeeActivityRowDto
                            {
                                Date = today,
                                EmployeeId = emp.EmployeeId,
                                EnglishFullName = emp.EnglishFullName,
                                ArabicFullName = emp.ArabicFullName,
                                JobTitleId = (int)emp.JobTitleId,
                                JobLevelId = (int)emp.JobLevelId,
                                ManagerId = (int)emp.ManagerId,
                                CompanyId = (int)emp.CompanyId,
                                DepartmentId = (int)emp.DepartmentId,
                                ShiftId = (int)emp.ShiftId,
                                ActivityId = act.ActivityId,
                                ActivityTypeId = act.ActivityTypeId,
                                EmployeeTodayStatuesId = 1,
                                TodayStatues = "Has Activity",
                                RequestDate = act.RequestDate
                            });
                        }
                    }
                }

                // =====================================================
                // 3️⃣ Summary
                // =====================================================
                var summary = rows
                    .GroupBy(r => r.TodayStatues)
                    .Select(g => new ActivitySummaryDto
                    {
                        ActivityTypeCode = g.Key,
                        ActivityTypeName = g.Key,
                        Count = g.Count()
                    })
                    .ToList();

                return new ResponseResultDTO<EmployeeActivityReportByManagerDto>
                {
                    Success = true,
                    Data = new EmployeeActivityReportByManagerDto
                    {
                        Rows = rows,
                        Summary = summary
                    }
                };
            }
        }
    }
}

