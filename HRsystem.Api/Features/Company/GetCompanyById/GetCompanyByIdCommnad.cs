using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Features.Groups.GetGroupById;
using MediatR;

namespace HRsystem.Api.Features.Company.GetCompanyById
{
    public record GetCompanyByIdCommand(int CompanyId) : IRequest<GetCompanyResponse?>;

    public class GetCompanyResponse
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

        public int GroupId { get; set; }

        public string  CompanyLogo { get; set; }
    }

    /**********/
    public class GetCompanyHandler : IRequestHandler<GetCompanyByIdCommand, GetCompanyResponse>
    {
        private readonly DBContextHRsystem _db;

        public GetCompanyHandler(DBContextHRsystem db)
        {
            _db = db;
        }

        public async Task<GetCompanyResponse> Handle(GetCompanyByIdCommand request, CancellationToken cancellationToken)
        {
            var company = await _db.TbCompanies.FindAsync(new object[] { request.CompanyId }, cancellationToken);

            if (company == null)
                return null!;

            return new GetCompanyResponse
            {
               CompanyId  = company.CompanyId,
               CompanyName = company.CompanyName,
               GroupId = company.GroupId,
               CompanyLogo = company.CompanyLogo,
               

                
            };
        }
    }

    //public class GetCompanyCommandValidator : AbstractValidator<GetCompanyByIdCommand>
    //{
    //    public GetCompanyCommandValidator()
    //    {
    //        RuleFor(x => x.)
    //            .GreaterThan(0).WithMessage("Group id must be greater than 0");
    //    }
    //}



}
