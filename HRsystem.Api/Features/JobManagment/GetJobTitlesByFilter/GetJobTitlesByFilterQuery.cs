using HRsystem.Api.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.JobTitles.GetFilteredJobTitles
{
    public record GetFilteredJobTitlesQuery(int CompanyId, int DepartmentId, int JobLevelId) : IRequest<List<JobTitleDto>>;

    public class GetFilteredJobTitlesHandler : IRequestHandler<GetFilteredJobTitlesQuery, List<JobTitleDto>>
    {
        private readonly DBContextHRsystem _db;

        public GetFilteredJobTitlesHandler(DBContextHRsystem db) => _db = db;

        public async Task<List<JobTitleDto>> Handle(GetFilteredJobTitlesQuery request, CancellationToken cancellationToken)
        {
            return await _db.TbJobTitles
                .Where(j => j.CompanyId == request.CompanyId &&
                            j.DepartmentId == request.DepartmentId &&
                            j.JobLevelId == request.JobLevelId)
                .Select(j => new JobTitleDto(j.JobTitleId, j.TitleName))
                .ToListAsync(cancellationToken);
        }
    }

    public record JobTitleDto(int JobTitleId, string TitleName);
}
