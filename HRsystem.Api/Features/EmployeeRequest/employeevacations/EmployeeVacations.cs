using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace HRsystem.Api.Features.employeevacations
{
    //public class EmployeeVacations
    //{
        public record GetEmployeeVacationsQuery() : IRequest<List<EmployeeVacationDto>>;

        public class EmployeeVacationDto
        {
            public int? VacationTypeId { get; set; }
            public string? VacationName { get; set; }

            public decimal? TotalBalance { get; set; }
            public decimal? UsedDays { get; set; }
            public decimal? RemainingBalance { get; set; }
        }

        public class GetEmployeeVacationsHandler
        : IRequestHandler<GetEmployeeVacationsQuery, List<EmployeeVacationDto>>
        {
            private readonly DBContextHRsystem _db;
            private readonly ICurrentUserService _currentUserService;
            public GetEmployeeVacationsHandler(DBContextHRsystem db, ICurrentUserService currentUserService)
            {
                _db = db;
                _currentUserService = currentUserService;
            }

            public async Task<List<EmployeeVacationDto>> Handle(GetEmployeeVacationsQuery request, CancellationToken ct)
            {
                var employeeId = _currentUserService.EmployeeID;
                var languagetype = _currentUserService.UserLanguage;

                var vacationBalances = await _db.TbEmployeeVacationBalances
                    .Include(b => b.VacationType)
                    .Where(b => b.EmployeeId == employeeId)
                    .Select(b => new EmployeeVacationDto
                    {
                        VacationTypeId = b.VacationTypeId,
                        VacationName = languagetype == "ar"
                            ? b.VacationType.VacationName.ar
                            : b.VacationType.VacationName.en,
                        TotalBalance = b.TotalDays,
                        UsedDays = b.UsedDays ?? 0,
                        RemainingBalance = b.RemainingDays ?? (b.TotalDays - (b.UsedDays ?? 0))
                    })
                    .ToListAsync(ct);

                //var balances = await _db.TbEmployeeVacationBalances
                //    .Where(b => b.EmployeeId == employeeId)
                //    .Include(b => b.EmployeeId) // عشان تجيب الاسم
                //    .Select(b => new EmployeeVacationDto
                //    {
                //        VacationTypeId = b.VacationTypeId,
                //        VacationName = b.VacationType,
                //        TotalBalance = b.TotalDays,
                //        UsedDays = b.UsedDays,
                //        RemainingBalance = b.RemainingDays
                //    })
                //    .ToListAsync(ct);

                return vacationBalances;

            }
        }
}
//}