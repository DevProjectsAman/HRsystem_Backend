using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HRsystem.Api.Features.EmployeeUpdates.UpdateEmployeeWorkLocation
{
    //public class SaveEmployeeWorkLocationsDto
    //{
    //    [Required]
    //    public int EmployeeId { get; set; }

    //    [Required]
    //    public List<int> WorkLocationIds { get; set; } = new();
    //}



    public record SaveEmployeeWorkLocationsCommand(int EmployeeId, List<int> WorkLocationIds) : IRequest<bool>;

    public class Handler    : IRequestHandler<SaveEmployeeWorkLocationsCommand, bool>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;

        public Handler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
            SaveEmployeeWorkLocationsCommand request,
            CancellationToken ct)
        {
            var companyId = _currentUser.CompanyID;

            // Existing selections
            var existing = await _db.TbEmployeeWorkLocations
                .Where(e =>
                    e.EmployeeId == request.EmployeeId &&
                    e.CompanyId == companyId)
                .ToListAsync(ct);

            var existingIds = existing
                .Select(e => e.WorkLocationId)
                .ToHashSet();

            var newIds = request.WorkLocationIds
                .Distinct()
                .ToHashSet();

            // ➕ Add
            var toAdd = newIds.Except(existingIds);
            foreach (var workLocationId in toAdd)
            {
                _db.TbEmployeeWorkLocations.Add(
                    new TbEmployeeWorkLocation
                    {
                        EmployeeId = request.EmployeeId,
                        WorkLocationId = workLocationId,
                        CompanyId = (int)companyId,
                        CreatedBy = _currentUser.UserId,
                        CreatedAt = DateTime.UtcNow
                    });
            }

            // ➖ Remove
            var toRemove = existing
                .Where(e => !newIds.Contains(e.WorkLocationId))
                .ToList();

            _db.TbEmployeeWorkLocations.RemoveRange(toRemove);

            await _db.SaveChangesAsync(ct);
            return true;
        }
    }

}
