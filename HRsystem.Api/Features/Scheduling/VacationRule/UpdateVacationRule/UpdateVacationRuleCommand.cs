using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static HRsystem.Api.Enums.EnumsList;

namespace HRsystem.Api.Features.Scheduling.VacationRule.UpdateVacationRule
{
    public record UpdateVacationRuleCommand(
        int RuleId,
        int CompanyId,
        string? VacationRuleName,
        int VacationTypeId,
        int? MinAge,
        int? MaxAge,
        int? MinServiceYears,
        int? MaxServiceYears,
        EnumGenderType Gender,
        EnumReligionType Religion,
        int YearlyBalance,
        bool? Prorate 
    ) : IRequest<TbVacationRule?>;

    public class UpdateVacationRuleHandler : IRequestHandler<UpdateVacationRuleCommand, TbVacationRule?>
    {
        private readonly DBContextHRsystem _db;
        public UpdateVacationRuleHandler(DBContextHRsystem db) => _db = db;

        public async Task<TbVacationRule?> Handle(UpdateVacationRuleCommand request, CancellationToken ct)
        {
            var entity = await _db.TbVacationRules.FirstOrDefaultAsync(r => r.RuleId == request.RuleId, ct);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Vacation Rule with ID {request.RuleId} not found.");
            }

            entity.VacationTypeId = request.VacationTypeId;
            entity.CompanyId= request.CompanyId;
            entity.VacationRuleName = request.VacationRuleName;
            entity.MinAge = request.MinAge;
            entity.MaxAge = request.MaxAge;
            entity.MinServiceYears = request.MinServiceYears;
            entity.MaxServiceYears = request.MaxServiceYears;
            entity.Gender = request.Gender;
            entity.Religion = request.Religion;
            entity.YearlyBalance = request.YearlyBalance;
            entity.Prorate = request.Prorate;
           

            await _db.SaveChangesAsync(ct);
            return entity;
        }
    }
}


namespace HRsystem.Api.Features.Scheduling.VacationRule.UpdateVacationRule
{
    public class UpdateVacationRuleValidator : AbstractValidator<UpdateVacationRuleCommand>
    {
        public UpdateVacationRuleValidator()
        {
            RuleFor(x => x.RuleId)
                .GreaterThan(0).WithMessage("RuleId is required and must be greater than 0");

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

            RuleFor(x => x.VacationRuleName
            )
                .NotEmpty().WithMessage("VacationRuleName is required")
                .MaximumLength(100).WithMessage("VacationRuleName must not exceed 100 characters");
        }
    }




}
