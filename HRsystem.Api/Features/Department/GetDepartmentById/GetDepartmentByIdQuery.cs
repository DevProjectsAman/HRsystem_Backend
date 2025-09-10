using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HRsystem.Api.Features.Department.GetDepartmentById
{
    public record GetDepartmentByIdQuery(int DepartmentId) : IRequest<TbDepartment>;

    public class Handler : IRequestHandler<GetDepartmentByIdQuery, TbDepartment>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<TbDepartment> Handle(GetDepartmentByIdQuery request, CancellationToken ct)
        {
            return await _db.TbDepartments.FindAsync(request.DepartmentId);
        }
    }
}
