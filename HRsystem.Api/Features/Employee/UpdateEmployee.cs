// File: Features/Employee/Commands/UpdateEmployeeCommand.cs
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.Employee.DTO;
using HRsystem.Api.Services.CurrentUser;

namespace HRsystem.Api.Features.Employee
{
    // Command
    public record UpdateEmployeeCommand(EmployeeUpdateDto Employee) : IRequest<Unit>;

    // Handler
    public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, Unit>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUserService;

        public UpdateEmployeeHandler(DBContextHRsystem db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Employee ?? throw new ArgumentNullException(nameof(request.Employee));
            var currentEmployeeId = _currentUserService.EmployeeID;

            var employee = await _db.TbEmployees
                .Include(e => e.TbEmployeeWorkLocations)
                .Include(e => e.TbEmployeeVacationBalances)
                .FirstOrDefaultAsync(e => e.EmployeeId == dto.EmployeeId, cancellationToken);

            if (employee == null)
                throw new Exception($"Employee with id {dto.EmployeeId} not found.");

            using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                // Basic data (partial update: only apply non-null fields from DTO)
                if (dto.EmployeeBasicData != null)
                {
                    var b = dto.EmployeeBasicData;
                    if (b.EnglishFullName != null) employee.EnglishFullName = b.EnglishFullName;
                    if (b.ArabicFullName != null) employee.ArabicFullName = b.ArabicFullName;
                    if (b.NationalId != null) employee.NationalId = b.NationalId;
                    if (b.Birthdate.HasValue) employee.Birthdate = b.Birthdate.Value;
                    if (b.PlaceOfBirth != null) employee.PlaceOfBirth = b.PlaceOfBirth;
                    if (b.Gender.HasValue) employee.Gender = b.Gender.Value;
                    if (b.PassportNumber != null) employee.PassportNumber = b.PassportNumber;
                    if (b.MaritalStatusId.HasValue) employee.MaritalStatusId = b.MaritalStatusId.Value;
                    if (b.NationalityId.HasValue) employee.NationalityId = b.NationalityId.Value;
                    if (b.Email != null) employee.Email = b.Email;
                    if (b.PrivateMobile != null) employee.PrivateMobile = b.PrivateMobile;
                    if (b.BuisnessMobile != null) employee.BuisnessMobile = b.BuisnessMobile;
                    if (b.Address != null) employee.Address = b.Address;
                    if (b.EmployeePhotoPath != null) employee.EmployeePhotoPath = b.EmployeePhotoPath;
                    if (b.Note != null) employee.Note = b.Note;
                }

                // Organization
                if (dto.EmployeeOrganization != null)
                {
                    var o = dto.EmployeeOrganization;
                    if (o.CompanyId.HasValue) employee.CompanyId = o.CompanyId.Value;
                    if (o.DepartmentId.HasValue) employee.DepartmentId = o.DepartmentId.Value;
                    if (o.JobTitleId.HasValue) employee.JobTitleId = o.JobTitleId.Value;
                    if (o.ManagerId.HasValue) employee.ManagerId = o.ManagerId.Value;
                    if (o.ContractTypeId.HasValue) employee.ContractTypeId = o.ContractTypeId.Value;
                    if (o.SerialMobile != null) employee.SerialMobile = o.SerialMobile;
                    if (o.EmployeeCodeFinance != null) employee.EmployeeCodeFinance = o.EmployeeCodeFinance;
                    if (o.EmployeeCodeHr != null) employee.EmployeeCodeHr = o.EmployeeCodeHr;
                    if (o.HireDate.HasValue) employee.HireDate = o.HireDate.Value;
                    if (o.StartDate.HasValue) employee.StartDate = o.StartDate.Value;
                    if (o.EndDate.HasValue) employee.EndDate = o.EndDate;
                    if (o.Status != null) employee.Status = o.Status;
                }

                // Work conditions
                if (dto.EmployeeWorkConditions != null)
                {
                    var w = dto.EmployeeWorkConditions;
                    if (w.ShiftId.HasValue) employee.ShiftId = w.ShiftId.Value;
                    if (w.WorkDaysId.HasValue) employee.WorkDaysId = w.WorkDaysId.Value;

                    // Replace work locations only if provided
                    if (w.EmployeeWorkLocations != null)
                    {
                        _db.TbEmployeeWorkLocations.RemoveRange(employee.   TbEmployeeWorkLocations ?? Enumerable.Empty<TbEmployeeWorkLocation>());

                        var newLocations = w.EmployeeWorkLocations
                            .Select(loc => new TbEmployeeWorkLocation
                            {
                                EmployeeId = employee.EmployeeId,
                                CityId = loc.CityId ?? 0,
                                WorkLocationId = loc.WorkLocationId ?? 0,
                                CompanyId = loc.CompanyId ?? 0,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = (int)currentEmployeeId
                            }).ToList();

                        if (newLocations.Any())
                            _db.TbEmployeeWorkLocations.AddRange(newLocations);
                    }
                }

                // Vacation balances (replace only if provided)
                if (dto.EmployeeVacationBalances != null)
                {
                    _db.TbEmployeeVacationBalances.RemoveRange(employee.TbEmployeeVacationBalances ?? Enumerable.Empty<TbEmployeeVacationBalance>());

                    var newBalances = dto.EmployeeVacationBalances.Select(bal => new TbEmployeeVacationBalance
                    {
                        EmployeeId = employee.EmployeeId,
                        VacationTypeId = bal.VacationTypeId,
                        Year = bal.Year ?? DateTime.UtcNow.Year,
                        TotalDays = bal.TotalDays ?? 0,
                        UsedDays = bal.UsedDays ?? 0,
                        RemainingDays = bal.RemainingDays ?? 0
                    }).ToList();

                    if (newBalances.Any())
                        _db.TbEmployeeVacationBalances.AddRange(newBalances);
                }

                // Meta
                employee.UpdatedAt = DateTime.UtcNow;
                employee.UpdatedBy = (int)currentEmployeeId;

                await _db.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return Unit.Value;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}

