using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HRsystem.Api.Features.Department.GetAllDepartments
{
    public record GetAllDepartmentsQuery() : IRequest<List<TbDepartment>>;

    public class Handler : IRequestHandler<GetAllDepartmentsQuery, List<TbDepartment>>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<List<TbDepartment>> Handle(GetAllDepartmentsQuery request, CancellationToken ct)
        {
            return await _db.TbDepartments.ToListAsync(ct);
        }
    }
}
