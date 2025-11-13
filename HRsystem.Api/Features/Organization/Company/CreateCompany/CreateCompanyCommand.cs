using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Migrations;
using MediatR;
using System;

namespace HRsystem.Api.Features.Organization.Company.CreateCompany
{
    public record CreateCompanyCommand(int GroupId, string CompanyName, string CompanyLogo) : IRequest<CreateCompanyResponse>;

    public record CreateCompanyResponse(int CompanyId, string CompanyName, int GroupId, string CompanyLogo);


    public class CreateCompanyHandler : IRequestHandler<CreateCompanyCommand, CreateCompanyResponse>
    {
        private readonly DBContextHRsystem _db;

        public CreateCompanyHandler(DBContextHRsystem db) => _db = db;

        public async Task<CreateCompanyResponse> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = new TbCompany
            {
                GroupId = request.GroupId,
                CompanyName = request.CompanyName,
                CompanyLogo = request.CompanyLogo,
                CreatedAt = DateTime.Now
            };

            _db.TbCompanies.Add(company);
            await _db.SaveChangesAsync(cancellationToken);

            return new CreateCompanyResponse(company.CompanyId, company.CompanyName, company.GroupId, company.CompanyLogo);
        }
    }

    public class CreateCompanyValidator : AbstractValidator<CreateCompanyCommand>
    {
        public CreateCompanyValidator()
        {
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
