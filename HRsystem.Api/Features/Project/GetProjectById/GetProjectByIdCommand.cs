using HRsystem.Api.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Project.GetProjectById
{
    public record GetProjectByIdCommand(int ProjectId) : IRequest<ProjectResponse?>;

    public record ProjectResponse(
        int ProjectId,
        string ProjectCode,
        string ProjectName,
        int? CityId,
        int? WorkLocationId,
        int CompanyId
    );

    public class Handler(DBContextHRsystem db) : IRequestHandler<GetProjectByIdCommand, ProjectResponse?>
    {
        public async Task<ProjectResponse?> Handle(GetProjectByIdCommand request, CancellationToken ct)
        {
            var entity = await db.TbProjects.FirstOrDefaultAsync(x => x.ProjectId == request.ProjectId, ct);
            if (entity == null) return null;

            return new ProjectResponse(entity.ProjectId, entity.ProjectCode, entity.ProjectName, entity.CityId, entity.WorkLocationId, entity.CompanyId);
        }
    }
}
