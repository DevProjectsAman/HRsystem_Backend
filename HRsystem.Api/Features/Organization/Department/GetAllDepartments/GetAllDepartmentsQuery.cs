using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.Tools;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HRsystem.Api.Features.Organization.Department.GetAllDepartments
{
    public record GetAllDepartmentsQuery() : IRequest<List<DepartmentDto>>;

    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public int? CompanyId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class Handler : IRequestHandler<GetAllDepartmentsQuery, List<DepartmentDto>>
    {
        private readonly DBContextHRsystem _db;

        private readonly ICurrentUserService _currentUser;
        public Handler(DBContextHRsystem db, ICurrentUserService currentUser)
        {

            _db = db;
            _currentUser = currentUser;
        }

        public async Task<List<DepartmentDto>> Handle(GetAllDepartmentsQuery request, CancellationToken ct)
        {
            var statues = await _db.TbDepartments.ToListAsync(ct);

            var lang = _currentUser.UserLanguage ?? "en";

            return statues.Select(p => new DepartmentDto
            {
                DepartmentId = p.DepartmentId,
                CompanyId = p.CompanyId,
                DepartmentCode = p.DepartmentCode,
                DepartmentName = p.DepartmentName.GetTranslation(lang),
                CreatedBy = p.CreatedBy,
                CreatedAt = p.CreatedAt,
                UpdatedBy = p.UpdatedBy,
                UpdatedAt = p.UpdatedAt
            }).ToList();
        }
    }
}
