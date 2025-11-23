using AutoMapper;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.Scheduling.Shift.GetAllShifts;
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
        : IRequestHandler<GetMatchingShiftRulesQuery, ResponseResultDTO<List<ShiftRuleDto>>>
    {
        private readonly DBContextHRsystem _db;
        private readonly IMapper _mapper;

        public GetMatchingShiftRulesHandler(DBContextHRsystem db ,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ResponseResultDTO<List<ShiftRuleDto>>> Handle(
            GetMatchingShiftRulesQuery request,
            CancellationToken cancellationToken)
        {
            var baseQuery = _db.TbShiftRules
                .Include(r => r.Shift)
                .Include(r => r.Gov)
                .Include(r => r.City)
                .Include(r => r.JobTitle)
                .Include(r => r.WorkingLocation)
                .Include(r => r.Project)

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
                    Message = "No matching shift rules found",
                    StatusCode = 409
                };
            }

            var siftdto = _mapper.Map<List<ShiftDto>>(rules.Select(r => r.Shift).ToList());


            var result = rules.Select(r => new ShiftRuleDto(
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
                 _mapper.Map<ShiftDto>( r.Shift),
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
