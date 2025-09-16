using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using HRsystem.Api.Shared.DTO;

namespace HRsystem.Api.Features.Department.UpdateDepartment
{
    public record UpdateDepartmentCommand(
        int DepartmentId,
        string? DepartmentCode,
        LocalizedData DepartmentName,
        int? CompanyId,
        int? UpdatedBy
    ) : IRequest<TbDepartment>;

    public class Handler : IRequestHandler<UpdateDepartmentCommand, TbDepartment>
    {
        private readonly DBContextHRsystem _db;
        public Handler(DBContextHRsystem db) => _db = db;

        public async Task<TbDepartment> Handle(UpdateDepartmentCommand request, CancellationToken ct)
        {
            var entity = await _db.TbDepartments.FindAsync(request.DepartmentId);
            if (entity == null) return null;

            entity.DepartmentName = request.DepartmentName;
            entity.DepartmentCode = request.DepartmentCode;
            entity.CompanyId = request.CompanyId;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = request.UpdatedBy;

            await _db.SaveChangesAsync(ct);
            return entity;
        }
    }

    public class UpdateDepartmentValidator : AbstractValidator<UpdateDepartmentCommand>
    {
        public UpdateDepartmentValidator()
        {
            RuleFor(x => x.DepartmentId).GreaterThan(0);
            RuleFor(x => x.DepartmentName.en).NotEmpty();
            RuleFor(x => x.DepartmentName.ar).NotEmpty();
            RuleFor(x => x.DepartmentCode).MaximumLength(25);
            RuleFor(x => x.CompanyId).NotNull();
        }
    }
}
