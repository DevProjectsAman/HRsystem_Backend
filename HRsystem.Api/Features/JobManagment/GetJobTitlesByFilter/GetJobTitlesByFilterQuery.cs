using global::HRsystem.Api.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.JobManagment.GetJobTitlesByFilter
{
    public record GetJobTitlesByFilterQuery(int CompanyId, int DepartmentId, int JobLevelId) : IRequest<List<JobTitleDto>>;

    public class GetJobTitlesByFilterHandler : IRequestHandler<GetJobTitlesByFilterQuery, List<JobTitleDto>>
    {
        private readonly DBContextHRsystem _db;

        public GetJobTitlesByFilterHandler(DBContextHRsystem db) => _db = db;

        public async Task<List<JobTitleDto>> Handle(GetJobTitlesByFilterQuery request, CancellationToken cancellationToken)
        {
            var jobTitles = await _db.TbJobTitles
                .Where(j =>
                    j.CompanyId == request.CompanyId &&
                    j.DepartmentId == request.DepartmentId &&
                    j.JobLevelId == request.JobLevelId
                )
                .Select(j => new JobTitleDto(j.JobTitleId, j.TitleName))
                .ToListAsync(cancellationToken);

            return jobTitles;
        }
    }

    public record JobTitleDto(int JobTitleId, string TitleName);
}
