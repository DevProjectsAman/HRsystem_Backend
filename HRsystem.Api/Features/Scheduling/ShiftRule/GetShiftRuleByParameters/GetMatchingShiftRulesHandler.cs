using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.ShiftRule.GetShiftRuleByParameters
{
    

    public record GetMatchingShiftRulesQuery(
        int CompanyId,
        int? GovId,
        int? CityId,
        int? JobLevelId,
        int? JobTitleId,
        int? WorkingLocationId,
        int? ProjectId
    ) : IRequest<ResponseResultDTO<List<ShiftRuleDto>>>;

      public record ShiftRuleDto(
        int RuleId,
        string? ShiftRuleName,
        int? GovId,
        int? CityId,
        int? JobLevelId,
        int? JobTitleId,
        int? WorkingLocationId,
        int? ProjectId,
        int ShiftId,
        int? Priority
    );
    

    public class GetMatchingShiftRulesHandler
        : IRequestHandler<GetMatchingShiftRulesQuery, ResponseResultDTO<List<ShiftRuleDto>>>
    {
        private readonly DBContextHRsystem _db;

        public GetMatchingShiftRulesHandler(DBContextHRsystem db)
        {
            _db = db;
        }

        public async Task<ResponseResultDTO<List<ShiftRuleDto>>> Handle(
            GetMatchingShiftRulesQuery request,
            CancellationToken cancellationToken)
        {
            var baseQuery = _db.TbShiftRules
                .Where(r => r.CompanyId == request.CompanyId);

            // define progressive filter steps
            var attempts = new List<Func<IQueryable<TbShiftRule>, IQueryable<TbShiftRule>>>
        {
            // all params
            q => q.Where(r => r.GovID == request.GovId
                           && r.CityID == request.CityId
                           && r.JobTitleId == request.JobTitleId
                           && r.WorkingLocationId == request.WorkingLocationId
                           && r.ProjectId == request.ProjectId),
            // drop ProjectId
            q => q.Where(r => r.GovID == request.GovId
                           && r.CityID == request.CityId
                           && r.JobTitleId == request.JobTitleId
                           && r.WorkingLocationId == request.WorkingLocationId),
            // drop WorkingLocationId
            q => q.Where(r => r.GovID == request.GovId
                           && r.CityID == request.CityId
                           && r.JobTitleId == request.JobTitleId),
            // drop JobTitleId
            q => q.Where(r => r.GovID == request.GovId
                           && r.CityID == request.CityId),
            // drop CityId
            q => q.Where(r => r.GovID == request.GovId)
        };

            List<TbShiftRule>? rules = null;

            foreach (var filter in attempts)
            {
                rules = await filter(baseQuery)
                    .OrderBy(r => r.Priority)
                    .ToListAsync(cancellationToken);

                if (rules.Any())
                    break;
            }

            if (rules == null || rules.Count == 0)
            {
                return new ResponseResultDTO<List<ShiftRuleDto>>
                {
                    Success = false,
                    Message = "No matching shift rules found"
                };
            }

            var result = rules.Select(r => new ShiftRuleDto(
                r.RuleId,
                r.ShiftRuleName,
                r.GovID,
                r.CityID,
                r.JobTitleId,
                r.JobLevelId,
                r.WorkingLocationId,
                r.ProjectId,
                r.ShiftId,
                r.Priority
            )).ToList();


            
            return new ResponseResultDTO<List<ShiftRuleDto>>
            {
                Success = true,
                Data = result,
                Message = "Matching shift rules found"
            };
        }
    }

}
