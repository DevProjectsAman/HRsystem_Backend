using HRsystem.Api.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace HRsystem.Api.Features.Employee
{
    
    public record GetAvailableManagersQuery(
    int CompanyId,
    int DepartmentId,
    int JobLevelId,
    bool SameDepartmentOnly
) : IRequest<List<ManagerDto>>;


    public record ManagerDto(
    int EmployeeId,
    string FullName,
    int JobLevelId,
    string JobLevelDesc,
    int DepartmentId,
    string DepartmentName
);


    public class GetAvailableManagersHandler
    : IRequestHandler<GetAvailableManagersQuery, List<ManagerDto>>
    {
        private readonly DBContextHRsystem _db;

        public GetAvailableManagersHandler(DBContextHRsystem db)
        {
            _db = db;
        }

        public async Task<List<ManagerDto>> Handle(GetAvailableManagersQuery request, CancellationToken ct)
        {
            // Base query: All employees that ARE managers
            var query = _db.TbEmployees
                .Include(e => e.JobLevel)
                .Include(e => e.Department)
                .Where(e => e.CompanyId == request.CompanyId);
               // .Where(e => e.ManagerId > 0 || e.IsTopmanager == 1);   // managers only


            // Filter by department
            if (request.SameDepartmentOnly)
            {
                query = query.Where(e => e.DepartmentId == request.DepartmentId);
            }

            // Filter by job level (must be higher)
            query = query.Where(e => e.JobLevelId < request.JobLevelId);

            var result = await query
                .OrderBy(e => e.JobLevelId)
                .ThenBy(e => e.EnglishFullName)
                .Select(e => new ManagerDto(
                    e.EmployeeId,
                    e.EnglishFullName,
                    e.JobLevelId ?? 0,
                    e.JobLevel.JobLevelDesc ?? "",
                    e.DepartmentId,
                    e.Department.DepartmentName.en
                ))
                .ToListAsync(ct);

            return result;
        }
    }

}
