using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Organization.Project.UpdateProject
{
    public record UpdateProjectCommand(
        int ProjectId,
        string? ProjectCode,
        LocalizedData ProjectName,
        //int? CityId,
        //int? WorkLocationId,
        int CompanyId,
        int? UpdatedBy
    ) : IRequest<UpdateProjectResponse?>;

    public record UpdateProjectResponse(
        int ProjectId,
        string ProjectCode,
        LocalizedData ProjectName,
        //int? CityId,
        //int? WorkLocationId,
        int CompanyId
    );

    public class Handler(DBContextHRsystem db) : IRequestHandler<UpdateProjectCommand, UpdateProjectResponse?>
    {
        public async Task<UpdateProjectResponse?> Handle(UpdateProjectCommand request, CancellationToken ct)
        {
            var entity = await db.TbProjects.FirstOrDefaultAsync(x => x.ProjectId == request.ProjectId, ct);
            if (entity is null)
            {
                throw new KeyNotFoundException($"Project with ID {request.ProjectId} not found.");
            }


                entity.ProjectCode = request.ProjectCode;
            entity.ProjectName = request.ProjectName;
            //entity.CityId = request.CityId;
            //entity.WorkLocationId = request.WorkLocationId;
            entity.CompanyId = request.CompanyId;
            entity.UpdatedBy = request.UpdatedBy;
            entity.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync(ct);

            return new UpdateProjectResponse(entity.ProjectId, entity.ProjectCode, entity.ProjectName, entity.CompanyId);
        }
    }

    public class UpdateProjectValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectValidator()
        {
            RuleFor(x => x.ProjectName.en).NotEmpty().MaximumLength(100);
            RuleFor(x => x.ProjectName.ar).NotEmpty().MaximumLength(100);
            RuleFor(x => x.ProjectCode).MaximumLength(25);
        }
    }
}
