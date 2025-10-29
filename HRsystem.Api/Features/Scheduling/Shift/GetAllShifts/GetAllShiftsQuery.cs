using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using HRsystem.Api.Shared.Tools;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Scheduling.Shift.GetAllShifts

{

    public record GetAllShiftsQuery(int companyId) : IRequest<List<ShiftDto>>;

    public class ShiftDto
    {
        public int ShiftId { get; set; }
        public LocalizedData ShiftName { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool IsFlexible { get; set; }
        public TimeOnly? MinStartTime { get; set; }
        public TimeOnly? MaxStartTime { get; set; }
        public int GracePeriodMinutes { get; set; }
        public decimal? RequiredWorkingHours { get; set; }
        public string Notes { get; set; }
        public int CompanyId { get; set; }
    }
    public class GetAllShiftsHandler : IRequestHandler<GetAllShiftsQuery, List<ShiftDto>>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;

        public GetAllShiftsHandler(DBContextHRsystem db, ICurrentUserService currentUser)
        {

            _db = db;
            _currentUser = currentUser;
        }

        public async Task<List<ShiftDto>> Handle(GetAllShiftsQuery request, CancellationToken ct)
        {
            var statues = await _db.TbShifts.Where(c=>c.CompanyId==request.companyId).ToListAsync(ct);

            var lang = _currentUser.UserLanguage ?? "en";

            return statues.Select(s => new ShiftDto
            {
                ShiftId = s.ShiftId,
               // ShiftName = s.ShiftName.GetTranslation(lang),
                ShiftName = s.ShiftName,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                GracePeriodMinutes = s.GracePeriodMinutes,
                IsFlexible = s.IsFlexible,
                MinStartTime = s.MinStartTime,
                MaxStartTime = s.MaxStartTime,
                RequiredWorkingHours = s.RequiredWorkingHours,
                Notes = s.Notes,
                CompanyId = s.CompanyId
            }).ToList();

        }
    }
}
