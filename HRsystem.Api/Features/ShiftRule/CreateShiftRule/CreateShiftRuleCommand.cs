using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;

namespace HRsystem.Api.Features.ShiftRule.CreateShiftRule
{
    public record CreateShiftRuleCommand(
        int? JobTitleId,
        int? WorkingLocationId,
        int? ProjectId,
        int ShiftId,
        int? Priority,
        int CompanyId,
        int? CreatedBy
    ) : IRequest<TbShiftRule>;

    public class CreateShiftRuleHandler : IRequestHandler<CreateShiftRuleCommand, TbShiftRule>
    {
        private readonly DBContextHRsystem _db;
        public CreateShiftRuleHandler(DBContextHRsystem db) => _db = db;

        public async Task<TbShiftRule> Handle(CreateShiftRuleCommand request, CancellationToken ct)
        {
            var entity = new TbShiftRule
            {
                JobTitleId = request.JobTitleId,
                WorkingLocationId = request.WorkingLocationId,
                ProjectId = request.ProjectId,
                ShiftId = request.ShiftId,
                Priority = request.Priority,
                CompanyId = request.CompanyId,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            _db.TbShiftRules.Add(entity);
            await _db.SaveChangesAsync(ct);

            return entity;
        }
    }

}
