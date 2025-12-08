using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.Scheduling.Shift.GetAllShifts;
using HRsystem.Api.Features.ShiftRule.CreateShiftRule;
using HRsystem.Api.Services.CurrentUser;
using MediatR;

namespace HRsystem.Api.Features.ShiftRule.CreateShiftRule
{
    public record CreateShiftRuleMappedCommand(
        string? ShiftRuleName,
        int? JobLevelId,
        int? DepartmentId,
        int? JobTitleId,
        int? GovId,
        int? CityId,
        int? WorkingLocationId,
        int? ProjectId,
        List<ShiftDto> Shifts,
        int? Priority,
        int CompanyId
        
    ) : IRequest<TbShiftRule>;

    public class CreateShiftRuleMappedCommandHandler : IRequestHandler<CreateShiftRuleMappedCommand, TbShiftRule>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;
        public CreateShiftRuleMappedCommandHandler(DBContextHRsystem db ,ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUser = currentUserService;
        }
        public async Task<TbShiftRule> Handle(CreateShiftRuleMappedCommand request, CancellationToken ct)
        {
            var entity = new TbShiftRule
            {
                JobLevelId = request.JobLevelId,
                ShiftRuleName = request.ShiftRuleName,
                JobTitleId = request.JobTitleId,
                WorkingLocationId = request.WorkingLocationId,
                GovID = request.GovId ,
                CityID = request.CityId ,
                ProjectId = request.ProjectId,
               // ShiftId = request.ShiftId,
                Priority = request.Priority,
                CompanyId = request.CompanyId,
                CreatedBy = _currentUser.UserId,
                CreatedAt = DateTime.UtcNow,
            };

            _db.TbShiftRules.Add(entity);
            await _db.SaveChangesAsync(ct);

            TbShiftRuleMappng shiftRuleMapping = new TbShiftRuleMappng();
            foreach (var shift in request.Shifts)
            {
                shiftRuleMapping = new TbShiftRuleMappng
                {
                    ShiftRuleId = entity.RuleId,
                    ShiftId = shift.ShiftId,
                    
                };
                _db.TbShiftRuleMappngs.Add(shiftRuleMapping);
                await _db.SaveChangesAsync(ct);
            }
             
            return entity;
        }
    }

}

public class CreateShiftRuleMappedValidator : AbstractValidator<CreateShiftRuleMappedCommand>
{
    public CreateShiftRuleMappedValidator()
    {
        RuleFor(x => x.Shifts.Count).GreaterThan(0).WithMessage("Shifts are required");
        RuleFor(x => x.CompanyId).GreaterThan(0).WithMessage("CompanyId is required");
    }
}
