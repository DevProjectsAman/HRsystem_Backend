using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using HRsystem.Api.Shared.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Lookups.GeneralLookups
{
    // ===================== Query =====================
    public record GetAllMaritalStatusesQuery() : IRequest<List<MaritalStatusDto>>;

    // ===================== DTO =====================
    public record MaritalStatusDto
    {
        public int MaritalStatusId { get; set; }
        public string NameEn { get; set; } = string.Empty;
        public string? NameAr { get; set; }
    
    }

    // ===================== Handler =====================
    public class GetAllHandler : IRequestHandler<GetAllMaritalStatusesQuery, List<MaritalStatusDto>>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser; // optional, for language

        public GetAllHandler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<List<MaritalStatusDto>> Handle(GetAllMaritalStatusesQuery request, CancellationToken ct)
        {
            var lang = _currentUser.UserLanguage ?? "en";

            var statuses = await _db.TbMaritalStatuses.ToListAsync(ct);

            var res =  statuses.Select(s => new MaritalStatusDto
            {
                MaritalStatusId = s.MaritalStatusId,
                NameEn = s.NameEn,
                NameAr = s.NameAr // optionally, you could translate based on lang
            }).ToList();

            return res;
        }
    }

    // ===================== Minimal API Endpoint =====================
    public static class MaritalStatusEndpoints
    {
        public static void MapMaritalStatusEndpoints(this WebApplication app)
        {
            app.MapGet("/api/Lookups/marital-statuses", [Authorize] async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllMaritalStatusesQuery());
                return Results.Ok(new ResponseResultDTO<object> { Success=true  , Data =result  } );
            })
            .WithName("GetAllMaritalStatuses")
            .WithTags("Lookups");
        }
    }
}
