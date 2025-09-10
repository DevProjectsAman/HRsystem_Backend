using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HRsystem.Api.Features.Department.DeleteDepartment
{
    public record DeleteDepartmentCommand(int DepartmentId) : IRequest<bool>;

    public class Handler : IRequestHandler<DeleteDepartmentCommand, bool>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<bool> Handle(DeleteDepartmentCommand request, CancellationToken ct)
        {
            var entity = await _db.TbDepartments.FindAsync(request.DepartmentId);
            if (entity == null) return false;

            _db.TbDepartments.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
