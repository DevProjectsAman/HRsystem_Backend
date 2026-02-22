using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.Tools;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.MyTeam.Get
{
    public record GetMyTeamTreeQuery()
        : IRequest<List<MyTeamTreeDto>>;

    public class MyTeamTreeDto
    {
        public int EmployeeId { get; set; }

        public string FullName { get; set; }
        public string EmployeeCode { get; set; }
        public string HrCode { get; set; }

        public string JobTitle { get; set; }
        public string DepartmentName { get; set; }
        public string DirectManagerName { get; set; }

        public string ShiftName { get; set; }
        public TimeOnly? ShiftFrom { get; set; }
        public TimeOnly? ShiftTo { get; set; }

        public List<string> WorkDays { get; set; } = [];

        public List<MyTeamTreeDto> Children { get; set; } = [];
    }

    public class Handler : IRequestHandler<GetMyTeamTreeQuery, List<MyTeamTreeDto>>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;

        public Handler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<List<MyTeamTreeDto>> Handle(GetMyTeamTreeQuery request, CancellationToken ct)
        {
            // Get current manager ID from JWT
            int managerId = (int)_currentUser.EmployeeID;
            var lang = _currentUser.UserLanguage ?? "en";
            var today = DateOnly.FromDateTime(DateTime.Today);

            // Step 1️⃣ – Get all team IDs recursively
            var teamIds = new List<int>();
            var currentLevel = new List<int> { managerId };

            while (currentLevel.Any())
            {
                var nextLevel = await _db.TbEmployees
                    .Where(e => e.IsActive && currentLevel.Contains(e.ManagerId))
                    .Select(e => e.EmployeeId)
                    .ToListAsync(ct);

                teamIds.AddRange(nextLevel);
                currentLevel = nextLevel;
            }

            if (!teamIds.Any())
                return new List<MyTeamTreeDto>();

            // Step 2️⃣ – Fetch employee data for those IDs
            var employees = await _db.TbEmployees
                .Where(e => teamIds.Contains(e.EmployeeId))
                .Select(e => new
                {
                    e.EmployeeId,
                    e.ManagerId,
                    FullName = lang == "ar" ? e.ArabicFullName : e.EnglishFullName,
                    e.UniqueEmployeeCode,
                    e.EmployeeCodeHr,
                    JobTitle = lang == "ar"
                        ? e.JobTitle.TitleName.ar
                        : e.JobTitle.TitleName.en,
                    Department = e.Department.DepartmentName,
                    ManagerName = lang == "ar"
                        ? e.Manager.ArabicFullName
                        : e.Manager.EnglishFullName,
                    WorkDays = e.TbWorkDays.WorkDaysNames,

                    // Latest active shift
                    Shift = e.TbEmployeeShifts
                        .Where(s => s.EffectiveDate <= today &&
                                   (s.EndDate == null || s.EndDate >= today))
                        .OrderByDescending(s => s.EffectiveDate)
                        .Select(s => new
                        {
                            Name = $"{s.Shift.ShiftName.en} ({s.Shift.ShiftName.ar})",
                            From = s.Shift.StartTime,
                            To = s.Shift.EndTime
                        })
                        .FirstOrDefault()
                })
                .ToListAsync(ct);

            // Step 3️⃣ – Build lookup dictionary
            var dictionary = employees.ToDictionary(
                e => e.EmployeeId,
                e => new MyTeamTreeDto
                {
                    EmployeeId = e.EmployeeId,
                    FullName = e.FullName,
                    EmployeeCode = e.UniqueEmployeeCode,
                    HrCode = e.EmployeeCodeHr,
                    JobTitle = e.JobTitle,
                    DepartmentName =$"{e.Department.en} ( {e.Department.ar} ) " ,
                    DirectManagerName = e.ManagerName,
                    ShiftName = e.Shift?.Name,
                    ShiftFrom = e.Shift?.From,
                    ShiftTo = e.Shift?.To,
                    WorkDays = e.WorkDays
                });

            // Step 4️⃣ – Build Tree
            var rootNodes = new List<MyTeamTreeDto>();

            foreach (var emp in employees)
            {
                if (emp.ManagerId == managerId)
                {
                    rootNodes.Add(dictionary[emp.EmployeeId]);
                }
                else if (dictionary.ContainsKey(emp.ManagerId))
                {
                    dictionary[emp.ManagerId]
                        .Children
                        .Add(dictionary[emp.EmployeeId]);
                }
            }

            return rootNodes;
        }
    }
}
