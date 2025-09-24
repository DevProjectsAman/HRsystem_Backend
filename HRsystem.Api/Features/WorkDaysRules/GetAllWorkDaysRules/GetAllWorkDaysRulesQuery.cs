using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.WorkDaysRules.GetAllWorkDaysRules
{
    public class WorkDaysRuleDto
    {
        public int WorkDaysRuleId { get; set; }
        public int? GovID { get; set; }
        public int? CityID { get; set; }
        public int? JobTitleId { get; set; }
        public int? WorkingLocationId { get; set; }
        public int? ProjectId { get; set; }
        public int WorkDaysId { get; set; }
        public int? Priority { get; set; }
        public int CompanyId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    // Get All
    public record GetAllWorkDaysRulesQuery() : IRequest<List<WorkDaysRuleDto>>;

    // Get One
    public record GetWorkDaysRuleByIdQuery(int Id) : IRequest<WorkDaysRuleDto?>;

    // Create
    public record CreateWorkDaysRuleCommand(
        int? GovID,
        int? CityID,
        int? JobTitleId,
        int? WorkingLocationId,
        int? ProjectId,
        int WorkDaysId,
        int? Priority,
        int CompanyId,
        int? CreatedBy
    ) : IRequest<WorkDaysRuleDto>;

    // Update
    public record UpdateWorkDaysRuleCommand(
        int WorkDaysRuleId,
        int? GovID,
        int? CityID,
        int? JobTitleId,
        int? WorkingLocationId,
        int? ProjectId,
        int WorkDaysId,
        int? Priority,
        int CompanyId,
        int? CreatedBy
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
                .Select(r => new WorkDaysRuleDto
                {
                    WorkDaysRuleId = r.WorkDaysRuleId,
                    GovID = r.GovID,
                    CityID = r.CityID,
                    JobTitleId = r.JobTitleId,
                    WorkingLocationId = r.WorkingLocationId,
                    ProjectId = r.ProjectId,
                    WorkDaysId = r.WorkDaysId,
                    Priority = r.Priority,
                    CompanyId = r.CompanyId,
                    CreatedAt = r.CreatedAt,
                    CreatedBy = r.CreatedBy
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
                    GovID = r.GovID,
                    CityID = r.CityID,
                    JobTitleId = r.JobTitleId,
                    WorkingLocationId = r.WorkingLocationId,
                    ProjectId = r.ProjectId,
                    WorkDaysId = r.WorkDaysId,
                    Priority = r.Priority,
                    CompanyId = r.CompanyId,
                    CreatedAt = r.CreatedAt,
                    CreatedBy = r.CreatedBy
                })
                .FirstOrDefaultAsync(ct);
        }
    }

    // Create
    public class CreateWorkDaysRuleHandler
        : IRequestHandler<CreateWorkDaysRuleCommand, WorkDaysRuleDto>
    {
        private readonly DBContextHRsystem _db;
        public CreateWorkDaysRuleHandler(DBContextHRsystem db) => _db = db;

        public async Task<WorkDaysRuleDto> Handle(CreateWorkDaysRuleCommand request, CancellationToken ct)
        {
            var entity = new TbWorkDaysRule
            {
                GovID = request.GovID,
                CityID = request.CityID,
                JobTitleId = request.JobTitleId,
                WorkingLocationId = request.WorkingLocationId,
                ProjectId = request.ProjectId,
                WorkDaysId = request.WorkDaysId,
                Priority = request.Priority,
                CompanyId = request.CompanyId,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            _db.TbWorkDaysRules.Add(entity);
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
                CreatedBy = entity.CreatedBy
            };
        }
    }

    // Update
    public class UpdateWorkDaysRuleHandler
        : IRequestHandler<UpdateWorkDaysRuleCommand, WorkDaysRuleDto?>
    {
        private readonly DBContextHRsystem _db;
        public UpdateWorkDaysRuleHandler(DBContextHRsystem db) => _db = db;

        public async Task<WorkDaysRuleDto?> Handle(UpdateWorkDaysRuleCommand request, CancellationToken ct)
        {
            var entity = await _db.TbWorkDaysRules.FindAsync(new object[] { request.WorkDaysRuleId }, ct);
            if (entity == null) return null;

            entity.GovID = request.GovID;
            entity.CityID = request.CityID;
            entity.JobTitleId = request.JobTitleId;
            entity.WorkingLocationId = request.WorkingLocationId;
            entity.ProjectId = request.ProjectId;
            entity.WorkDaysId = request.WorkDaysId;
            entity.Priority = request.Priority;
            entity.CompanyId = request.CompanyId;
            entity.CreatedBy = request.CreatedBy;

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
                CreatedBy = entity.CreatedBy
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
