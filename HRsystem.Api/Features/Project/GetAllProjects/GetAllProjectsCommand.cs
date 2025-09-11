using HRsystem.Api.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Project.GetAllProjects
{
    public record GetAllProjectsCommand() : IRequest<List<ProjectResponse>>;

    public record ProjectResponse(
        int ProjectId,
        string ProjectCode,
        string ProjectName,
        int? CityId,
        int? WorkLocationId,
        int CompanyId
    );

    public class Handler(DBContextHRsystem db) : IRequestHandler<GetAllProjectsCommand, List<ProjectResponse>>
    {
        public async Task<List<ProjectResponse>> Handle(GetAllProjectsCommand request, CancellationToken ct)
        {
            return await db.TbProjects
                .Select(p => new ProjectResponse(p.ProjectId, p.ProjectCode, p.ProjectName, p.CityId, p.WorkLocationId, p.CompanyId))
                .ToListAsync(ct);
        }
    }
}
