using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using static HRsystem.Api.Enums.EnumsList;

namespace HRsystem.Api.Features.VacationRule.CreateVacationRule
{
    public record CreateVacationRuleCommand(
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
    ) : IRequest<TbVacationRule>;

    public class CreateVacationRuleHandler : IRequestHandler<CreateVacationRuleCommand, TbVacationRule>
    {
        private readonly DBContextHRsystem _db;
        public CreateVacationRuleHandler(DBContextHRsystem db) => _db = db;

        public async Task<TbVacationRule> Handle(CreateVacationRuleCommand request, CancellationToken ct)
        {
            var entity = new TbVacationRule
            {
                VacationTypeId = request.VacationTypeId,
                MinAge = request.MinAge,
                MaxAge = request.MaxAge,
                MinServiceYears = request.MinServiceYears,
                MaxServiceYears = request.MaxServiceYears,
                Gender = request.Gender,
                Religion = request.Religion,
                YearlyBalance = request.YearlyBalance,
                Prorate = request.Prorate,
                RuleName = request.RuleName
            };

            _db.TbVacationRules.Add(entity);
            await _db.SaveChangesAsync(ct);

            return entity;
        }
    }

}
