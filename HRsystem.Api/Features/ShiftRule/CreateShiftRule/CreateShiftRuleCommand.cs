using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.ShiftRule.CreateShiftRule;
using MediatR;

namespace HRsystem.Api.Features.ShiftRule.CreateShiftRule
{
    public record CreateShiftRuleCommand(
        int? JobTitleId,
        int? GovId,
        int? CityId,
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
                GovID = request.GovId ,
                CityID = request.CityId ,
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

public class CreateShiftRuleValidator : AbstractValidator<CreateShiftRuleCommand>
{
    public CreateShiftRuleValidator()
    {
        RuleFor(x => x.ShiftId).GreaterThan(0).WithMessage("ShiftId is required");
        RuleFor(x => x.CompanyId).GreaterThan(0).WithMessage("CompanyId is required");
    }
}
