using HRsystem.Api.Database;
using HRsystem.Api.Database.Entities;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.EmployeeHandler.GetList
{
 
        public record GetAllEmployeesListQuery(
            int? DepartmentId
        ) : IRequest<ResponseResultDTO<List<EmployeeSearchDto>>>;

        public class GetAllEmployeesListHandler
            : IRequestHandler<GetAllEmployeesListQuery, ResponseResultDTO<List<EmployeeSearchDto>>>
        {
            private readonly DBContextHRsystem _db;
            private readonly UserManager<ApplicationUser> _userManager;

            public GetAllEmployeesListHandler(
                DBContextHRsystem db,
                UserManager<ApplicationUser> userManager)
            {
                _db = db;
                _userManager = userManager;
            }

            public async Task<ResponseResultDTO<List<EmployeeSearchDto>>> Handle(
                GetAllEmployeesListQuery request,
                CancellationToken ct)
            {
                var query = _db.TbEmployees
                    .AsNoTracking()
                    .AsQueryable();

                // Optional Department filter
                if (request.DepartmentId.HasValue && request.DepartmentId > 0)
                {
                    query = query.Where(e => e.DepartmentId == request.DepartmentId);
                }

                // Sort by EmployeeId for consistency
                query = query.OrderBy(e => e.EmployeeId);

                var employees = await query
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

                return new ResponseResultDTO<List<EmployeeSearchDto>>
                {
                    Success = true,
                    Message = $"Retrieved {employees.Count} employees successfully.",
                    Data = employees
                };
            }
        }
    }

