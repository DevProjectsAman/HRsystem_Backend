using AutoMapper;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.Scheduling.VacationRulesGroup.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Scheduling.VacationRulesGroup.GetBestMatchByParameters
{
    public class GetMatchingVacationRulesHandler
     : IRequestHandler<GetMatchingVacationRulesQuery, List<VacationRulesGroupDto>>
    {
        private readonly DBContextHRsystem _db;
        private readonly IMapper _mapper;

        public GetMatchingVacationRulesHandler(DBContextHRsystem db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<VacationRulesGroupDto>> Handle(
            GetMatchingVacationRulesQuery request,
            CancellationToken cancellationToken)
        {
            var query = _db.TbVacationRulesGroups
                .Include(r => r.VacationRuleDetails)
                .Where(r => r.CompanyId == request.CompanyId);

            // Score rules based on match
            var scored = await query
                .Select(r => new
                {
                    Rule = r,
                    Score =
                        (request.EmployeeAge != null &&
                         r.MinAge <= request.EmployeeAge && r.MaxAge >= request.EmployeeAge ? 1 : 0) +
                        (request.ServiceYears != null &&
                         r.MinServiceYears <= request.ServiceYears && r.MaxServiceYears >= request.ServiceYears ? 1 : 0) +
                        (request.WorkingYearsAtCompany != null &&
                         r.WorkingYearsAtCompany <= request.WorkingYearsAtCompany ? 1 : 0)
                })
                .ToListAsync(cancellationToken);

            if (!scored.Any())
                throw new Exception("No vacation rules found for this company");

            var maxScore = scored.Max(x => x.Score);

            // Return best match(es)
            if (maxScore > 0)
            {
                var best = scored
                    .Where(x => x.Score == maxScore)
                    .OrderBy(x => x.Rule.GroupId) // optional tie-breaker
                    .Select(x => x.Rule)
                    .Take(1)
                    .ToList();

                return best.Select(MapResult).ToList();
            }

            // Return default (rules with null filters)
            var defaultRule = await query
                .Where(r =>
                    r.MinAge == null &&
                    r.MaxAge == null &&
                    r.MinServiceYears == null &&

                    r.MaxServiceYears == null &&
                    r.WorkingYearsAtCompany == null)
                .OrderBy(r => r.GroupId)
                .FirstOrDefaultAsync(cancellationToken);

            if (defaultRule != null)
                return new List<VacationRulesGroupDto> { MapResult(defaultRule) };

            throw new Exception("No matching vacation rules and no default exists");
        }

        private VacationRulesGroupDto MapResult(TbVacationRulesGroup rule)
        {
            return new VacationRulesGroupDto
            {
                GroupId = rule.GroupId,
                GroupName = rule.GroupName ?? string.Empty,
                MinAge = rule.MinAge,
                MaxAge = rule.MaxAge,
                MinServiceYears = rule.MinServiceYears,
                MaxServiceYears = rule.MaxServiceYears,
                WorkingYearsAtCompany = rule.WorkingYearsAtCompany,
                VacationRuleDetails = rule.VacationRuleDetails.Select(d => new VacationRulesGroupDetailDto
                {
                    DetailId = d.DetailId,
                    VacationTypeId = d.VacationTypeId,
                    YearlyBalance = d.YearlyBalance,
                    Prorate = d.Prorate,
                    Priority = d.Priority,
                    Gender = d.Gender,
                    Religion = d.Religion
                }).ToList()
            };
        }
    }

}
