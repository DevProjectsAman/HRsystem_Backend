using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.ActivityType.GetAllActivityTypes;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using HRsystem.Api.Shared.Tools;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.VacationType.GetAllVacationTypes
{
    public record GetAllVacationTypesQuery() : IRequest<List<VacationTypeDto>>;

    public class VacationTypeDto
    {
       
        public int VacationTypeId { get; set; }   
        
        public LocalizedData VacationTypeName { get; set; }

        public string Description { get; set; }

        public bool? IsPaid { get; set; }

        public bool? RequiresHrApproval { get; set; }


    }

    public class Handler : IRequestHandler<GetAllVacationTypesQuery, List<VacationTypeDto>>
    {
        private readonly DBContextHRsystem _db;

        private readonly ICurrentUserService _currentuser;
        public Handler(DBContextHRsystem db , ICurrentUserService currentuser)
        {
            _db = db;
            _currentuser = currentuser;
        }

        public async Task<List<VacationTypeDto>> Handle(GetAllVacationTypesQuery request, CancellationToken ct)
        {

            
            var lang = _currentuser.UserLanguage ?? "en";

            var statuses = await _db.TbVacationTypes.ToListAsync(ct);

            return statuses.Select(s => new VacationTypeDto
            {
                VacationTypeId = s.VacationTypeId,
                Description = s.Description,
                VacationTypeName = s.VacationName,// ✅ translated here
                IsPaid = s.IsPaid,
                RequiresHrApproval = s.RequiresHrApproval,
            }).ToList();
        }

    }
}
