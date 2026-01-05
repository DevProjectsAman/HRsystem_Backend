using AutoMapper;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.Scheduling.Shift.GetAllShifts;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HRsystem.Api.Features.Scheduling.ShiftRule.GetShiftRuleByParameters
{


    public record GetMatchingShiftRulesQuery(
        int CompanyId,
        int? GovId,
        int? CityId,
        int? DepartmentId,
        int? JobLevelId,
        int? JobTitleId,
        int? WorkingLocationId,
        int? ProjectId
    ) : IRequest<List<ShiftRuleDto>>;

    public record ShiftRuleDto(
      int RuleId,
      string? ShiftRuleName,
      int? GovId,
      string? GovName,
      int? CityId,
      string? CityName,
      int? JobLevelId,
      string? JobLevelName,
      int? JobTitleId,
      LocalizedData? JobTitleName,
      int? WorkingLocationId,
      LocalizedData? WorkingLocationName,
      int? ProjectId,
      LocalizedData? ProjectName,
      ShiftDto Shift,
      int? Priority
  );



    public class GetMatchingShiftRulesHandler
        : IRequestHandler<GetMatchingShiftRulesQuery, List<ShiftRuleDto>>
    {
        private readonly DBContextHRsystem _db;
        private readonly IMapper _mapper;

        public GetMatchingShiftRulesHandler(DBContextHRsystem db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<ShiftRuleDto>> Handle(
     GetMatchingShiftRulesQuery request,
     CancellationToken cancellationToken)
        {
            var query = _db.TbShiftRules
                .Include(r => r.Shift)
                .Include(r => r.Gov)
                .Include(r => r.City)
                .Include(r => r.JobLevel)
                .Include(r => r.JobTitle)
                .Include(r => r.WorkingLocation)
                .Include(r => r.Project)
                .Include(r => r.Department)
                .Where(r => r.CompanyId == request.CompanyId);

            // 1️⃣ Score each rule
            var scored = await query
                .Select(r => new
                {
                    Rule = r,
                    Score =
                        (request.GovId != null && r.GovID == request.GovId ? 1 : 0) +
                        (request.CityId != null && r.CityID == request.CityId ? 1 : 0) +
                        (request.JobLevelId != null && r.JobLevelId == request.JobLevelId ? 1 : 0) +
                        (request.JobTitleId != null && r.JobTitleId == request.JobTitleId ? 1 : 0) +
                        (request.WorkingLocationId != null && r.WorkingLocationId == request.WorkingLocationId ? 1 : 0) +
                        (request.ProjectId != null && r.ProjectId == request.ProjectId ? 1 : 0) +
                        (request.DepartmentId != null && r.DepartmentId == request.DepartmentId ? 1 : 0)
                })
                .ToListAsync(cancellationToken);

            if (!scored.Any())
            {
                throw new Exception("No shift rules found for this company");
                
            }

            // 2️⃣ Find highest score
            var maxScore = scored.Max(x => x.Score);

            // 3️⃣ If the best score is > 0, return those rules
            if (maxScore > 0)
            {
                var best = scored
                    .Where(x => x.Score == maxScore)
                    .OrderBy(x => x.Rule.Priority)
                   // .Take(1)                                     // RETURN ONLY ONE RECORD
                    .Select(x => x.Rule)
                    .ToList();

                return MapResult(best, "Best matching shift rule found");
            }

            // 4️⃣ Otherwise, try to return the DEFAULT rule (all nullable filters = null)
            var defaultRule = await query
                .Where(r =>
                    r.GovID == null &&
                    r.CityID == null &&
                    r.JobLevelId == null &&
                    r.JobTitleId == null &&
                    r.WorkingLocationId == null &&
                    r.ProjectId == null &&
                    r.DepartmentId == null
                )
                .OrderBy(r => r.Priority)
                .FirstOrDefaultAsync(cancellationToken);

            if (defaultRule != null)
            {
                List<TbShiftRule> defaultRuleList = new List<TbShiftRule> { defaultRule };


                var shift = await _db.TbShifts.FirstOrDefaultAsync(s => s.IsDefault == true);
                if (shift != null)
                {
                    defaultRuleList.FirstOrDefault().Shift = shift;
                }

 



                return MapResult(defaultRuleList, "Default shift rule returned");
            }


            throw new  Exception("No shift rule matches the parameters and no default rule exists");
            // No matching rule + No default rule
            
        }


        // Helper to map DTOs cleanly
        private List<ShiftRuleDto> MapResult(List<TbShiftRule> rules, string msg)
        {
            var data = rules.Select(r => new ShiftRuleDto(
                r.RuleId,
                r.ShiftRuleName,
                r.GovID,
                r.Gov?.GovName,
                r.CityID,
                r.City?.CityName,
                r.JobLevelId,
                r.JobLevel?.JobLevelDesc,
                r.JobTitleId,
                r.JobTitle?.TitleName,
                r.WorkingLocationId,
                r.WorkingLocation?.LocationName,
                r.ProjectId,
                r.Project?.ProjectName,
                _mapper.Map<ShiftDto>(r.Shift),
                r.Priority
            )).ToList();

            return  data ;
        }





    }

}
