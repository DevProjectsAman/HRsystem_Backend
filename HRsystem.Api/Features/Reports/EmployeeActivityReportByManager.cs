
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
//                var today = DateTime.UtcNow.Date;

//                var fromDate = request.FromDate?.Date ?? today;
//                var toDate = request.ToDate?.Date ?? today;

//                var rows = new List<EmployeeActivityRowDto>();

//                // =====================================================
//                // 1️⃣ Past days → Monthly Report
//                // =====================================================
//                if (fromDate < today)
//                {
//                    var pastToDate = toDate < today ? toDate : today.AddDays(-1);

//                    var pastData = await _db.TbEmployeeMonthlyReports
//                        .AsNoTracking()
//                        .Where(r =>
//                            r.ManagerId == managerId &&
//                            r.Date >= fromDate &&
//                            r.Date <= pastToDate)
//                        .ToListAsync(cancellationToken);



//                    rows.AddRange(pastData.Select(r => new EmployeeActivityRowDto

//                    {
//                        DayId = r.DayId,
//                        Date = r.Date,
//                        EmployeeId = r.EmployeeId ?? 0,
//                        EnglishFullName = r.EnglishFullName ?? null,
//                        ArabicFullName = r.ArabicFullName ?? null,
//                        ContractTypeId = r.ContractTypeId ?? null,
//                        EmployeeCodeFinance = r.EmployeeCodeFinance ?? null,
//                        EmployeeCodeHr = r.EmployeeCodeHr ?? null,
//                        JobTitleId = r.JobTitleId ?? 0,
//                        JobLevelId = r.JobLevelId ?? 0,
//                        ManagerId = r.ManagerId ?? 0,
//                        CompanyId = r.CompanyId ?? 0,
//                        DepartmentId = r.DepartmentId ?? 0,
//                        ShiftId = r.ShiftId ?? 0,
//                        WorkDaysId = r.WorkDaysId ?? 0,
//                        RemoteWorkDaysId = r.RemoteWorkDaysId ?? 0,
//                        ActivityId = r.ActivityId ?? 0,
//                        ActivityTypeId = r.ActivityTypeId ?? 0,
//                        EmployeeTodayStatuesId = r.EmployeeTodayStatuesId,
//                        RequestBy = r.RequestBy ?? null,
//                        ApprovedBy = r.ApprovedBy,
//                        RequestDate = r.RequestDate,
//                        ApprovedDate = r.ApprovedDate,
//                        AttendanceId = r.AttendanceId,
//                        AttendanceDate = r.AttendanceDate,
//                        FirstPuchin = r.FirstPuchin,
//                        AttStatues = r.AttStatues.HasValue ? (int?)r.AttStatues.Value : null,
//                        LastPuchout = r.LastPuchout,
//                        TotalHours = r.TotalHours,
//                        ActualWorkingHours = r.ActualWorkingHours,
//                        IsHoliday = r.IsHoliday,
//                        IsWorkday = r.IsWorkday,
//                        IsRemoteday = r.IsRemoteday,
//                        TodayStatues = r.TodayStatues,
//                        Details = r.Details
//                    }));
//                }

//                // =====================================================
//                // 2️⃣ Today logic
//                // =====================================================
//                if (fromDate <= today && toDate >= today)
//                {
//                    var employees = await _db.TbEmployees
//                        .AsNoTracking()
//                        .Where(e => e.ManagerId == managerId)
//                        .ToListAsync(cancellationToken);

//                    var todayActivities = await _db.TbEmployeeActivities
//                        .AsNoTracking()
//                        .Where(a => a.RequestDate.Date == today)
//                        .ToListAsync(cancellationToken);

//                    foreach (var emp in employees)
//                    {
//                        var empActivities = todayActivities
//                            .Where(a => a.EmployeeId == emp.EmployeeId)
//                            .ToList();

//                        //  No activities → ABSENT
//                        if (!empActivities.Any())
//                        {
//                            rows.Add(new EmployeeActivityRowDto
//                            {
//                                Date = today,
//                                EmployeeId = emp.EmployeeId,
//                                EnglishFullName = emp.EnglishFullName,
//                                ArabicFullName = emp.ArabicFullName,
//                                JobTitleId = (int)emp.JobTitleId,
//                                JobLevelId = (int)emp.JobLevelId,
//                                ManagerId = (int)emp.ManagerId,
//                                CompanyId = (int)emp.CompanyId,
//                                DepartmentId = (int)emp.DepartmentId,
//                                ShiftId = (int)emp.ShiftId,
//                                EmployeeTodayStatuesId = 2, // Absent
//                                TodayStatues = "Absent",
//                                IsWorkday = true
//                            });

//                            continue;
//                        }

//                        //  Has activity
//                        foreach (var act in empActivities)
//                        {
//                            rows.Add(new EmployeeActivityRowDto
//                            {
//                                Date = today,
//                                EmployeeId = emp.EmployeeId,
//                                EnglishFullName = emp.EnglishFullName,
//                                ArabicFullName = emp.ArabicFullName,
//                                JobTitleId = (int)emp.JobTitleId,
//                                JobLevelId = (int)emp.JobLevelId,
//                                ManagerId = (int)emp.ManagerId,
//                                CompanyId = (int)emp.CompanyId,
//                                DepartmentId = (int)emp.DepartmentId,
//                                ShiftId = (int)emp.ShiftId,
//                                ActivityId = act.ActivityId,
//                                ActivityTypeId = act.ActivityTypeId,
//                                EmployeeTodayStatuesId = 1,
//                                TodayStatues = "Has Activity",
//                                RequestDate = act.RequestDate
//                            });
//                        }
//                    }
//                }

//                // =====================================================
//                // 3️⃣ Summary
//                // =====================================================
//                var summary = rows
//                    .GroupBy(r => r.TodayStatues)
//                    .Select(g => new ActivitySummaryDto
//                    {
//                        ActivityTypeCode = g.Key,
//                        ActivityTypeName = g.Key,
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

                // ===================== جلب كل المديرين مرة واحدة =====================
                var allManagerIds = await _db.TbEmployees
                    .Select(e => e.EmployeeId)
                    .ToListAsync(cancellationToken);

                var managers = await _db.TbEmployees
                    .Where(e => allManagerIds.Contains(e.EmployeeId))
                    .Select(e => new { e.EmployeeId, e.EnglishFullName })
                    .ToListAsync(cancellationToken);

                // =====================================================
                // 1️⃣ Past days → Monthly Report
                // =====================================================
                if (fromDate < today)
                {
                    var pastToDate = toDate < today ? toDate : today.AddDays(-1);

                    var pastData = await (
                        from r in _db.TbEmployeeMonthlyReports.AsNoTracking()
                        join jt in _db.TbJobTitles.AsNoTracking() on r.JobTitleId equals jt.JobTitleId into jtG
                        from jt in jtG.DefaultIfEmpty()
                        join jl in _db.TbJobLevels.AsNoTracking() on r.JobLevelId equals jl.JobLevelId into jlG
                        from jl in jlG.DefaultIfEmpty()
                        join d in _db.TbDepartments.AsNoTracking() on r.DepartmentId equals d.DepartmentId into dG
                        from d in dG.DefaultIfEmpty()
                        join c in _db.TbCompanies.AsNoTracking() on r.CompanyId equals c.CompanyId into cG
                        from c in cG.DefaultIfEmpty()
                        join s in _db.TbShifts.AsNoTracking() on r.ShiftId equals s.ShiftId into sG
                        from s in sG.DefaultIfEmpty()
                        where r.ManagerId == managerId
                              && r.Date >= fromDate
                              && r.Date <= pastToDate
                        select new EmployeeActivityRowDto
                        {
                            DayId = r.DayId,
                            Date = r.Date,
                            EmployeeId = r.EmployeeId,
                            EnglishFullName = r.EnglishFullName,
                            ArabicFullName = r.ArabicFullName,
                            ContractTypeId = r.ContractTypeId,
                            EmployeeCodeFinance = r.EmployeeCodeFinance,
                            EmployeeCodeHr = r.EmployeeCodeHr,
                            JobTitleId = r.JobTitleId,
                            JobTitleName = jt != null ? jt.TitleName.en : null,
                            JobLevelId = r.JobLevelId,
                            JobLevelCode = jl != null ?jl.JobLevelCode : null,
                            ManagerId = r.ManagerId,
                            //ManagerName = r.ManagerId.HasValue
                            //    ? managers.FirstOrDefault(m => m.EmployeeId == r.ManagerId.Value)?.EnglishFullName
                            //    : null,
                            ManagerName = managers.FirstOrDefault(m => m.EmployeeId == r.ManagerId) != null
                            ? managers.First(m => m.EmployeeId == r.ManagerId).EnglishFullName
                            : null,
                            CompanyId = r.CompanyId,
                            CompanyName = c != null?c.CompanyName : null,
                            DepartmentId = r.DepartmentId,
                            DepartmentCode = d != null ? d.DepartmentCode : null,
                            DepartmentName = d != null ? d.DepartmentName.en: null,
                            ShiftId = r.ShiftId,
                            ShiftName = s != null?s.ShiftName.en : null,
                            ShiftStartTime = s != null ? s.StartTime.ToString(@"hh\:mm") : null,
                            ShiftEndTime = s != null ?s.EndTime.ToString(@"hh\:mm") : null,
                            ActivityId = r.ActivityId,
                            ActivityTypeId = r.ActivityTypeId,
                            EmployeeTodayStatuesId = r.EmployeeTodayStatuesId,
                            TodayStatues = r.TodayStatues,
                            AttStatues = r.AttStatues.HasValue ? (int?)r.AttStatues.Value : null,
                            Details = r.Details
                        }).ToListAsync(cancellationToken);

                    rows.AddRange(pastData);
                }

                // =====================================================
                // 2️⃣ Today logic
                // =====================================================
                if (fromDate <= today && toDate >= today)
                {
                    var employees = await _db.TbEmployees
                        .Include(e => e.JobTitle)
                        .Include(e => e.JobLevel)
                        .Include(e => e.Department)
                        .Include(e => e.Company)
                      //  .Include(e => e.Shift)
                        .AsNoTracking()
                        .Where(e => e.ManagerId == managerId)
                        .ToListAsync(cancellationToken);

                    var todayActivities = await _db.TbEmployeeActivities
                        .Include(a => a.ActivityType)
                        .AsNoTracking()
                        .Where(a => a.RequestDate.Date == today)
                        .ToListAsync(cancellationToken);

                    foreach (var emp in employees)
                    {
                        var empActs = todayActivities
                            .Where(a => a.EmployeeId == emp.EmployeeId)
                            .ToList();

                        var managerName = managers.FirstOrDefault(m => m.EmployeeId == emp.ManagerId) != null
                             ? managers.First(m => m.EmployeeId == emp.ManagerId).EnglishFullName
                             : null;

                        // No activities → Absent
                        if (!empActs.Any())
                        {
                            rows.Add(new EmployeeActivityRowDto
                            {
                                Date = today,
                                EmployeeId = emp.EmployeeId,
                                EnglishFullName = emp.EnglishFullName,
                                ArabicFullName = emp.ArabicFullName,
                                JobTitleId = emp.JobTitleId,
                                JobTitleName = emp.JobTitle != null ? emp.JobTitle.TitleName.en : null,
                                JobLevelId = emp.JobLevelId,
                                JobLevelCode = emp.JobLevel?.JobLevelCode,
                                ManagerId = emp.ManagerId,
                                ManagerName = managerName,
                                CompanyId = emp.CompanyId,
                                CompanyName = emp.Company?.CompanyName,
                                DepartmentId = emp.DepartmentId,
                                DepartmentCode = emp.Department?.DepartmentCode,
                                DepartmentName = emp.Department != null ? emp.Department.DepartmentName.en : null,
                                ShiftId = emp.ShiftId,
                                ShiftName = null,
                                ShiftStartTime = null,
                                ShiftEndTime = null,
                                EmployeeTodayStatuesId = 2,
                                TodayStatues = "Absent",
                                IsWorkday = true
                            });
                            continue;
                        }

                        // Has activities
                        foreach (var act in empActs)
                        {
                            rows.Add(new EmployeeActivityRowDto
                            {
                                Date = today,
                                EmployeeId = emp.EmployeeId,
                                EnglishFullName = emp.EnglishFullName,
                                ArabicFullName = emp.ArabicFullName,
                                JobTitleId = emp.JobTitleId,
                                JobTitleName = emp.JobTitle != null ? emp.JobTitle.TitleName.en : null,
                                JobLevelId = emp.JobLevelId,
                                JobLevelCode = emp.JobLevel?.JobLevelCode,
                                ManagerId = emp.ManagerId,
                                ManagerName = managerName,
                                CompanyId = emp.CompanyId,
                                CompanyName = emp.Company?.CompanyName,
                                DepartmentId = emp.DepartmentId,
                                DepartmentCode = emp.Department?.DepartmentCode,
                                DepartmentName = emp.Department != null ? emp.Department.DepartmentName.en : null,
                                ShiftId = emp.ShiftId,
                                ShiftName = null,
                                ShiftStartTime = null,
                                ShiftEndTime =null,
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

