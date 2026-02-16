
using HRsystem.Api.Database;
using HRsystem.Api.Database.Entities;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.EmployeeHandler.GetList
{
    public class EmployeeSearchDto
    {
        public int EmployeeId { get; set; }
        public int CompanyId { get; set; }
        public string FullNameEnglish { get; set; } = string.Empty;
        public string FullNameArabic { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public string EmployeeCodeHr { get; set; } = string.Empty;
        public string EmployeeCodeFinance { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public List<string> WorkLocations { get; set; } = new();
        public List<string> Projects { get; set; } = new();

        public List<int>? RoleIds { get; set; } = new();
        public int? UserId { get; set; }
        public string? UserName { get; set; } = string.Empty;
        public string? UserFullName { get; set; } = string.Empty;

        public string? PhotoUrl { get; set; } = string.Empty;
        public string? DepartmentName { get; set; } = string.Empty;
        public string? MobileNumber { get; set; } = string.Empty;

        public string ShiftName { get; set; } = string.Empty;
        public string? ShiftStartTime { get; set; } = string.Empty;
        public string? ShiftEndTime { get; set; } = string.Empty;

    }

    //public record GetEmployeesListQuery(
    //        int? DepartmentId,
    //        int PageNumber = 1,
    //        int PageSize = 10
    //    ) : IRequest<ResponseResultDTO<PagedResult<EmployeeSearchDto>>>;


    public record GetEmployeesListQuery(
    int? DepartmentId,
    string? Search,
    string? SortBy,
    string? SortDirection,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<ResponseResultDTO<PagedResult<EmployeeSearchDto>>>;


    public class GetEmployeesListHandler
            : IRequestHandler<GetEmployeesListQuery, ResponseResultDTO<PagedResult<EmployeeSearchDto>>>
    {
        private readonly DBContextHRsystem _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetEmployeesListHandler(
            DBContextHRsystem db,
            UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<ResponseResultDTO<PagedResult<EmployeeSearchDto>>> Handle(
            GetEmployeesListQuery request,
            CancellationToken ct)
        {
            var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
            var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            var query = _db.TbEmployees
                .AsNoTracking()
                .AsQueryable();

            // Optional Department filter
            if (request.DepartmentId.HasValue && request.DepartmentId > 0)
            {
                query = query.Where(e => e.DepartmentId == request.DepartmentId);
            }


            // 🔍 Search filter
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.ToLower();


                query = query.Where(e =>
                    e.EnglishFullName.ToLower().Contains(search) ||
                    e.ArabicFullName.ToLower().Contains(search) ||
                    e.NationalId.Contains(search) ||
                    e.EmployeeCodeHr.Contains(search) ||
                     e.JobTitle.TitleName.ar.Contains(search) ||
                      e.PrivateMobile.Contains(search) ||
                    e.Department.DepartmentName.en.Contains(search)
                );
            }




            var totalCount = await query.CountAsync(ct);

            query = request.SortBy switch
            {
                "EnglishFullName" => request.SortDirection == "desc"
                    ? query.OrderByDescending(e => e.EnglishFullName)
                    : query.OrderBy(e => e.EnglishFullName),

                "NationalId" => request.SortDirection == "desc"
                    ? query.OrderByDescending(e => e.NationalId)
                    : query.OrderBy(e => e.NationalId),

                "EmployeeCodeHr" => request.SortDirection == "desc"
                    ? query.OrderByDescending(e => e.EmployeeCodeHr)
                    : query.OrderBy(e => e.EmployeeCodeHr),

                _ => query.OrderBy(e => e.EmployeeId)
            };


            var employees = await query
                 
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EmployeeSearchDto
                {
                    EmployeeId = e.EmployeeId,
                    CompanyId = e.CompanyId,
                    FullNameEnglish = e.EnglishFullName,
                    FullNameArabic = e.ArabicFullName,
                    NationalId = e.NationalId,
                    EmployeeCodeHr = e.EmployeeCodeHr,
                    EmployeeCodeFinance = e.EmployeeCodeFinance,
                    PhotoUrl = e.EmployeePhotoPath,
                    MobileNumber = e.PrivateMobile,
                    DepartmentName = e.Department != null
                        ? e.Department.DepartmentName.ar + " - (" +
                          e.Department.DepartmentName.en + ")"
                        : string.Empty,
                    JobTitle = e.JobTitle != null
                        ? e.JobTitle.TitleName.ar
                        : string.Empty,
                    ShiftName = e.Shifts != null
                        ? e.Shifts.ShiftName.ar
                        : string.Empty,
                    ShiftStartTime = e.Shifts != null
                        ? e.Shifts.StartTime.ToString()
                        : string.Empty,
                    ShiftEndTime = e.Shifts != null
                        ? e.Shifts.EndTime.ToString()
                        : string.Empty,
                    WorkLocations = e.TbEmployeeWorkLocations
                        .Select(w => w.WorkLocation.LocationName.ar)
                        .ToList(),
                    Projects = e.TbEmployeeProjects
                        .Select(p => p.Project.ProjectName.ar)
                        .ToList()
                })
                .ToListAsync(ct);

            // Load Users in ONE query (avoid N+1)
            var employeeIds = employees.Select(e => e.EmployeeId).ToList();

            var users = await _userManager.Users
                .Where(u => u.EmployeeId != null && employeeIds.Contains(u.EmployeeId.Value))
                .Select(u => new { u.Id, u.UserName, u.EmployeeId })
                .ToListAsync(ct);

            var userIds = users.Select(u => u.Id).ToList();

            var userRoles = await _db.UserRoles
                .Where(ur => userIds.Contains(ur.UserId))
                .ToListAsync(ct);

            // Attach user + roles
            foreach (var emp in employees)
            {
                var user = users.FirstOrDefault(u => u.EmployeeId == emp.EmployeeId);

                if (user != null)
                {
                    emp.UserId = user.Id;
                    emp.UserName = user.UserName;
                    emp.UserFullName = user.UserName;

                    emp.RoleIds = userRoles
                        .Where(r => r.UserId == user.Id)
                        .Select(r => r.RoleId)
                        .ToList();
                }
            }

            var result = new PagedResult<EmployeeSearchDto>
            {
                Items = employees,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return new ResponseResultDTO<PagedResult<EmployeeSearchDto>>
            {
                Success = true,
                Message = "Employees retrieved successfully.",
                Data = result
            };
        }
    }
}


