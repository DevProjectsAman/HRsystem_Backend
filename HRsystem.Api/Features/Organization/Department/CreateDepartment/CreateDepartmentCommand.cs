using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HRsystem.Api.Features.Organization.Department.CreateDepartment
{
    public record CreateDepartmentCommand(
        string? DepartmentCode,
        LocalizedData DepartmentName,
        int? CompanyId

    ) : IRequest<TbDepartment>;

    public class Handler : IRequestHandler<CreateDepartmentCommand, TbDepartment>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;
        public Handler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;

        }

        public async Task<TbDepartment> Handle(CreateDepartmentCommand request, CancellationToken ct)
        {


            var entity = new TbDepartment
            {
                DepartmentCode = request.DepartmentCode,
                DepartmentName = request.DepartmentName,
                CompanyId = request.CompanyId,
                //CompanyLogo = request.CompanyLogo,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = _currentUser.UserId

            };

            _db.TbDepartments.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity;
        }



    }

    public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentCommand>
    {
        private readonly DBContextHRsystem _db;

        public CreateDepartmentValidator(DBContextHRsystem db)
        {
            _db = db;

            RuleFor(x => x.DepartmentName.en)
                .NotEmpty().WithMessage("English department name is required")
                .MaximumLength(55).WithMessage("English department name cannot exceed 55 characters")
                .MustAsync(BeUniqueEnglishName).WithMessage("English department name Already Exist");

            RuleFor(x => x.DepartmentName.ar)
                .NotEmpty().WithMessage("Arabic department name is required")
                .MaximumLength(55).WithMessage("Arabic department name cannot exceed 55 characters")
                .MustAsync(BeUniqueArabicName).WithMessage("Arabic department name Already Exist");


            RuleFor(x => x.DepartmentCode)
           .NotEmpty().WithMessage("Department code is required")
           .MaximumLength(25).WithMessage("Department code cannot exceed 25 characters")
           .MustAsync(BeUniqueCode).WithMessage("Department code Already Exist");

            RuleFor(x => x.CompanyId)
                .NotNull().WithMessage("CompanyId is required");

            //RuleFor(x => x.CompanyLogo)
            //    .MaximumLength(255);

            // ✅ Check combination of DepartmentCode + DepartmentName
            //RuleFor(x => x)
            //    .MustAsync(NotDuplicateCombination)
            //    .WithMessage("A department with the same code and name already exists.");
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

        //private async Task<bool> NotDuplicateCombination(CreateDepartmentCommand cmd, CancellationToken ct)
        //{
        //    return !_db.TbDepartments
        //        .AsEnumerable() // same trick since DepartmentName is complex type
        //        .Any(d =>
        //            d.DepartmentCode == cmd.DepartmentCode &&
        //            (
        //                d.DepartmentName.en == cmd.DepartmentName.en ||
        //                d.DepartmentName.ar == cmd.DepartmentName.ar
        //            ) &&
        //            d.CompanyId == cmd.CompanyId
        //        );
        //}
    }

}
