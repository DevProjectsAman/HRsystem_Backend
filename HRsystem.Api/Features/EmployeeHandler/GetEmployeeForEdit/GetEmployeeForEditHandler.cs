using HRsystem.Api.Database;
using HRsystem.Api.Features.Employee.DTO;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.EmployeeHandler.GetEmployeeForEdit
{
    public record GetEmployeeForEditQuery(int EmployeeId)
     : IRequest<ResponseResultDTO< EmployeeEditDto>>;

    public sealed class GetEmployeeForEditQueryHandler
    : IRequestHandler<GetEmployeeForEditQuery, ResponseResultDTO<EmployeeEditDto>>
    {
        private readonly DBContextHRsystem _db;

        public GetEmployeeForEditQueryHandler(DBContextHRsystem db)
        {
            _db = db;
        }

        public async Task<ResponseResultDTO<EmployeeEditDto>> Handle(
            GetEmployeeForEditQuery request,
            CancellationToken cancellationToken)
        {
            var employee = await _db.TbEmployees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EmployeeId == request.EmployeeId,
                    cancellationToken);

            if (employee == null)
                throw new Exception("Employee not found");

            // =========================
            // Load Related Data
            // =========================

            //var workLocations = await _db.TbEmployeeWorkLocations
            //    .Where(x => x.EmployeeId == request.EmployeeId)
            //    .Include(x => x.WorkLocation)
            //        .ThenInclude(w => w.City)
            //            .ThenInclude(c => c.Gov)
            //    .AsNoTracking()
            //    .ToListAsync(cancellationToken);

            //var project = await _db.TbEmployeeProjects
            //    .Where(x => x.EmployeeId == request.EmployeeId)
            //    .OrderByDescending(x => x.CreatedAt)
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(cancellationToken);

            //var shift = await _db.TbEmployeeShifts
            //    .Where(x => x.EmployeeId == request.EmployeeId)
            //    .OrderByDescending(x => x.EffectiveDate)
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(cancellationToken);

            //var vacationBalances = await _db.TbEmployeeVacationBalances
            //    .Where(x => x.EmployeeId == request.EmployeeId)
            //    .AsNoTracking()
            //    .ToListAsync(cancellationToken);

            // =========================
            // Map To Edit DTO
            // =========================

            var dto = new EmployeeEditDto
            {
                EmployeeBasicDataEdit = new EmployeeBasicDataEditDto
                {
                    UniqueEmployeeCode = employee.UniqueEmployeeCode,
                    EnglishFullName = employee.EnglishFullName,
                    ArabicFullName = employee.ArabicFullName,
                    NationalId = employee.NationalId,
                    Birthdate = employee.Birthdate,
                    PlaceOfBirth = employee.PlaceOfBirth,
                    Gender = employee.Gender,
                    EmployeePhotoFileName = employee.EmployeePhotoPath
                },

                EmployeeExtraDataEdit = new EmployeeExtraDataEditDto
                {
                    PassportNumber = employee.PassportNumber,
                    MaritalStatusId = employee.MaritalStatusId,
                    NationalityId = employee.NationalityId,
                    Email = employee.Email,
                    PrivateMobile = employee.PrivateMobile,
                    BuisnessMobile = employee.BuisnessMobile,
                    Address = employee.Address,
                    Religion = employee.Religion,
                    Note = employee.Note
                },

                //EmployeeOrganization = new EmployeeOrganizationEditDto
                //{
                //    CompanyId = employee.CompanyId,
                //    DepartmentId = employee.DepartmentId,
                //    JobLevelId =(int) employee.JobLevelId,
                //    JobTitleId = employee.JobTitleId,
                //    ManagerId = employee.ManagerId ,
                //    ProjectId = project?.ProjectId
                //},

                //EmployeeOrganizationHiring = new EmployeeOrganizationHiringEditDto
                //{
                //    ContractTypeId = employee.ContractTypeId,
                //    SerialMobile = employee.SerialMobile,
                //    EmployeeCodeFinance = employee.EmployeeCodeFinance,
                //    EmployeeCodeHr = employee.EmployeeCodeHr,
                //    HireDate = employee.HireDate,
                //    StartDate = employee.StartDate,
                //    EndDate = employee.EndDate,
                //    Status = employee.Status
                //},

                //EmployeeShiftWorkDays = new EmployeeShiftWorkDaysEditDto
                //{
                //    ShiftId = shift?.ShiftId ?? 0,
                //    WorkDaysId = employee.WorkDaysId
                //},

                //EmployeeWorkLocations = new EmployeeWorkLocationsEditDto
                //{
                //    EmployeeWorkLocations = workLocations.Select(w => new WorkLocationEditDto
                //    {
                //        WorkLocationId = w.WorkLocationId,
                //        CompanyId = w.CompanyId,
                //        WorkLocationCode = w.WorkLocation.WorkLocationCode,
                //        Latitude =(double) w.WorkLocation.Latitude,
                //        Longitude =(double) w.WorkLocation.Longitude,
                //        AllowedRadiusM = w.WorkLocation.AllowedRadiusM,

                //        LocationName = new LocalizedData
                //        {
                //            ar = w.WorkLocation.LocationName.ar,
                //            en = w.WorkLocation.LocationName.en
                //        },

                //        CityId = w.WorkLocation.CityId,
                //        CityName = w.WorkLocation.City.CityName,
                //        GovId = w.WorkLocation.City.GovId,
                //        GovName = w.WorkLocation.City.Gov.GovName
                //    }).ToList()
                //},

                //EmployeeVacationsBalance = new EmployeeVacationsBalanceListEditDto
                //{
                //    EmployeeVacationBalances = vacationBalances
                //        .Select(v => new EmployeeVacationBalanceEditDto
                //        {
                //            VacationTypeId = v.VacationTypeId,
                //            Year = v.Year,
                //            TotalDays = v.TotalDays,
                //            Gender = employee.Gender,
                //            Religion = employee.Religion
                //        }).ToList()
                //}
            };

            return new ResponseResultDTO<EmployeeEditDto> { Success = true, Message = "Success", Data = dto }
            ;
        }
    }


}
