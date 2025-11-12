using AutoMapper;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.Scheduling.Shift.GetAllShifts;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Scheduling.ShiftRule.GetAllShiftRules
{
    public record GetAllShiftRulesQuery(int CompanyId) : IRequest<List<ShiftRuleDto>>;

    public class GetAllShiftRulesHandler : IRequestHandler<GetAllShiftRulesQuery, List<ShiftRuleDto>>
    {
        private readonly DBContextHRsystem _db;
        private readonly IMapper _mapper;

        public GetAllShiftRulesHandler(DBContextHRsystem db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<ShiftRuleDto>> Handle(GetAllShiftRulesQuery request, CancellationToken ct)
        {
            return await _db.TbShiftRules
                .Include(r => r.Gov)
                .Include(r => r.City)
                .Include(r => r.WorkingLocation)
                .Include(r => r.Project)
                .Include(r => r.JobLevel)
                .Include(r => r.JobTitle)
                .Include(r => r.Department)
                .Include(r => r.Shift)
                                .Where(r => r.CompanyId == request.CompanyId)
                .Select(r => new ShiftRuleDto
                {
                    RuleId = r.RuleId,
                    ShiftRuleName = r.ShiftRuleName,
                    GovID = r.GovID,
                    CityID = r.CityID,
                    JobLevelId = r.JobLevelId,
                    JobTitleId = r.JobTitleId,
                    WorkingLocationId = r.WorkingLocationId,
                    ProjectId = r.ProjectId,
                    shiftDto = _mapper.Map<ShiftDto>(r.Shift),
                    Priority = r.Priority,
                    CompanyId = r.CompanyId,
                    DeparmentId = r.DepartmentId,

                    // ✅ include related names (optional, depends on your DTO)
                    GovName = r.Gov != null ? r.Gov.GovName : null,
                    CityName = r.City != null ? r.City.CityName : null,
                    WorkLocationName = r.WorkingLocation != null ? r.WorkingLocation.LocationName : null,
                    ProjectName = r.Project != null ? r.Project.ProjectName : null,
                    JobLevelName = r.JobLevel != null ? r.JobLevel.JobLevelDesc : null,
                    TitleName = r.JobTitle != null ? r.JobTitle.TitleName : null,
                    //  ShiftName = r.Shift != null ? r.Shift.ShiftName : null,
                    DepartmentName = r.Department != null ? r.Department.DepartmentName : null,


                })
                .ToListAsync(ct);
        }



    }

    // Define this class in your DTOs folder
    public class ShiftRuleDto
    {
        public int RuleId { get; set; }

        public string? ShiftRuleName { get; set; }

        // Location/Hierarchy Fields
        public int? GovID { get; set; }
        public string? GovName { get; set; }
        public int? CityID { get; set; }
        public string? CityName { get; set; }
        public int? WorkingLocationId { get; set; }
        public LocalizedData? WorkLocationName { get; set; } = null;

        public int? ProjectId { get; set; }
        public LocalizedData? ProjectName { get; set; } = null;

        // Job Fields
        public int? JobLevelId { get; set; }
        public string? JobLevelName { get; set; } = null!;
        public int? DeparmentId { get; set; }
        public LocalizedData? DepartmentName { get; set; } = null!;
        public int? JobTitleId { get; set; }
        public LocalizedData? TitleName { get; set; } = null!;

        // Shift Details
        //public int ShiftId { get; set; }
        //public LocalizedData? ShiftName { get; set; } = null!;

        public ShiftDto shiftDto { get; set; } = null!;

        public int? Priority { get; set; }

        // Other fields (optional)
        public int CompanyId { get; set; }

    }
}
