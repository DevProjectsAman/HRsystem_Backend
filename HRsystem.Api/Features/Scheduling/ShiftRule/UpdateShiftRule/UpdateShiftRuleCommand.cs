using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.ShiftRule.UpdateShiftRule;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.ShiftRule.UpdateShiftRule
{
    public record UpdateShiftRuleCommand(
        int RuleId,
        int? JobLevelId,
        int? JobTitleId,
        int? GovId,
        int? CityId,
        int? WorkingLocationId,
        int? ProjectId,
        int ShiftId,
        int? Priority,
        int CompanyId       
    ) : IRequest<TbShiftRule?>;

    public class UpdateShiftRuleHandler : IRequestHandler<UpdateShiftRuleCommand, TbShiftRule?>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUserService;
        public UpdateShiftRuleHandler(DBContextHRsystem db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }
        
                public async Task<TbShiftRule?> Handle(UpdateShiftRuleCommand request, CancellationToken ct)
        {
            var entity = await _db.TbShiftRules.FirstOrDefaultAsync(r => r.RuleId == request.RuleId, ct);
            if (entity == null)
            {
                throw new KeyNotFoundException($"ShiftRule with RuleId {request.RuleId} not found.");

            }
            

            entity.JobTitleId = request.JobTitleId;
            entity.WorkingLocationId = request.WorkingLocationId;
            entity.ProjectId = request.ProjectId;
            entity.GovID = request.GovId;
            entity.CityID = request.CityId;
            entity.ShiftId = request.ShiftId;
            entity.Priority = request.Priority;
            entity.CompanyId = request.CompanyId;
            entity.UpdatedBy = _currentUserService.UserId;
            entity.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(ct);
            return entity;
        }
    }
}

public class UpdateShiftRuleValidator : AbstractValidator<UpdateShiftRuleCommand>
{
    public UpdateShiftRuleValidator()
    {
        RuleFor(x => x.RuleId).GreaterThan(0).WithMessage("RuleId is required");
        RuleFor(x => x.ShiftId).GreaterThan(0).WithMessage("ShiftId is required");
        RuleFor(x => x.CompanyId).GreaterThan(0).WithMessage("CompanyId is required");
    }
}
