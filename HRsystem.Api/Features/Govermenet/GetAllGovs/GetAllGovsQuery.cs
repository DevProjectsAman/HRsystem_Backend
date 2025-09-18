using HRsystem.Api.Database.DataTables;
using MediatR;
using HRsystem.Api.Database;
using Microsoft.EntityFrameworkCore;
using static HRsystem.Api.Features.Gov.GetAllGovs.Handler;

namespace HRsystem.Api.Features.Gov.GetAllGovs
{
    public record GetAllGovsQuery() : IRequest<List<GovDto>>;

    public class Handler : IRequestHandler<GetAllGovsQuery, List<GovDto>>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<List<GovDto>> Handle(GetAllGovsQuery request, CancellationToken ct)
        {
            return await _db.TbGovs
                .Select(g => new GovDto
                {
                    GovId = g.GovId,
                    GovName = g.GovName,
                    Cities = g.TbCities.Select(c => new CityDto
                    {
                        CityId = c.CityId,
                        CityName = c.CityName
                    }).ToList()
                })
                .ToListAsync(ct);
        }

        public class GovDto
        {
            public int GovId { get; set; }
            public string GovName { get; set; }
            public List<CityDto> Cities { get; set; } = new();
        }

        public class CityDto
        {
            public int CityId { get; set; }
            public string? CityName { get; set; }
        }

    }
}
