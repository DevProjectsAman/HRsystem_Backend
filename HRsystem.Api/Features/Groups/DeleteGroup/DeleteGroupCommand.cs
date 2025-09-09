using HRsystem.Api.Database;
using MediatR;
using System;
using Microsoft.EntityFrameworkCore;
    

namespace HRsystem.Api.Features.Groups.DeleteGroup
{
    public record DeleteGroupCommand (int groud_id) : IRequest<bool>;

    public class DeleteGroupHandler : IRequestHandler<DeleteGroupCommand, bool>
    {
        private readonly DBContextHRsystem _db;

        public DeleteGroupHandler(DBContextHRsystem db)
        {
            _db = db;
        }

        public async Task<bool> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _db.TbGroups
                .FirstOrDefaultAsync(g => g.GroupId == request.groud_id, cancellationToken);

            if (group == null)
                return false;

            _db.TbGroups.Remove(group);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }

}
