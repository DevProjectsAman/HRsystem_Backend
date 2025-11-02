using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.Organization.Department.GetAllDepartments;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using HRsystem.Api.Shared.Tools;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HRsystem.Api.Features.Organization.Department.GetAllDeparmentsLocalized
{
    public record GetAllDepartmentsLocalized(int companyID) : IRequest<List<DepartmentLocalizedDto>>;

    public class DepartmentLocalizedDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public LocalizedData DepartmentName { get; set; }
        public int? CompanyId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class Handler : IRequestHandler<GetAllDepartmentsLocalized, List<DepartmentLocalizedDto>>
    {
        private readonly DBContextHRsystem _db;

        private readonly ICurrentUserService _currentUser;
        public Handler(DBContextHRsystem db, ICurrentUserService currentUser)
        {

            _db = db;
            _currentUser = currentUser;
        }

        public async Task<List<DepartmentLocalizedDto>> Handle(GetAllDepartmentsLocalized request, CancellationToken ct)
        {
            var statues = await _db.TbDepartments.Where(c => c.CompanyId == request.companyID).ToListAsync(ct);

            var lang = _currentUser.UserLanguage ?? "en";

            return statues.Select(p => new DepartmentLocalizedDto
            {
                DepartmentId = p.DepartmentId,
                CompanyId = p.CompanyId,
                DepartmentCode = p.DepartmentCode ?? string.Empty, // Fixed the CS0019 error by replacing '0' with 'string.Empty'  
                DepartmentName = p.DepartmentName,
                CreatedBy = p.CreatedBy,
                CreatedAt = p.CreatedAt,
                UpdatedBy = p.UpdatedBy,
                UpdatedAt = p.UpdatedAt
            }).ToList();
        }
    }
}
