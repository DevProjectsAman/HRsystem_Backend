using HRsystem.Api.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Organization.Project.DeleteProject
{
    public record DeleteProjectCommand(int ProjectId) : IRequest<bool>;

    public class Handler(DBContextHRsystem db) : IRequestHandler<DeleteProjectCommand, bool>
    {
        public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken ct)
        {
            var entity = await db.TbProjects.FirstOrDefaultAsync(x => x.ProjectId == request.ProjectId, ct);
            if (entity == null) return false;

            db.TbProjects.Remove(entity);
            await db.SaveChangesAsync(ct);
            return true;
        }
    }
}
