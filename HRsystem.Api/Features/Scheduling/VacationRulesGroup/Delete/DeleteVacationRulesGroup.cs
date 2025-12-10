using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Scheduling.VacationRulesGroup.Delete
{
    public record DeleteVacationRulesGroupCommand(int GroupId) : IRequest<bool>;

    public class DeleteVacationRulesGroupHandler : IRequestHandler<DeleteVacationRulesGroupCommand, bool>
    {
        private readonly DBContextHRsystem _db;
        public DeleteVacationRulesGroupHandler(DBContextHRsystem db) => _db = db;

        public async Task<bool> Handle(DeleteVacationRulesGroupCommand request, CancellationToken ct)
        {
            var entity = await _db.TbVacationRulesGroups
                .Include(g => g.VacationRuleDetails)
                .FirstOrDefaultAsync(g => g.GroupId == request.GroupId, ct);

            if (entity is null) return false;

            _db.TbVacationRulesGroupDetails.RemoveRange(entity.VacationRuleDetails);
            _db.TbVacationRulesGroups.Remove(entity);



            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
