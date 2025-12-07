using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.WorkDaysRules.GetAllWorkDaysRules
{
    public class WorkDaysRuleDto
    {
        public int WorkDaysRuleId { get; set; }
        public string? WorkDaysRuleName { get; set; }
        public int? GovID { get; set; }
        public string? GovName { get; set; }
        public int? CityID { get; set; }
        public string? CityName { get; set; }
        public int? JobTitleId { get; set; }
        public string? JobTitleName { get; set; }

        public int? WorkingLocationId { get; set; }
        public string? WorkingLocationName { get; set; }

        public int? ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public int WorkDaysId { get; set; }
        public string? WorkDaysName { get; set; }

        public int? Priority { get; set; }
        public int CompanyId { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }

        public int? JobLevelId { get; set; }
        public string? JobLevelName { get; set; }

        public DateTime? CreatedAt { get; set; }
    }

    // Get All
    public record GetAllWorkDaysRulesQuery(int CompanyId) : IRequest<List<WorkDaysRuleDto>>;

    // Get One
    public record GetWorkDaysRuleByIdQuery(int Id) : IRequest<WorkDaysRuleDto?>;

    // Create
    public record CreateWorkDaysRuleCommand(
        string? WorkDaysRuleName,
        int? GovID,
        int? CityID,
        int? JobLevelId,
        int? JobTitleId,
        int? WorkingLocationId,
        int? ProjectId,
        int WorkDaysId,
        int? Priority,
         int? Department,
        int CompanyId
      
    ) : IRequest<WorkDaysRuleDto>;

    // Update
    public record UpdateWorkDaysRuleCommand(
        int WorkDaysRuleId,
        string? WorkDaysRuleName,
        int? GovID,
        int? CityID,
         int? JobLevelId,
        int? JobTitleId,
        int? WorkingLocationId,
        int? ProjectId,
        int WorkDaysId,
        int? Priority,
        int? Department,
        int CompanyId
        
    ) : IRequest<WorkDaysRuleDto?>;

    // Delete
    public record DeleteWorkDaysRuleCommand(int Id) : IRequest<bool>;

    // Get All
    public class GetAllWorkDaysRulesHandler
        : IRequestHandler<GetAllWorkDaysRulesQuery, List<WorkDaysRuleDto>>
    {
        private readonly DBContextHRsystem _db;
        public GetAllWorkDaysRulesHandler(DBContextHRsystem db) => _db = db;

        public async Task<List<WorkDaysRuleDto>> Handle(GetAllWorkDaysRulesQuery request, CancellationToken ct)
        {
            return await _db.TbWorkDaysRules
                .Include(r => r.WorkingLocation)
                .Include(r => r.Project)
                .Include(r => r.JobLevel)
                .Include(r => r.JobTitle)
                .Include(r => r.Gov)
                .Include(r => r.City)
                .Include(r => r.Department)
                .Include(r => r.WorkDays)
                 .OrderBy(r => r.WorkDaysRuleId)   // ✅ Sort here in SQL

                .Select(r => new WorkDaysRuleDto
                {
                    WorkDaysRuleId = r.WorkDaysRuleId,
                    WorkDaysRuleName = r.WorkDaysRuleName,
                    GovID = r.GovID,
                    GovName = r.Gov.GovName,
                    CityID = r.CityID,
                    CityName = r.City.CityName,
                    JobTitleId = r.JobTitleId,
                    JobTitleName = r.JobTitle.TitleName.ar,
                    WorkingLocationId = r.WorkingLocationId,
                    WorkingLocationName = r.WorkingLocation.LocationName.ar,
                    ProjectId = r.ProjectId,
                    ProjectName = r.Project.ProjectName.ar,
                    WorkDaysId = r.WorkDaysId,
                    WorkDaysName =                     string.Join(", ", r.WorkDays.WorkDaysNames),
                    Priority = r.Priority,
                    CompanyId = r.CompanyId,
                    CreatedAt = r.CreatedAt,
                    DepartmentId = r.DepartmentId,
                    DepartmentName = r.Department.DepartmentName.ar,
                    JobLevelId = r.JobLevelId,
                    JobLevelName = r.JobLevel.JobLevelDesc


                })
                .ToListAsync(ct);
        }
    }

    // Get One
    public class GetWorkDaysRuleByIdHandler
        : IRequestHandler<GetWorkDaysRuleByIdQuery, WorkDaysRuleDto?>
    {
        private readonly DBContextHRsystem _db;
        public GetWorkDaysRuleByIdHandler(DBContextHRsystem db) => _db = db;

        public async Task<WorkDaysRuleDto?> Handle(GetWorkDaysRuleByIdQuery request, CancellationToken ct)
        {
            return await _db.TbWorkDaysRules
                .Where(r => r.WorkDaysRuleId == request.Id)
                .Select(r => new WorkDaysRuleDto
                {
                    WorkDaysRuleId = r.WorkDaysRuleId,
                    WorkDaysRuleName = r.WorkDaysRuleName,
                    GovID = r.GovID,
                    CityID = r.CityID,
                    JobTitleId = r.JobTitleId,
                    WorkingLocationId = r.WorkingLocationId,
                    ProjectId = r.ProjectId,
                    WorkDaysId = r.WorkDaysId,
                    Priority = r.Priority,
                    CompanyId = r.CompanyId,
                    CreatedAt = r.CreatedAt,

                })
                .FirstOrDefaultAsync(ct);
        }
    }

    // Create
    public class CreateWorkDaysRuleHandler
        : IRequestHandler<CreateWorkDaysRuleCommand, WorkDaysRuleDto>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;

        public CreateWorkDaysRuleHandler(DBContextHRsystem db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUser = currentUserService;

        }

        public async Task<WorkDaysRuleDto> Handle(CreateWorkDaysRuleCommand request, CancellationToken ct)
        {
            var entity = new TbWorkDaysRule
            {
                
                GovID = request.GovID,
                WorkDaysRuleName = request.WorkDaysRuleName,
                CityID = request.CityID,
                JobTitleId = request.JobTitleId,
                WorkingLocationId = request.WorkingLocationId,
                ProjectId = request.ProjectId,
                WorkDaysId = request.WorkDaysId,
                Priority = request.Priority,
                CompanyId = request.CompanyId,
                CreatedBy = _currentUser.UserId,
                CreatedAt = DateTime.UtcNow

            };

            _db.TbWorkDaysRules.Add(entity);
            await _db.SaveChangesAsync(ct);

            return new WorkDaysRuleDto
            {
                WorkDaysRuleId = entity.WorkDaysRuleId,
                WorkDaysRuleName = entity.WorkDaysRuleName,
                GovID = entity.GovID,
                CityID = entity.CityID,
                JobTitleId = entity.JobTitleId,
                WorkingLocationId = entity.WorkingLocationId,
                ProjectId = entity.ProjectId,
                WorkDaysId = entity.WorkDaysId,
                Priority = entity.Priority,
                CompanyId = entity.CompanyId,
                CreatedAt = entity.CreatedAt,

            };
        }
    }

    // Update
    public class UpdateWorkDaysRuleHandler
        : IRequestHandler<UpdateWorkDaysRuleCommand, WorkDaysRuleDto?>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;

        public UpdateWorkDaysRuleHandler(DBContextHRsystem db, ICurrentUserService currentUserService)
        {
            _db = db;

            _currentUser = currentUserService;
        }

        public async Task<WorkDaysRuleDto?> Handle(UpdateWorkDaysRuleCommand request, CancellationToken ct)
        {
            var entity = await _db.TbWorkDaysRules.FindAsync(new object[] { request.WorkDaysRuleId }, ct);
            if (entity == null) return null;

            entity.GovID = request.GovID;
            entity.WorkDaysRuleName = request.WorkDaysRuleName;
            entity.CityID = request.CityID;
            entity.JobTitleId = request.JobTitleId;
            entity.WorkingLocationId = request.WorkingLocationId;
            entity.ProjectId = request.ProjectId;
            entity.WorkDaysId = request.WorkDaysId;
            entity.Priority = request.Priority;
            entity.CompanyId = request.CompanyId;
            entity.CreatedBy = _currentUser.UserId;

            await _db.SaveChangesAsync(ct);

            return new WorkDaysRuleDto
            {
                WorkDaysRuleId = entity.WorkDaysRuleId,
                GovID = entity.GovID,
                CityID = entity.CityID,
                JobTitleId = entity.JobTitleId,
                WorkingLocationId = entity.WorkingLocationId,
                ProjectId = entity.ProjectId,
                WorkDaysId = entity.WorkDaysId,
                Priority = entity.Priority,
                CompanyId = entity.CompanyId,
                CreatedAt = entity.CreatedAt,
                
            };
        }
    }

    // Delete
    public class DeleteWorkDaysRuleHandler
        : IRequestHandler<DeleteWorkDaysRuleCommand, bool>
    {
        private readonly DBContextHRsystem _db;
        public DeleteWorkDaysRuleHandler(DBContextHRsystem db) => _db = db;

        public async Task<bool> Handle(DeleteWorkDaysRuleCommand request, CancellationToken ct)
        {
            var entity = await _db.TbWorkDaysRules.FindAsync(new object[] { request.Id }, ct);
            if (entity == null) return false;

            _db.TbWorkDaysRules.Remove(entity);
            await _db.SaveChangesAsync(ct);

            return true;
        }
    }

}
