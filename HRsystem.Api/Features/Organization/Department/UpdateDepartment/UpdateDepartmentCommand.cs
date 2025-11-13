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
using HRsystem.Api.Services.CurrentUser;
using System.Xml.Linq;

namespace HRsystem.Api.Features.Organization.Department.UpdateDepartment
{
    public record UpdateDepartmentCommand(
        int DepartmentId,
        string? DepartmentCode,
        LocalizedData DepartmentName,
        int? CompanyId
    ) : IRequest<TbDepartment>;

    public class Handler : IRequestHandler<UpdateDepartmentCommand, TbDepartment>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUserService;

        public Handler(DBContextHRsystem db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }
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
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = _currentUserService.UserId;

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

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0);

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
        }

        private async Task<bool> BeUniqueCode(UpdateDepartmentCommand command, string code, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(code))
                return true;

            return !await _db.TbDepartments
                .AsNoTracking()
                .AnyAsync(d => d.DepartmentCode.ToLower() == code.ToLower()
                            && d.DepartmentId != command.DepartmentId && d.CompanyId == command.CompanyId, ct);
        }

        private async Task<bool> BeUniqueEnglishName(UpdateDepartmentCommand command, string name, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(name))
                return true;

            var existingNames = await _db.TbDepartments
                .AsNoTracking()
                .Where(d => d.DepartmentId != command.DepartmentId  && d.CompanyId==  command.CompanyId)
                .Select(d => new { d.DepartmentId, EnglishName = d.DepartmentName.en })
                .ToListAsync(ct);

            return !existingNames.Any(d =>
                string.Equals(d.EnglishName, name, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<bool> BeUniqueArabicName(UpdateDepartmentCommand command, string name, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(name))
                return true;

            var existingNames = await _db.TbDepartments
                .AsNoTracking()
                .Where(d => d.DepartmentId != command.DepartmentId && d.CompanyId == command.CompanyId)
                .Select(d => new { d.DepartmentId, ArabicName = d.DepartmentName.ar })
                .ToListAsync(ct);

            return !existingNames.Any(d =>
                string.Equals(d.ArabicName, name, StringComparison.OrdinalIgnoreCase));
        }
    }

}

