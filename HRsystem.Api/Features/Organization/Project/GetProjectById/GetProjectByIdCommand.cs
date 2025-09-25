using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.Tools;
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

    public class Handler(DBContextHRsystem db,ICurrentUserService currentUser) : IRequestHandler<GetProjectByIdCommand, ProjectResponse?>
    {
        public async Task<ProjectResponse?> Handle(GetProjectByIdCommand request, CancellationToken ct)
        {
            var entity = await db.TbProjects.FirstOrDefaultAsync(x => x.ProjectId == request.ProjectId, ct);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Project with ID {request.ProjectId} not found.");
            }

                return new ProjectResponse(entity.ProjectId, entity.ProjectCode, entity.ProjectName.GetTranslation(currentUser.UserLanguage), entity.CityId, entity.WorkLocationId, entity.CompanyId);
        }
    }
}
