using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Features.Groups.UpdateGroup;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Organization.Company.UpdateCompany
{
    public record UpdateCompanyCommand(int CompanyId, string CompanyName, int GroupId, string CompanyLogo) : IRequest<UpdateCompanyResponse>;

    public class UpdateCompanyResponse
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

        public int GroupId { get; set; }

        public string CompanyLogo { get; set; }
    }

    /****/
    public class UpdateCompanyHandler : IRequestHandler<UpdateCompanyCommand, UpdateCompanyResponse>
    {
        private readonly DBContextHRsystem _db;

        private readonly ICurrentUserService _currentUser;

        public UpdateCompanyHandler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<UpdateCompanyResponse> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _db.TbCompanies.FindAsync(new object[] { request.CompanyId }, cancellationToken);

            if (company == null)
                throw new KeyNotFoundException($"Company with Id {request.CompanyId} not found");

            var groupExists = await _db.TbGroups.AnyAsync(g => g.GroupId == request.GroupId, cancellationToken);
            if (!groupExists)
                throw new KeyNotFoundException($"Group with Id {request.GroupId} not found");

            company.CompanyName = request.CompanyName;
            company.GroupId = request.GroupId;
            company.UpdatedBy = _currentUser.UserId; // optional
            company.CompanyLogo = request.CompanyLogo;
            company.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _db.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Database error: {ex.InnerException?.Message ?? ex.Message}", ex);
            }

            return new UpdateCompanyResponse
            {
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName,
                GroupId = company.GroupId,
                CompanyLogo = company.CompanyLogo,

            };
        }


    }

    public class UpdateCompanyValidator : AbstractValidator<UpdateCompanyCommand>
    {
        public UpdateCompanyValidator()
        {
            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("CompanyId must be greater than 0");

            RuleFor(x => x.GroupId)
                .GreaterThan(0).WithMessage("GroupId must be greater than 0");

            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Company name is required")
                .MaximumLength(100).WithMessage("Company name cannot exceed 100 characters");

            RuleFor(x => x.CompanyLogo)
                .NotEmpty().WithMessage("Company logo is required")
                .MaximumLength(250).WithMessage("Company logo URL cannot exceed 250 characters");
        }
    }
}
