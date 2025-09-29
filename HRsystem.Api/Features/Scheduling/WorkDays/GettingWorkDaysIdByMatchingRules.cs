using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;

using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Scheduling.WorkDays
{
    public record GettingWorkDaysIdByMatchingRules
    (
    //int WorkDaysRuleId ,
     int? GovID,
     int? CityID,
     int? JobTitleId,
     int? WorkingLocationId,
     int? ProjectId,
     int WorkDaysId,
     //int? Priority ,
     int CompanyId
    ) : IRequest<List<WorkDaysRuleDto>>;

    public record WorkDaysRuleDto
    (
         int WorkDaysRuleId ,
           int? GovID ,
           int? CityID ,
            int? JobTitleId ,
            int? WorkingLocationId ,
            int? ProjectId ,
            int WorkDaysId ,
            int? Priority ,
            int CompanyId
    );

    //public record GettingWorkDaysIdByMatchingRules
    //(
    //     int? GovID,
    //     int? CityID,
    //     int? JobTitleId,
    //     int? WorkingLocationId,
    //     int? ProjectId,
    //     int WorkDaysId,
    //     int CompanyId
    //) : IRequest<List<WorkDaysRuleDto>>;  // ✅ بس داتا

    public class GettingWorkDaysIdByMatchingRulesHandler
        : IRequestHandler<GettingWorkDaysIdByMatchingRules, List<WorkDaysRuleDto>>
    {
        private readonly DBContextHRsystem _db;

        public GettingWorkDaysIdByMatchingRulesHandler(DBContextHRsystem db)
        {
            _db = db;
        }

        public async Task<List<WorkDaysRuleDto>> Handle(GettingWorkDaysIdByMatchingRules request, CancellationToken cancellationToken)
        {
            var baseQuery = _db.TbWorkDaysRules
                .Where(r => r.CompanyId == request.CompanyId);

            var attempts = new List<Func<IQueryable<TbWorkDaysRule>, IQueryable<TbWorkDaysRule>>>
        {
            q => q.Where(r => r.GovID == request.GovID
                           && r.CityID == request.CityID
                           && r.JobTitleId == request.JobTitleId
                           && r.WorkingLocationId == request.WorkingLocationId
                           && r.ProjectId == request.ProjectId),
            q => q.Where(r => r.GovID == request.GovID
                           && r.CityID == request.CityID
                           && r.JobTitleId == request.JobTitleId
                           && r.WorkingLocationId == request.WorkingLocationId),
            q => q.Where(r => r.GovID == request.GovID
                           && r.CityID == request.CityID
                           && r.JobTitleId == request.JobTitleId),
            q => q.Where(r => r.GovID == request.GovID
                           && r.CityID == request.CityID),
            q => q.Where(r => r.GovID == request.GovID)
        };

            List<TbWorkDaysRule>? rules = null;

            foreach (var filter in attempts)
            {
                rules = await filter(baseQuery)
                    .OrderBy(r => r.Priority)
                    .ToListAsync(cancellationToken);

                if (rules.Any())
                    break;
            }

            return rules?.Select(r => new WorkDaysRuleDto(
                r.WorkDaysRuleId,
                r.GovID,
                r.CityID,
                r.JobTitleId,
                r.WorkingLocationId,
                r.ProjectId,
                r.WorkDaysId,
                r.Priority,
                r.CompanyId
            )).ToList() ?? new List<WorkDaysRuleDto>();
        }
    }

}





/*
 * using HRsystem.Api.Database;
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
        int? JobTitleId,
        int? WorkingLocationId,
        int? ProjectId
    ) : IRequest<ResponseResultDTO<List<ShiftRuleDto>>>;

    public record ShiftRuleDto(
        int RuleId,
        int? GovId,
        int? CityId,
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
                r.GovID,
                r.CityID,
                r.JobTitleId,
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

 */
