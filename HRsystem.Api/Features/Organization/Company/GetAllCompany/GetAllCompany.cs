using HRsystem.Api.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Organization.Company.GetAllCompany
{
    public record GetAllCompanyCommand() : IRequest<List<GetAllCompanyResponse>>;

    public class GetAllCompanyResponse

    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int GroupId { get; set; }

        public string CompanyLogo { get; set; }

    };


    public class GetAllCompanyHandler : IRequestHandler<GetAllCompanyCommand, List<GetAllCompanyResponse>>
    {
        private readonly DBContextHRsystem _db;

        public GetAllCompanyHandler(DBContextHRsystem db) => _db = db;

        public async Task<List<GetAllCompanyResponse>> Handle(GetAllCompanyCommand request, CancellationToken cancellationToken)
        {



            var company = await _db.TbCompanies
                .Select(g => new GetAllCompanyResponse
                {
                    CompanyId = g.CompanyId,
                    CompanyName = g.CompanyName,
                    GroupId = g.GroupId,
                    CompanyLogo = g.CompanyLogo,
                }).ToListAsync(cancellationToken);

            return company;
        }
    }


}
