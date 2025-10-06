using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.Scheduling.WorkDays;
using HRsystem.Api.Migrations;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection;
using static HRsystem.Api.Enums.EnumsList;
using static System.Net.Mime.MediaTypeNames;

namespace HRsystem.Api.Features.Scheduling.VacationRule
{

    public record GetVacationByMatchingRulesQueury(
     //int RuleId,
      int? MinAge,
      int? MaxAge,
      //int VacationTypeId,
      int? MinServiceYears,
       int? MaxServiceYears,
      int? WorkingYearsAtCompany,
      EnumGenderType Gender,
     EnumReligionType Religion,
      int YearlyBalance,
      bool? Prorate
    ) : IRequest<List<VacationRuleDto>>;
    

    public record VacationRuleDto(
      int RuleId,
      int? MinAge,
      int? MaxAge,
      int VacationTypeId,
      int? MinServiceYears,
      int? MaxServiceYears,
      int? WorkingYearsAtCompany,
      EnumGenderType Gender,
      EnumReligionType Religion,
      int YearlyBalance,
      bool? Prorate,
      int? Priority 

    );
    public class GetVacationByMatchingRulesQueuryHandler
    : IRequestHandler<GetVacationByMatchingRulesQueury, List<VacationRuleDto>>
    {
        private readonly DBContextHRsystem _db;

        public GetVacationByMatchingRulesQueuryHandler(DBContextHRsystem db)
        {
            _db = db;
        }

        //public async Task<List<VacationRuleDto>> Handle(GetVacationByMatchingRulesQueury request, CancellationToken cancellationToken)
        //{
        //    var baseQuery = _db.TbVacationRules
        //        .Where(r => r.Gender == request.Gender);

        //    var attempts = new List<Func<IQueryable<TbVacationRule>, IQueryable<TbVacationRule>>>
        //{
        //    q => q.Where(r => r.Religion == request.Religion
        //                   && r.MinAge == request.MinAge
        //                   && r.MaxAge == request.MaxAge
        //                   && r.MinServiceYears == request.MinServiceYears
        //                   && r.MaxServiceYears == request.MaxServiceYears
        //                   && r.YearlyBalance == request.YearlyBalance
        //                   && r.Prorate == request.Prorate),

        //    q => q.Where(r => r.Religion == request.Religion
        //                   //&& r.MinAge == request.MinAge
        //                   && r.MaxAge == request.MaxAge
        //                   && r.MinServiceYears == request.MinServiceYears
        //                   && r.MaxServiceYears == request.MaxServiceYears
        //                   && r.YearlyBalance == request.YearlyBalance
        //                   && r.Prorate == request.Prorate),

        //    q => q.Where(r => r.Religion == request.Religion
        //                  // && r.MinAge == request.MinAge
        //                   //&& r.MaxAge == request.MaxAge
        //                   && r.MinServiceYears == request.MinServiceYears
        //                   && r.MaxServiceYears == request.MaxServiceYears
        //                   && r.YearlyBalance == request.YearlyBalance
        //                   && r.Prorate == request.Prorate),

        //    q => q.Where(r => r.Religion == request.Religion
        //                   //&& r.MinAge == request.MinAge
        //                   //&& r.MaxAge == request.MaxAge
        //                   //&& r.MinServiceYears == request.MinServiceYears
        //                   && r.MaxServiceYears == request.MaxServiceYears
        //                   && r.YearlyBalance == request.YearlyBalance
        //                   && r.Prorate == request.Prorate),

        //                q => q.Where(r => r.Religion == request.Religion
        //                   //&& r.MinAge == request.MinAge
        //                   //&& r.MaxAge == request.MaxAge
        //                   //&& r.MinServiceYears == request.MinServiceYears
        //                   //&& r.MaxServiceYears == request.MaxServiceYears
        //                   && r.YearlyBalance == request.YearlyBalance
        //                   && r.Prorate == request.Prorate),

        //    q => q.Where(r => r.Religion == request.Religion
        //                   //&& r.MinAge == request.MinAge
        //                   //&& r.MaxAge == request.MaxAge
        //                   //&& r.MinServiceYears == request.MinServiceYears
        //                   //&& r.MaxServiceYears == request.MaxServiceYears
        //                   && r.YearlyBalance == request.YearlyBalance
        //                   //&& r.Prorate == request.Prorate
        //                   ),


        //    q => q.Where(r => r.Religion == request.Religion
        //                   //&& r.MinAge == request.MinAge
        //                   //&& r.MaxAge == request.MaxAge
        //                   //&& r.MinServiceYears == request.MinServiceYears
        //                   //&& r.MaxServiceYears == request.MaxServiceYears
        //                   ////&& r.YearlyBalance == request.YearlyBalance
        //                   //&& r.Prorate == request.Prorate
        //                   )

        //};

        //    List<TbVacationRule>? rules = null;

        //    foreach (var filter in attempts)
        //    {
        //        rules = await filter(baseQuery)
        //            .OrderBy(r => r.Priority)
        //            .ToListAsync(cancellationToken);

        //        if (rules.Any())
        //            break;
        //    }

        //    return rules?.Select(r => new VacationRuleDto(
        //      r.RuleId,
        //      r.MinAge,
        //      r.MaxAge,
        //      r.VacationTypeId,
        //      r.MinServiceYears,
        //      r.MaxServiceYears,
        //      r.WorkingYearsAtCompany,
        //      r.Gender,
        //      r.Religion,
        //      r.YearlyBalance,
        //      r.Prorate,
        //      r.Priority
        //      )).ToList() ?? new List<VacationRuleDto>();
        //}
        public async Task<List<VacationRuleDto>> Handle(GetVacationByMatchingRulesQueury request, CancellationToken cancellationToken)
        {
            var baseQuery = _db.TbVacationRules.AsQueryable();

            var query = baseQuery.Where(r =>
                // ✅ الدين (لو All أو نفس الدين)
                (r.Religion == request.Religion || r.Religion == EnumReligionType.All) &&

                // ✅ النوع (لو All أو نفس النوع)
                (r.Gender == request.Gender || r.Gender == EnumGenderType.All) &&

                // ✅ العمر داخل الرينج أو الـ rule مش محدد min/max
                (r.MinAge == null || request.MinAge >= r.MinAge) &&
                (r.MaxAge == null || request.MaxAge <= r.MaxAge) &&

                // ✅ سنوات الخدمة داخل الرينج
                (r.MinServiceYears == null || request.MinServiceYears >= r.MinServiceYears) &&
                (r.MaxServiceYears == null || request.MaxServiceYears <= r.MaxServiceYears) &&

                // ✅ عدد سنوات العمل بالشركة
                (r.WorkingYearsAtCompany == null || request.WorkingYearsAtCompany >= r.WorkingYearsAtCompany) &&

                // ✅ الرصيد السنوي (لو null يعني wildcard)
                (r.YearlyBalance == null || r.YearlyBalance == request.YearlyBalance) &&

                // ✅ prorate (لو null يعني wildcard)
                (r.Prorate == null || r.Prorate == request.Prorate)
            );

            var rules = await query.OrderBy(r => r.Priority).ToListAsync(cancellationToken);

            return rules.Select(r => new VacationRuleDto(
                r.RuleId,
                r.MinAge,
                r.MaxAge,
                r.VacationTypeId,
                r.MinServiceYears,
                r.MaxServiceYears,
                r.WorkingYearsAtCompany,
                r.Gender,
                r.Religion,
                r.YearlyBalance,
                r.Prorate,
                r.Priority
            )).ToList();
        }

    }

}