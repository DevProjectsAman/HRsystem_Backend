using HRsystem.Api.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.City.DeleteCity
{
    public record DeleteCityCommand(int CityId) : IRequest<bool>;

    public class Handler(DBContextHRsystem db) : IRequestHandler<DeleteCityCommand, bool>
    {
        public async Task<bool> Handle(DeleteCityCommand request, CancellationToken ct)
        {
            var entity = await db.TbCities.FirstOrDefaultAsync(x => x.CityId == request.CityId, ct);
            if (entity == null) return false;

            db.TbCities.Remove(entity);
            await db.SaveChangesAsync(ct);
            return true;
        }
    }
}
