 
using HRsystem.Api.Database;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.EmployeeEdit.GetEmployeeData
{
   
 
        public record GetEmployeeFullDetailsQuery(int EmployeeId)
            : IRequest<ResponseResultDTO<EmployeeFullDetailsDto>>;

        public class GetEmployeeFullDetailsHandler
            : IRequestHandler<GetEmployeeFullDetailsQuery, ResponseResultDTO<EmployeeFullDetailsDto>>
        {
            private readonly DBContextHRsystem _db;

            public GetEmployeeFullDetailsHandler(DBContextHRsystem db)
            {
                _db = db;
            }

            public async Task<ResponseResultDTO<EmployeeFullDetailsDto>> Handle(
                GetEmployeeFullDetailsQuery request,
                CancellationToken ct)
            {

            var emp = await _db.TbEmployees
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.EmployeeId == request.EmployeeId, ct);

            var employee = await _db.TbEmployees
                    .AsNoTracking()
                    .Include(e => e.Company)
                    .Include(e => e.Department)
                    .Include(e => e.JobLevel)
                    .Include(e => e.JobTitle)
                   // .Include(e => e.Manager)
                    .Include(e => e.MaritalStatus)
                    .Include(e => e.Nationality)
                    .Include(e => e.Shifts)
                    .Include(e => e.TbWorkDays)
                    .Include(e => e.TbRemoteWorkDays)
                    .Include(e => e.TbEmployeeWorkLocations)
                        .ThenInclude(wl => wl.WorkLocation)
                    .Include(e => e.TbEmployeeProjects)
                        .ThenInclude(p => p.Project)
                    .Include(e => e.TbEmployeeVacationBalances)
                        .ThenInclude(vb => vb.VacationType)
                    .FirstOrDefaultAsync(e => e.EmployeeId == request.EmployeeId, ct);

                if (employee == null)
                {
                    return new ResponseResultDTO<EmployeeFullDetailsDto>
                    {
                        Success = false,
                        Message = "Employee not found.",
                        Data = null
                    };
                }

                var result = new EmployeeFullDetailsDto
                {
                    EmployeeId = employee.EmployeeId,

                    BasicData = new EmployeeBasicDataDto
                    {
                        EnglishFullName = employee.EnglishFullName,
                        ArabicFullName = employee.ArabicFullName,
                        NationalId = employee.NationalId,
                        Birthdate = employee.Birthdate,
                        PlaceOfBirth = employee.PlaceOfBirth,
                        Gender = employee.Gender,
                        EmployeePhotoPath = employee.EmployeePhotoPath,
                        UniqueEmployeeCode = employee.UniqueEmployeeCode ?? string.Empty
                    },

                    ExtraData = new EmployeeExtraDataDto
                    {
                        PassportNumber = employee.PassportNumber,
                        MaritalStatusId = employee.MaritalStatusId,
                        MaritalStatusName = employee.MaritalStatus?.NameAr,
                        NationalityId = employee.NationalityId,
                        NationalityName = employee.Nationality?.NameAr,
                        Email = employee.Email,
                        PrivateMobile = employee.PrivateMobile,
                        BuisnessMobile = employee.BuisnessMobile,
                        Address = employee.Address,
                        Religion = employee.Religion,
                        BloodGroup = employee.BloodGroup,
                        Note = employee.Note
                    },

                    Organization = new EmployeeOrganizationDto
                    {
                        CompanyId = employee.CompanyId,
                        CompanyName = employee.Company?.CompanyName,
                        DepartmentId = employee.DepartmentId,
                        DepartmentName = employee.Department?.DepartmentName?.ar,
                        JobLevelId = employee.JobLevelId,
                        JobLevelName = employee.JobLevel?.JobLevelDesc,
                        JobTitleId = employee.JobTitleId,
                        JobTitleName = employee.JobTitle?.TitleName?.ar,
                        ManagerId = employee.ManagerId,
                        ManagerName = employee.Manager?.EnglishFullName
                    },

                    Hiring = new EmployeeHiringDto
                    {
                        ContractTypeId = employee.ContractTypeId,
                        SerialMobile = employee.SerialMobile,
                        EmployeeCodeFinance = employee.EmployeeCodeFinance,
                        EmployeeCodeHr = employee.EmployeeCodeHr,
                        HireDate = employee.HireDate,
                        StartDate = employee.StartDate,
                        EndDate = employee.EndDate,
                        Status = employee.Status,
                        IsActive = employee.IsActive
                    },

                    WorkLocations = employee.TbEmployeeWorkLocations
                        .Select(wl => new EmployeeWorkLocationDto
                        {
                            EmployeeWorkLocationId = wl.EmployeeWorkLocationId,
                            CityId = wl.CityId,
                            WorkLocationId = wl.WorkLocationId,
                            WorkLocationName = wl.WorkLocation?.LocationName?.ar,
                            CompanyId = wl.CompanyId
                        })
                        .ToList(),

                    Projects = employee.TbEmployeeProjects
                        .Select(p => new EmployeeProjectDto
                        {
                            EmployeeProjectId = p.EmployeeProjectId,
                            ProjectId = p.ProjectId,
                            ProjectName = p.Project?.ProjectName?.ar,
                            CompanyId = p.CompanyId
                        })
                        .ToList(),

                    ShiftWorkDays = new EmployeeShiftWorkDaysDto
                    {
                        ShiftId = employee.ShiftId,
                        ShiftName = employee.Shifts?.ShiftName?.ar,
                        WorkDaysId = employee.WorkDaysId,
                        WorkDaysName = employee.TbWorkDays?.WorkDaysNames.ToString(),
                        RemoteWorkDaysId = employee.RemoteWorkDaysId,
                        RemoteWorkDaysName = employee.TbRemoteWorkDays?.RemoteWorkDaysNames.ToString()??string.Empty
                    },

                    VacationBalances = employee.TbEmployeeVacationBalances
                        .Select(vb => new EmployeeVacationBalanceDto
                        {
                            BalanceId = vb.BalanceId,
                            VacationTypeId = vb.VacationTypeId,
                            VacationTypeName = vb.VacationType?.VacationName.ar,
                            Year = vb.Year,
                            TotalDays = vb.TotalDays,
                            UsedDays = vb.UsedDays,
                            RemainingDays = vb.RemainingDays
                        })
                        .ToList()
                };

                return new ResponseResultDTO<EmployeeFullDetailsDto>
                {
                    Success = true,
                    Message = "Employee details retrieved successfully.",
                    Data = result
                };
            }
        }
    }
