using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using static HRsystem.Api.Enums.EnumsList;

namespace HRsystem.Api.Features.Scheduling.VacationRule.CreateVacationRule
{
    public record CreateVacationRuleCommand(
        int VacationTypeId,
        string? VacationRuleName,
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
                VacationRuleName = request.VacationRuleName,
                MinAge = request.MinAge,
                MaxAge = request.MaxAge,
                MinServiceYears = request.MinServiceYears,
                MaxServiceYears = request.MaxServiceYears,
                Gender = request.Gender,
                Religion = request.Religion,
                YearlyBalance = request.YearlyBalance,
                Prorate = request.Prorate,
                VacationRuleName = request.RuleName
            };

            _db.TbVacationRules.Add(entity);
            await _db.SaveChangesAsync(ct);

            return entity;
        }
    }

}


namespace HRsystem.Api.Features.Scheduling.VacationRule.CreateVacationRule
{
    public class CreateVacationRuleValidator : AbstractValidator<CreateVacationRuleCommand>
    {
        public CreateVacationRuleValidator()
        {
            RuleFor(x => x.VacationTypeId)
                .GreaterThan(0).WithMessage("VacationTypeId is required and must be greater than 0");

            RuleFor(x => x.MinAge)
                .GreaterThanOrEqualTo(18).When(x => x.MinAge.HasValue)
                .WithMessage("MinAge must be at least 18");

            RuleFor(x => x.MaxAge)
                .GreaterThan(x => x.MinAge)
                .When(x => x.MaxAge.HasValue && x.MinAge.HasValue)
                .WithMessage("MaxAge must be greater than MinAge");

            RuleFor(x => x.MinServiceYears)
                .GreaterThanOrEqualTo(0).When(x => x.MinServiceYears.HasValue)
                .WithMessage("MinServiceYears must be non-negative");

            RuleFor(x => x.MaxServiceYears)
                .GreaterThan(x => x.MinServiceYears)
                .When(x => x.MaxServiceYears.HasValue && x.MinServiceYears.HasValue)
                .WithMessage("MaxServiceYears must be greater than MinServiceYears");

            RuleFor(x => x.YearlyBalance)
                .GreaterThanOrEqualTo(0).WithMessage("YearlyBalance must be >= 0");

            RuleFor(x => x.RuleName)
                .NotEmpty().WithMessage("VacationRuleName is required")
                .MaximumLength(100).WithMessage("VacationRuleName must not exceed 100 characters");
        }
    }
}

