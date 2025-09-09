using HRsystem.Api.Database;
using HRsystem.Api.Features.Groups.UpdateGroup;
using HRsystem.Api.Services.CurrentUser;
using MediatR;

namespace HRsystem.Api.Features.Company.UpdateCompany
{
    public record UpdateCompanyCommand(int CompanyId, string CompanyName, int GroupId) : IRequest<UpdateCompanyResponse>;

    public class UpdateCompanyResponse
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

        public int GroupId { get; set; }
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

            company.CompanyName = request.CompanyName;
            company.UpdatedBy = _currentUser.UserId;

            await _db.SaveChangesAsync(cancellationToken);

            return new UpdateCompanyResponse
            {
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName,
                GroupId = company.GroupId
            };
        }

    }
}
