using AutoMapper;
using Google.Cloud.AIPlatform.V1;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Scheduling.VacationRule.GetAllVacationRules
{
    public record GetAllVacationRulesQuery(int CompanyID) : IRequest<List<VacationRuleDto>>;

    public class GetAllVacationRulesHandler : IRequestHandler<GetAllVacationRulesQuery, List<VacationRuleDto>>
    {
        private readonly DBContextHRsystem _db;
        private readonly IMapper _mapper;
        public GetAllVacationRulesHandler(DBContextHRsystem db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<VacationRuleDto>> Handle(GetAllVacationRulesQuery request, CancellationToken ct)
        {
            var v = await _db.TbVacationRules.Where(c => c.CompanyId == request.CompanyID).ToListAsync(ct);

            if (v.Any())
            {
                return _mapper.Map<List<VacationRuleDto>>(v);
            }
            else
            {
                return new List<VacationRuleDto>();
            }


        }
    }
}
