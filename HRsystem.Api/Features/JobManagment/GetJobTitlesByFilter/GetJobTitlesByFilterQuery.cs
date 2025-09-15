using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.Tools;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.JobTitles.GetFilteredJobTitles
{
    public record GetFilteredJobTitlesQuery(int CompanyId, int DepartmentId, int JobLevelId) : IRequest<List<JobTitleDto>>;

    public class GetFilteredJobTitlesHandler : IRequestHandler<GetFilteredJobTitlesQuery, List<JobTitleDto>>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;
        public GetFilteredJobTitlesHandler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;

        }

        public async Task<List<JobTitleDto>> Handle(GetFilteredJobTitlesQuery request, CancellationToken cancellationToken)
        {
            var jobTitles = await _db.TbJobTitles
                .Where(j => j.CompanyId == request.CompanyId &&
                            j.DepartmentId == request.DepartmentId &&
                            j.JobLevelId == request.JobLevelId)
                .Select(j => new { j.JobTitleId, j.TitleName }) // fetch only needed fields
                .ToListAsync(cancellationToken);

            return jobTitles
                .Select(j => new JobTitleDto(
                    j.JobTitleId,
                    j.TitleName.GetTranslation(_currentUser.UserLanguage) // safe in-memory call
                ))
                .ToList();
        }

    }

    public record JobTitleDto(int JobTitleId, string TitleName);
}
