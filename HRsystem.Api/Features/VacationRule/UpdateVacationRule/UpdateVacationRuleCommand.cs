using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static HRsystem.Api.Enums.EnumsList;

namespace HRsystem.Api.Features.VacationRule.UpdateVacationRule
{
    public record UpdateVacationRuleCommand(
        int RuleId,
        int VacationTypeId,
        int? MinAge,
        int? MaxAge,
        int? MinServiceYears,
        int? MaxServiceYears,
        EnumGenderType Gender,
        EnumReligionType Religion,
        int YearlyBalance,
        bool? Prorate,
        string RuleName
    ) : IRequest<TbVacationRule?>;

    public class UpdateVacationRuleHandler : IRequestHandler<UpdateVacationRuleCommand, TbVacationRule?>
    {
        private readonly DBContextHRsystem _db;
        public UpdateVacationRuleHandler(DBContextHRsystem db) => _db = db;

        public async Task<TbVacationRule?> Handle(UpdateVacationRuleCommand request, CancellationToken ct)
        {
            var entity = await _db.TbVacationRules.FirstOrDefaultAsync(r => r.RuleId == request.RuleId, ct);
            if (entity == null) return null;

            entity.VacationTypeId = request.VacationTypeId;
            entity.MinAge = request.MinAge;
            entity.MaxAge = request.MaxAge;
            entity.MinServiceYears = request.MinServiceYears;
            entity.MaxServiceYears = request.MaxServiceYears;
            entity.Gender = request.Gender;
            entity.Religion = request.Religion;
            entity.YearlyBalance = request.YearlyBalance;
            entity.Prorate = request.Prorate;
            entity.RuleName = request.RuleName;

            await _db.SaveChangesAsync(ct);
            return entity;
        }
    }
}
