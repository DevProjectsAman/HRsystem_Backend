namespace HRsystem.Api.Features.EmployeeHandler.Get
{
    using HRsystem.Api.Database;
    using HRsystem.Api.Database.DataTables; // Assuming this namespace has Tb_Employee
    using HRsystem.Api.Database.Entities;
    using HRsystem.Api.Shared.DTO;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;



    public record SearchEmployeeQuery(string SearchTerm) : IRequest<ResponseResultDTO<EmployeeSearchDto>>;

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


    }




    public class SearchEmployeeHandler : IRequestHandler<SearchEmployeeQuery, ResponseResultDTO<EmployeeSearchDto>>
    {
        private readonly DBContextHRsystem _db;

        private readonly UserManager<ApplicationUser> _userManager;



        public SearchEmployeeHandler(DBContextHRsystem db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<ResponseResultDTO<EmployeeSearchDto>> Handle(SearchEmployeeQuery request, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(request.SearchTerm))
                return new ResponseResultDTO<EmployeeSearchDto>
                {
                    Success = false,
                    Message = "Search term is empty."
                };

            var search = request.SearchTerm.Trim();

            // Query employee with related data
            var employee = await _db.TbEmployees
                .AsNoTracking()
                .Include(e => e.JobTitle)        // Assuming navigation property exists
                 .Include(e => e.Department)
                .Include(e => e.TbEmployeeWorkLocations)    // Many-to-many
                .ThenInclude(wl => wl.WorkLocation)
                .Include(e => e.TbEmployeeProjects)
                .ThenInclude(p => p.Project)
                .Where(e => e.NationalId == search
                         || e.EmployeeCodeHr == search
                         || e.EmployeeCodeFinance == search)
                .FirstOrDefaultAsync(ct);

            if (employee == null)
            {
                return new ResponseResultDTO<EmployeeSearchDto>
                {
                    Success = false,
                    Message = "Employee not found."
                };

            }
            else
            {



                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.EmployeeId == employee.EmployeeId);

                var roleIds = await _db.UserRoles
                  .Select(ur => ur.RoleId)
                  .ToListAsync();


                var dto = new EmployeeSearchDto
                {
                    EmployeeId = employee.EmployeeId,
                    CompanyId = employee.CompanyId,
                    FullNameEnglish = employee.EnglishFullName,
                    FullNameArabic = employee.ArabicFullName,
                    NationalId = employee.NationalId,
                    EmployeeCodeHr = employee.EmployeeCodeHr,
                    EmployeeCodeFinance = employee.EmployeeCodeFinance,
                    UserId = user != null ? user.Id : 0,
                    UserName = user != null ? user.UserName : string.Empty,
                    UserFullName = user != null ? user.UserName : string.Empty,
                    PhotoUrl = employee.EmployeePhotoPath,
                    DepartmentName = $"{employee.Department.DepartmentName.ar} - ({employee.Department.DepartmentName.en})",
                    MobileNumber = employee.PrivateMobile,
                    JobTitle = employee.JobTitle?.TitleName.ar ?? string.Empty,
                    WorkLocations = employee.TbEmployeeWorkLocations?.Select(w => w.WorkLocation.LocationName.ar).ToList() ?? new List<string>(),
                    Projects = employee.TbEmployeeProjects?.Select(p => p.Project.ProjectName.ar).ToList() ?? new List<string>()
                    ,
                    RoleIds = roleIds
                };


                return new ResponseResultDTO<EmployeeSearchDto>
                {
                    Success = true,
                    Message = "Employee retrieved successfully.",
                    Data = dto
                };
            }

        }
    }

}
