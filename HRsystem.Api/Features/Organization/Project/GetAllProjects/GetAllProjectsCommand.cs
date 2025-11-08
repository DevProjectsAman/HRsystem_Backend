using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using HRsystem.Api.Shared.Tools;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace HRsystem.Api.Features.Organization.Project.GetAllProjects
{
    public record GetAllProjectsCommand() : IRequest<List<ProjectResponse>>;

    public class ProjectResponse
    {
       public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public LocalizedData ProjectName { get; set; }
       
        public int CompanyId { get; set; }
        }


    public class Handler : IRequestHandler<GetAllProjectsCommand, List<ProjectResponse>>
    {
        private readonly DBContextHRsystem _db;

        private readonly ICurrentUserService _currentUser;
        public  Handler(DBContextHRsystem db , ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
            _db = db;
        }
        public async Task<List<ProjectResponse>> Handle(GetAllProjectsCommand request, CancellationToken ct)
        {
            var statues = await _db.TbProjects.ToListAsync(ct);
            var lang = _currentUser.UserLanguage ?? "en";


            return statues.Select(p => new ProjectResponse
            {
                  ProjectId = p.ProjectId,
                  ProjectCode = p.ProjectCode,
                  ProjectName = p.ProjectName,
                  //CityId = p.CityId,
                  //WorkLocationId = p.WorkLocationId,
                  CompanyId = p.CompanyId
                }).ToList();

        }
    }
}
