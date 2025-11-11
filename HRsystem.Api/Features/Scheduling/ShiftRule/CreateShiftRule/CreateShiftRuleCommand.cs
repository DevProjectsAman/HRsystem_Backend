using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.ShiftRule.CreateShiftRule;
using HRsystem.Api.Services.CurrentUser;
using MediatR;

namespace HRsystem.Api.Features.ShiftRule.CreateShiftRule
{
    public record CreateShiftRuleCommand(
        int? JobLevelId,
        int? DepartmentId,
        int? JobTitleId,
        int? GovId,
        int? CityId,
        int? WorkingLocationId,
        int? ProjectId,
        int ShiftId,
        int? Priority,
        int CompanyId
        
    ) : IRequest<TbShiftRule>;

    public class CreateShiftRuleHandler : IRequestHandler<CreateShiftRuleCommand, TbShiftRule>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;
        public CreateShiftRuleHandler(DBContextHRsystem db ,ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUser = currentUserService;
        }
        public async Task<TbShiftRule> Handle(CreateShiftRuleCommand request, CancellationToken ct)
        {
            var entity = new TbShiftRule
            {
                JobLevelId = request.JobLevelId,
                JobTitleId = request.JobTitleId,
                WorkingLocationId = request.WorkingLocationId,
                GovID = request.GovId ,
                CityID = request.CityId ,
                ProjectId = request.ProjectId,
                ShiftId = request.ShiftId,
                Priority = request.Priority,
                CompanyId = request.CompanyId,
                CreatedBy = _currentUser.UserId,
                CreatedAt = DateTime.Now,
                
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
