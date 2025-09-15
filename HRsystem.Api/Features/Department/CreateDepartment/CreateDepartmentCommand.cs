using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HRsystem.Api.Features.Department.CreateDepartment
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
            _db = db ;
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
        public CreateDepartmentValidator()
        {
            RuleFor(x => x.DepartmentName.En)
            .NotEmpty().WithMessage("English department name is required")
            .MaximumLength(55).WithMessage("English department name cannot exceed 55 characters");

            RuleFor(x => x.DepartmentName.Ar)
                .NotEmpty().WithMessage("Arabic department name is required")
                .MaximumLength(55).WithMessage("Arabic department name cannot exceed 55 characters");

            RuleFor(x => x.DepartmentCode)
                .MaximumLength(25);

            RuleFor(x => x.CompanyId)
                .NotNull().WithMessage("CompanyId is required");

            //RuleFor(x => x.CompanyLogo)
            //    .MaximumLength(255);
        }
    }
}
