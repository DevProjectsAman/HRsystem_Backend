using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Lookups.GeneralLookups
{
    // ===================== Query =====================
    public record GetAllNationalitiesQuery() : IRequest<List<NationalityDto>>;

    // ===================== DTO =====================
    public record NationalityDto
    {
        public int NationalityId { get; set; }
        public string NameEn { get; set; } = string.Empty;
        public string? NameAr { get; set; }
    }

    // ===================== Handler =====================
    public class GetAllNationalitiesHandler
        : IRequestHandler<GetAllNationalitiesQuery, List<NationalityDto>>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;

        public GetAllNationalitiesHandler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<List<NationalityDto>> Handle(GetAllNationalitiesQuery request, CancellationToken ct)
        {
            var lang = _currentUser.UserLanguage ?? "en";

            var data = await _db.TbNationalities.ToListAsync(ct);

            return data.Select(n => new NationalityDto
            {
                NationalityId = n.NationalityId,
                NameEn = n.NameEn,
                NameAr = n.NameAr
            }).ToList();
        }
    }

    // ===================== Endpoint =====================
    public static class GetAllNationalitiesEndpoint
    {
        public static void MapGetAllNationalitiesEndpoint(this WebApplication app)
        {
            app.MapGet("/api/lookups/nationalities", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllNationalitiesQuery());
                return Results.Ok(new ResponseResultDTO<object> { Success = true, Data = result });
            })
            .WithTags("Lookups")
            .WithName("GetAllNationalities");
        }
    }
}
