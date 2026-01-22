
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using FluentValidation;
    using global::HRsystem.Api.Database;
    using global::HRsystem.Api.Features.Reports.DTO;
    using global::HRsystem.Api.Shared.DTO;

namespace HRsystem.Api.Features.Reports
{
    public class EmployeeActivityReportByDepartment
    {
        // ================= Query =================
        public record GetEmployeeActivityReportByDepartmentQuery(
            int DepartmentId,
            DateTime? FromDate = null,
            DateTime? ToDate = null
        ) : IRequest<ResponseResultDTO<EmployeeActivityReportByDepartmentDto>>;

        // ================= Validator =================
        public class Validator
            : AbstractValidator<GetEmployeeActivityReportByDepartmentQuery>
        {
            public Validator()
            {
                RuleFor(x => x.DepartmentId).GreaterThan(0);

                RuleFor(x => x.FromDate)
                    .LessThanOrEqualTo(x => x.ToDate)
                    .When(x => x.FromDate.HasValue && x.ToDate.HasValue);
            }
        }

        // ================= Handler =================
        public class Handler
   : IRequestHandler<GetEmployeeActivityReportByDepartmentQuery,
       ResponseResultDTO<EmployeeActivityReportByDepartmentDto>>
        {
            private readonly DBContextHRsystem _db;

            public Handler(DBContextHRsystem db)
            {
                _db = db;
            }

            public async Task<ResponseResultDTO<EmployeeActivityReportByDepartmentDto>> Handle(
                GetEmployeeActivityReportByDepartmentQuery request,
                CancellationToken cancellationToken)
            {
                var today = DateTime.UtcNow.Date;
                var fromDate = request.FromDate?.Date ?? today;
                var toDate = request.ToDate?.Date ?? today;

                var rows = new List<ActivityRowDto>();

                // ===================== Managers lookup =====================
                var managers = await _db.TbEmployees
                    .Select(e => new { e.EmployeeId, e.EnglishFullName })
                    .ToListAsync(cancellationToken);

                // =====================================================
                // 1️⃣ Past days → Monthly Report
                // =====================================================
                if (fromDate < today)
                {
                    var pastToDate = toDate < today ? toDate : today.AddDays(-1);

                    var pastData = (
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
                        where r.DepartmentId == request.DepartmentId
                              && r.Date >= fromDate
                              && r.Date <= pastToDate
                        select new
                        {
                            r,
                            jt,
                            jl,
                            d,
                            c,
                            s
                        })
                        .AsEnumerable()
                        .Select(x => new ActivityRowDto
                        {
                            DayId = x.r.DayId,
                            Date = x.r.Date,
                            EmployeeId = x.r.EmployeeId,
                            EnglishFullName = x.r.EnglishFullName,
                            ArabicFullName = x.r.ArabicFullName,

                            JobTitleId = x.r.JobTitleId,
                            JobTitleName = x.jt?.TitleName?.en,

                            JobLevelId = x.r.JobLevelId,
                            JobLevelCode = x.jl?.JobLevelCode,

                            DepartmentId = x.r.DepartmentId,
                            DepartmentCode = x.d?.DepartmentCode,
                            DepartmentName = x.d?.DepartmentName?.en,

                            CompanyId = x.r.CompanyId,
                            CompanyName = x.c?.CompanyName,

                            ManagerId = x.r.ManagerId,
                            ManagerName = x.r.ManagerId.HasValue
                                ? managers.FirstOrDefault(m => m.EmployeeId == x.r.ManagerId)?.EnglishFullName
                                : null,

                            ShiftId = x.r.ShiftId,
                            ShiftName = x.s?.ShiftName?.en,
                            ShiftStartTime = x.s != null ? x.s.StartTime.ToString() : null,
                            ShiftEndTime = x.s != null ? x.s.EndTime.ToString() : null,

                            ActivityId = x.r.ActivityId,
                            ActivityTypeId = x.r.ActivityTypeId,
                            EmployeeTodayStatuesId = x.r.EmployeeTodayStatuesId,
                            TodayStatues = x.r.TodayStatues,
                            AttStatues = x.r.AttStatues.HasValue ? (int?)x.r.AttStatues.Value : null,
                            Details = x.r.Details
                        })
                        .ToList();

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
                        .AsNoTracking()
                        .Where(e => e.DepartmentId == request.DepartmentId)
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

                        var managerName = emp.ManagerId > 0
                            ? managers.FirstOrDefault(m => m.EmployeeId == emp.ManagerId)?.EnglishFullName
                            : null;

                        if (!empActs.Any())
                        {
                            rows.Add(new ActivityRowDto
                            {
                                Date = today,
                                EmployeeId = emp.EmployeeId,
                                EnglishFullName = emp.EnglishFullName,
                                ArabicFullName = emp.ArabicFullName,

                                JobTitleId = emp.JobTitleId,
                                JobTitleName = emp.JobTitle?.TitleName?.en,

                                JobLevelId = emp.JobLevelId,
                                JobLevelCode = emp.JobLevel?.JobLevelCode,

                                DepartmentId = emp.DepartmentId,
                                DepartmentCode = emp.Department?.DepartmentCode,
                                DepartmentName = emp.Department?.DepartmentName?.en,

                                CompanyId = emp.CompanyId,
                                CompanyName = emp.Company?.CompanyName,

                                ManagerId = emp.ManagerId,
                                ManagerName = managerName,

                                EmployeeTodayStatuesId = 2,
                                TodayStatues = "Absent",
                                IsWorkday = true
                            });
                            continue;
                        }

                        foreach (var act in empActs)
                        {
                            rows.Add(new ActivityRowDto
                            {
                                Date = today,
                                EmployeeId = emp.EmployeeId,
                                EnglishFullName = emp.EnglishFullName,
                                ArabicFullName = emp.ArabicFullName,

                                JobTitleId = emp.JobTitleId,
                                JobTitleName = emp.JobTitle?.TitleName?.en,

                                JobLevelId = emp.JobLevelId,
                                JobLevelCode = emp.JobLevel?.JobLevelCode,

                                DepartmentId = emp.DepartmentId,
                                DepartmentCode = emp.Department?.DepartmentCode,
                                DepartmentName = emp.Department?.DepartmentName?.en,

                                CompanyId = emp.CompanyId,
                                CompanyName = emp.Company?.CompanyName,

                                ManagerId = emp.ManagerId,
                                ManagerName = managerName,

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
                    .GroupBy(r => r.TodayStatues ?? "Unknown")
                    .Select(g => new SummaryDto
                    {
                        ActivityTypeCode = g.Key,
                        ActivityTypeName = g.Key,
                        Count = g.Count()
                    })
                    .ToList();

                return new ResponseResultDTO<EmployeeActivityReportByDepartmentDto>
                {
                    Success = true,
                    Data = new EmployeeActivityReportByDepartmentDto
                    {
                        Rows = rows,
                        Summary = summary
                    }
                };
            }
        }
    }
}


