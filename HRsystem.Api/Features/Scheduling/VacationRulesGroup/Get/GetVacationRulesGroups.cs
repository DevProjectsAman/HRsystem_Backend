using AutoMapper;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.Scheduling.VacationRulesGroup.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Scheduling.VacationRulesGroup.Get
{
    public record GetVacationRulesGroupsQuery() : IRequest<List<VacationRulesGroupDto>>;
    public record GetVacationRulesGroupByIdQuery(int GroupId) : IRequest<VacationRulesGroupDto?>;

    public class GetVacationRulesGroupsHandler :
        IRequestHandler<GetVacationRulesGroupsQuery, List<VacationRulesGroupDto>>,
        IRequestHandler<GetVacationRulesGroupByIdQuery, VacationRulesGroupDto?>
    {
        private readonly DBContextHRsystem _db;
        private readonly IMapper _mapper;
        public GetVacationRulesGroupsHandler(DBContextHRsystem db, IMapper mapper) { _db = db; _mapper = mapper; }

        public async Task<List<VacationRulesGroupDto>> Handle(GetVacationRulesGroupsQuery request, CancellationToken ct)
        {
            var vacatGroup = await _db.TbVacationRulesGroups
                  .Include(g => g.VacationRuleDetails)
                  .ToListAsync(ct);

          var grp = _mapper.Map<List<VacationRulesGroupDto>>(vacatGroup);

            return grp;

        }

        public async Task<VacationRulesGroupDto?> Handle(GetVacationRulesGroupByIdQuery request, CancellationToken ct)
        {

            var vacG = await _db.TbVacationRulesGroups
                .Include(g => g.VacationRuleDetails)
                .FirstOrDefaultAsync(g => g.GroupId == request.GroupId, ct);

            return _mapper.Map<VacationRulesGroupDto?>(vacG);

        }
    }
}