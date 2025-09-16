using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Shared.DTO;
using MediatR;

namespace HRsystem.Api.Features.Project.CreateProject
{
    public record CreateProjectCommand(
        string? ProjectCode,
        LocalizedData ProjectName,
        int? CityId,
        int? WorkLocationId,
        int CompanyId,
        int? CreatedBy
    ) : IRequest<CreateProjectResponse>;

    public record CreateProjectResponse(
        int ProjectId,
        string ProjectCode,
        LocalizedData ProjectName,
        int? CityId,
        int? WorkLocationId,
        int CompanyId
    );

    public class Handler(DBContextHRsystem db) : IRequestHandler<CreateProjectCommand, CreateProjectResponse>
    {
        public async Task<CreateProjectResponse> Handle(CreateProjectCommand request, CancellationToken ct)
        {
            var entity = new TbProject
            {
                ProjectCode = request.ProjectCode,
                ProjectName = request.ProjectName,
                CityId = request.CityId,
                WorkLocationId = request.WorkLocationId,
                CompanyId = request.CompanyId,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            db.TbProjects.Add(entity);
            await db.SaveChangesAsync(ct);

            return new CreateProjectResponse(entity.ProjectId, entity.ProjectCode, entity.ProjectName, entity.CityId, entity.WorkLocationId, entity.CompanyId);
        }
    }

    public class CreateProjectValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectValidator()
        {
            RuleFor(x => x.ProjectName.en).NotEmpty().MaximumLength(100);
            RuleFor(x => x.ProjectName.ar).NotEmpty().MaximumLength(100);
            RuleFor(x => x.ProjectCode).MaximumLength(25);
        }
    }
}
