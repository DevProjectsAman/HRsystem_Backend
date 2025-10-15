using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using HRsystem.Api.Shared.DTO;
using HRsystem.Api.Features.Organization.Department.CreateDepartment;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Organization.Department.UpdateDepartment
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
            if (entity == null)
            {
                throw new KeyNotFoundException($"Department with ID {request.DepartmentId} not found.");
            }

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
        private readonly DBContextHRsystem _db;

        public UpdateDepartmentValidator(DBContextHRsystem db)
        {
            _db = db;
            RuleFor(x => x.DepartmentId).GreaterThan(0);
            RuleFor(x => x.DepartmentName.en)
                     .NotEmpty().WithMessage("English department name is required")
                     .MaximumLength(55).WithMessage("English department name cannot exceed 55 characters")
                     .MustAsync(BeUniqueEnglishName).WithMessage("English department name must be unique");

            RuleFor(x => x.DepartmentName.ar)
                .NotEmpty().WithMessage("Arabic department name is required")
                .MaximumLength(55).WithMessage("Arabic department name cannot exceed 55 characters")
                .MustAsync(BeUniqueArabicName).WithMessage("Arabic department name must be unique");


            RuleFor(x => x.DepartmentCode)
           .NotEmpty().WithMessage("Department code is required")
           .MaximumLength(25).WithMessage("Department code cannot exceed 25 characters")
           .MustAsync(BeUniqueCode).WithMessage("Department code must be unique");

            RuleFor(x => x.CompanyId)
                .NotNull().WithMessage("CompanyId is required");

            RuleFor(x => x.CompanyId).NotNull();
        }

         
           

               
            


            private async Task<bool> BeUniqueCode(string code, CancellationToken ct)
            {
                if (string.IsNullOrWhiteSpace(code))
                    return true;

                return !await _db.TbDepartments
                    .AsNoTracking()
                    .AnyAsync(d => d.DepartmentCode.ToLower() == code.ToLower(), ct);
            }


            private async Task<bool> BeUniqueEnglishName(string name, CancellationToken ct)
            {
                return !_db.TbDepartments
                    .AsEnumerable() // switch to client evaluation
                    .Any(d => d.DepartmentName.en == name);
            }

            private async Task<bool> BeUniqueArabicName(string name, CancellationToken ct)
            {
                return !_db.TbDepartments
                    .AsEnumerable()
                    .Any(d => d.DepartmentName.ar == name);
            }

        
    }
}
