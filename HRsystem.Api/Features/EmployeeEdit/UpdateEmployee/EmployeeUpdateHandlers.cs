using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

using static HRsystem.Api.Enums.EnumsList;

namespace HRsystem.Api.Features.EmployeeEdit.UpdateEmployee
{
        
        #region Update Basic Data

        public record UpdateEmployeeBasicDataCommand(
            int EmployeeId,
            string EnglishFullName,
            string ArabicFullName,
            string NationalId,
            DateOnly Birthdate,
            string? PlaceOfBirth,
            EnumGenderType Gender,
            string? EmployeePhotoPath
        ) : IRequest<ResponseResultDTO>;

        public class UpdateEmployeeBasicDataValidator : AbstractValidator<UpdateEmployeeBasicDataCommand>
        {
            public UpdateEmployeeBasicDataValidator()
            {
                RuleFor(x => x.EmployeeId).GreaterThan(0);
                RuleFor(x => x.EnglishFullName).NotEmpty().MaximumLength(200);
                RuleFor(x => x.ArabicFullName).NotEmpty().MaximumLength(200);
                RuleFor(x => x.NationalId).NotEmpty().MaximumLength(25);
            }
        }

        public class UpdateEmployeeBasicDataHandler
            : IRequestHandler<UpdateEmployeeBasicDataCommand, ResponseResultDTO>
        {
            private readonly DBContextHRsystem _db;

            public UpdateEmployeeBasicDataHandler(DBContextHRsystem db)
            {
                _db = db;
            }

            public async Task<ResponseResultDTO> Handle(
                UpdateEmployeeBasicDataCommand request,
                CancellationToken ct)
            {
                var employee = await _db.TbEmployees
                    .FirstOrDefaultAsync(e => e.EmployeeId == request.EmployeeId, ct);

                if (employee == null)
                {
                    return new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Employee not found."
                    };
                }

                employee.EnglishFullName = request.EnglishFullName;
                employee.ArabicFullName = request.ArabicFullName;
                employee.NationalId = request.NationalId;
                employee.Birthdate = request.Birthdate;
                employee.PlaceOfBirth = request.PlaceOfBirth ?? string.Empty;
                employee.Gender = request.Gender;
                employee.EmployeePhotoPath = request.EmployeePhotoPath;
                employee.UpdatedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync(ct);

                return new ResponseResultDTO
                {
                    Success = true,
                    Message = "Employee basic data updated successfully."
                };
            }
        }

        #endregion

        #region Update Extra Data

        public record UpdateEmployeeExtraDataCommand(
            int EmployeeId,
            string? PassportNumber,
            int MaritalStatusId,
            int NationalityId,
            string? Email,
            string PrivateMobile,
            string? BuisnessMobile,
            string? Address,
            EnumReligionType Religion,
            string? BloodGroup,
            string? Note
        ) : IRequest<ResponseResultDTO>;

        public class UpdateEmployeeExtraDataValidator : AbstractValidator<UpdateEmployeeExtraDataCommand>
        {
            public UpdateEmployeeExtraDataValidator()
            {
                RuleFor(x => x.EmployeeId).GreaterThan(0);
                RuleFor(x => x.PrivateMobile).NotEmpty().MaximumLength(25);
                RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
            }
        }

        public class UpdateEmployeeExtraDataHandler
            : IRequestHandler<UpdateEmployeeExtraDataCommand, ResponseResultDTO>
        {
            private readonly DBContextHRsystem _db;

            public UpdateEmployeeExtraDataHandler(DBContextHRsystem db)
            {
                _db = db;
            }

            public async Task<ResponseResultDTO> Handle(
                UpdateEmployeeExtraDataCommand request,
                CancellationToken ct)
            {
                var employee = await _db.TbEmployees
                    .FirstOrDefaultAsync(e => e.EmployeeId == request.EmployeeId, ct);

                if (employee == null)
                {
                    return new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Employee not found."
                    };
                }

                employee.PassportNumber = request.PassportNumber;
                employee.MaritalStatusId = request.MaritalStatusId;
                employee.NationalityId = request.NationalityId;
                employee.Email = request.Email ?? string.Empty;
                employee.PrivateMobile = request.PrivateMobile;
                employee.BuisnessMobile = request.BuisnessMobile;
                employee.Address = request.Address;
                employee.Religion = request.Religion;
                employee.BloodGroup = request.BloodGroup;
                employee.Note = request.Note;
                employee.UpdatedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync(ct);

                return new ResponseResultDTO
                {
                    Success = true,
                    Message = "Employee extra data updated successfully."
                };
            }
        }

        #endregion

        #region Update Organization

        public record UpdateEmployeeOrganizationCommand(
            int EmployeeId,
            int CompanyId,
            int DepartmentId,
            int? JobLevelId,
            int JobTitleId,
            int ManagerId
        ) : IRequest<ResponseResultDTO>;

        public class UpdateEmployeeOrganizationValidator : AbstractValidator<UpdateEmployeeOrganizationCommand>
        {
            public UpdateEmployeeOrganizationValidator()
            {
                RuleFor(x => x.EmployeeId).GreaterThan(0);
                RuleFor(x => x.CompanyId).GreaterThan(0);
                RuleFor(x => x.DepartmentId).GreaterThan(0);
                RuleFor(x => x.JobTitleId).GreaterThan(0);
            }
        }

        public class UpdateEmployeeOrganizationHandler
            : IRequestHandler<UpdateEmployeeOrganizationCommand, ResponseResultDTO>
        {
            private readonly DBContextHRsystem _db;

            public UpdateEmployeeOrganizationHandler(DBContextHRsystem db)
            {
                _db = db;
            }

            public async Task<ResponseResultDTO> Handle(
                UpdateEmployeeOrganizationCommand request,
                CancellationToken ct)
            {
                var employee = await _db.TbEmployees
                    .FirstOrDefaultAsync(e => e.EmployeeId == request.EmployeeId, ct);

                if (employee == null)
                {
                    return new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Employee not found."
                    };
                }

                employee.CompanyId = request.CompanyId;
                employee.DepartmentId = request.DepartmentId;
                employee.JobLevelId = request.JobLevelId;
                employee.JobTitleId = request.JobTitleId;
                employee.ManagerId = request.ManagerId;
                employee.UpdatedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync(ct);

                return new ResponseResultDTO
                {
                    Success = true,
                    Message = "Employee organization data updated successfully."
                };
            }
        }

        #endregion

        #region Update Hiring Info

        public record UpdateEmployeeHiringCommand(
            int EmployeeId,
            int ContractTypeId,
            string? SerialMobile,
            string? EmployeeCodeFinance,
            string? EmployeeCodeHr,
            DateOnly HireDate,
            DateTime StartDate,
            DateTime? EndDate,
            string Status,
            bool IsActive
        ) : IRequest<ResponseResultDTO>;

        public class UpdateEmployeeHiringValidator : AbstractValidator<UpdateEmployeeHiringCommand>
        {
            public UpdateEmployeeHiringValidator()
            {
                RuleFor(x => x.EmployeeId).GreaterThan(0);
                RuleFor(x => x.Status).NotEmpty().MaximumLength(25);
            }
        }

        public class UpdateEmployeeHiringHandler
            : IRequestHandler<UpdateEmployeeHiringCommand, ResponseResultDTO>
        {
            private readonly DBContextHRsystem _db;

            public UpdateEmployeeHiringHandler(DBContextHRsystem db)
            {
                _db = db;
            }

            public async Task<ResponseResultDTO> Handle(
                UpdateEmployeeHiringCommand request,
                CancellationToken ct)
            {
                var employee = await _db.TbEmployees
                    .FirstOrDefaultAsync(e => e.EmployeeId == request.EmployeeId, ct);

                if (employee == null)
                {
                    return new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Employee not found."
                    };
                }

                employee.ContractTypeId = request.ContractTypeId;
                employee.SerialMobile = request.SerialMobile;
                employee.EmployeeCodeFinance = request.EmployeeCodeFinance;
                employee.EmployeeCodeHr = request.EmployeeCodeHr;
                employee.HireDate = request.HireDate;
                employee.StartDate = request.StartDate;
                employee.EndDate = request.EndDate;
                employee.Status = request.Status;
                employee.IsActive = request.IsActive;
                employee.UpdatedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync(ct);

                return new ResponseResultDTO
                {
                    Success = true,
                    Message = "Employee hiring information updated successfully."
                };
            }
        }

        #endregion

        #region Update Shift & Work Days

        public record UpdateEmployeeShiftWorkDaysCommand(
            int EmployeeId,
            int ShiftId,
            int WorkDaysId,
            int? RemoteWorkDaysId
        ) : IRequest<ResponseResultDTO>;

        public class UpdateEmployeeShiftWorkDaysValidator : AbstractValidator<UpdateEmployeeShiftWorkDaysCommand>
        {
            public UpdateEmployeeShiftWorkDaysValidator()
            {
                RuleFor(x => x.EmployeeId).GreaterThan(0);
                RuleFor(x => x.ShiftId).GreaterThan(0);
                RuleFor(x => x.WorkDaysId).GreaterThan(0);
            }
        }

        public class UpdateEmployeeShiftWorkDaysHandler
            : IRequestHandler<UpdateEmployeeShiftWorkDaysCommand, ResponseResultDTO>
        {
            private readonly DBContextHRsystem _db;

            public UpdateEmployeeShiftWorkDaysHandler(DBContextHRsystem db)
            {
                _db = db;
            }

            public async Task<ResponseResultDTO> Handle(
                UpdateEmployeeShiftWorkDaysCommand request,
                CancellationToken ct)
            {
                var employee = await _db.TbEmployees
                    .FirstOrDefaultAsync(e => e.EmployeeId == request.EmployeeId, ct);

                if (employee == null)
                {
                    return new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Employee not found."
                    };
                }

                employee.ShiftId = request.ShiftId;
                employee.WorkDaysId = request.WorkDaysId;
                employee.RemoteWorkDaysId = request.RemoteWorkDaysId;
                employee.UpdatedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync(ct);

                return new ResponseResultDTO
                {
                    Success = true,
                    Message = "Employee shift and work days updated successfully."
                };
            }
    }

        #endregion


 
        #region Update Work Locations

        public record UpdateEmployeeWorkLocationsCommandNew(
            int EmployeeId,
            List<WorkLocationItemCommand> WorkLocations
        ) : IRequest<ResponseResultDTO>;

        public record WorkLocationItemCommand(
            int CityId,
            int WorkLocationId,
            int CompanyId
        );

        public class UpdateEmployeeWorkLocationsValidator : AbstractValidator<UpdateEmployeeWorkLocationsCommandNew>
        {
            public UpdateEmployeeWorkLocationsValidator()
            {
                RuleFor(x => x.EmployeeId).GreaterThan(0);
                RuleFor(x => x.WorkLocations).NotNull();
            }
        }

        public class UpdateEmployeeWorkLocationsHandler
            : IRequestHandler<UpdateEmployeeWorkLocationsCommandNew, ResponseResultDTO>
        {
            private readonly DBContextHRsystem _db;

            public UpdateEmployeeWorkLocationsHandler(DBContextHRsystem db)
            {
                _db = db;
            }

            public async Task<ResponseResultDTO> Handle(
                UpdateEmployeeWorkLocationsCommandNew request,
                CancellationToken ct)
            {
                var employee = await _db.TbEmployees
                    .Include(e => e.TbEmployeeWorkLocations)
                    .FirstOrDefaultAsync(e => e.EmployeeId == request.EmployeeId, ct);

                if (employee == null)
                {
                    return new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Employee not found."
                    };
                }

                // Remove all existing work locations
                _db.TbEmployeeWorkLocations.RemoveRange(employee.TbEmployeeWorkLocations);

                // Add new work locations
                foreach (var location in request.WorkLocations)
                {
                    var workLocation = new TbEmployeeWorkLocation
                    {
                        EmployeeId = request.EmployeeId,
                        CityId = location.CityId,
                        WorkLocationId = location.WorkLocationId,
                        CompanyId = location.CompanyId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _db.TbEmployeeWorkLocations.Add(workLocation);
                }

                employee.UpdatedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync(ct);

                return new ResponseResultDTO
                {
                    Success = true,
                    Message = "Employee work locations updated successfully."
                };
            }
        }

        #endregion

        #region Update Projects

        public record UpdateEmployeeProjectsCommand(
            int EmployeeId,
            List<ProjectItemCommand> Projects
        ) : IRequest<ResponseResultDTO>;

        public record ProjectItemCommand(
            int ProjectId,
            int CompanyId
        );

        public class UpdateEmployeeProjectsValidator : AbstractValidator<UpdateEmployeeProjectsCommand>
        {
            public UpdateEmployeeProjectsValidator()
            {
                RuleFor(x => x.EmployeeId).GreaterThan(0);
                RuleFor(x => x.Projects).NotNull();
            }
        }

        public class UpdateEmployeeProjectsHandler
            : IRequestHandler<UpdateEmployeeProjectsCommand, ResponseResultDTO>
        {
            private readonly DBContextHRsystem _db;

            public UpdateEmployeeProjectsHandler(DBContextHRsystem db)
            {
                _db = db;
            }

            public async Task<ResponseResultDTO> Handle(
                UpdateEmployeeProjectsCommand request,
                CancellationToken ct)
            {
                var employee = await _db.TbEmployees
                    .Include(e => e.TbEmployeeProjects)
                    .FirstOrDefaultAsync(e => e.EmployeeId == request.EmployeeId, ct);

                if (employee == null)
                {
                    return new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Employee not found."
                    };
                }

                // Remove all existing projects
                _db.TbEmployeeProjects.RemoveRange(employee.TbEmployeeProjects);

                // Add new projects
                foreach (var project in request.Projects)
                {
                    var employeeProject = new TbEmployeeProject
                    {
                        EmployeeId = request.EmployeeId,
                        ProjectId = project.ProjectId,
                        CompanyId = project.CompanyId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _db.TbEmployeeProjects.Add(employeeProject);
                }

                employee.UpdatedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync(ct);

                return new ResponseResultDTO
                {
                    Success = true,
                    Message = "Employee projects updated successfully."
                };
            }
        }

        #endregion

        #region Update Vacation Balances

        public record UpdateEmployeeVacationBalancesCommand(
            int EmployeeId,
            List<VacationBalanceItemCommand> VacationBalances
        ) : IRequest<ResponseResultDTO>;

        public record VacationBalanceItemCommand(
            int? BalanceId,  // null for new records
            int VacationTypeId,
            int Year,
            decimal TotalDays,
            decimal? UsedDays,
            decimal? RemainingDays
        );

        public class UpdateEmployeeVacationBalancesValidator : AbstractValidator<UpdateEmployeeVacationBalancesCommand>
        {
            public UpdateEmployeeVacationBalancesValidator()
            {
                RuleFor(x => x.EmployeeId).GreaterThan(0);
                RuleFor(x => x.VacationBalances).NotNull();
                RuleForEach(x => x.VacationBalances).ChildRules(balance =>
                {
                    balance.RuleFor(b => b.VacationTypeId).GreaterThan(0);
                    balance.RuleFor(b => b.Year).GreaterThan(2000);
                    balance.RuleFor(b => b.TotalDays).GreaterThanOrEqualTo(0);
                });
            }
        }

        public class UpdateEmployeeVacationBalancesHandler
            : IRequestHandler<UpdateEmployeeVacationBalancesCommand, ResponseResultDTO>
        {
            private readonly DBContextHRsystem _db;

            public UpdateEmployeeVacationBalancesHandler(DBContextHRsystem db)
            {
                _db = db;
            }

            public async Task<ResponseResultDTO> Handle(
                UpdateEmployeeVacationBalancesCommand request,
                CancellationToken ct)
            {
                var employee = await _db.TbEmployees
                    .Include(e => e.TbEmployeeVacationBalances)
                    .FirstOrDefaultAsync(e => e.EmployeeId == request.EmployeeId, ct);

                if (employee == null)
                {
                    return new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Employee not found."
                    };
                }

                // Get existing balance IDs from request
                var requestBalanceIds = request.VacationBalances
                    .Where(vb => vb.BalanceId.HasValue)
                    .Select(vb => vb.BalanceId!.Value)
                    .ToList();

                // Remove balances that are not in the request
                var balancesToRemove = employee.TbEmployeeVacationBalances
                    .Where(vb => !requestBalanceIds.Contains(vb.BalanceId))
                    .ToList();

                _db.TbEmployeeVacationBalances.RemoveRange(balancesToRemove);

                // Update or add vacation balances
                foreach (var balance in request.VacationBalances)
                {
                    if (balance.BalanceId.HasValue && balance.BalanceId > 0)
                    {
                        // Update existing balance
                        var existingBalance = employee.TbEmployeeVacationBalances
                            .FirstOrDefault(vb => vb.BalanceId == balance.BalanceId.Value);

                        if (existingBalance != null)
                        {
                            existingBalance.VacationTypeId = balance.VacationTypeId;
                            existingBalance.Year = balance.Year;
                            existingBalance.TotalDays = balance.TotalDays;
                            existingBalance.UsedDays = balance.UsedDays;
                            existingBalance.RemainingDays = balance.RemainingDays;
                        }
                    }
                    else
                    {
                        // Add new balance
                        var newBalance = new TbEmployeeVacationBalance
                        {
                            EmployeeId = request.EmployeeId,
                            VacationTypeId = balance.VacationTypeId,
                            Year = balance.Year,
                            TotalDays = balance.TotalDays,
                            UsedDays = balance.UsedDays,
                            RemainingDays = balance.RemainingDays
                        };

                        _db.TbEmployeeVacationBalances.Add(newBalance);
                    }
                }

                employee.UpdatedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync(ct);

                return new ResponseResultDTO
                {
                    Success = true,
                    Message = "Employee vacation balances updated successfully."
                };
            }
        }

        #endregion

        #region Optional: Update All Employee Data (Bulk Update)

        public record UpdateEmployeeFullCommand(
            int EmployeeId,
            UpdateEmployeeBasicDataCommand BasicData,
            UpdateEmployeeExtraDataCommand ExtraData,
            UpdateEmployeeOrganizationCommand Organization,
            UpdateEmployeeHiringCommand Hiring,
            UpdateEmployeeWorkLocationsCommandNew WorkLocations,
            UpdateEmployeeProjectsCommand Projects,
            UpdateEmployeeShiftWorkDaysCommand ShiftWorkDays,
            UpdateEmployeeVacationBalancesCommand VacationBalances
        ) : IRequest<ResponseResultDTO>;

        public class UpdateEmployeeFullHandler
            : IRequestHandler<UpdateEmployeeFullCommand, ResponseResultDTO>
        {
            private readonly IMediator _mediator;

            public UpdateEmployeeFullHandler(IMediator mediator)
            {
                _mediator = mediator;
            }

            public async Task<ResponseResultDTO> Handle(
                UpdateEmployeeFullCommand request,
                CancellationToken ct)
            {
                // Execute all updates sequentially
                var results = new List<ResponseResultDTO>();

                results.Add(await _mediator.Send(request.BasicData, ct));
                results.Add(await _mediator.Send(request.ExtraData, ct));
                results.Add(await _mediator.Send(request.Organization, ct));
                results.Add(await _mediator.Send(request.Hiring, ct));
                results.Add(await _mediator.Send(request.WorkLocations, ct));
                results.Add(await _mediator.Send(request.Projects, ct));
                results.Add(await _mediator.Send(request.ShiftWorkDays, ct));
                results.Add(await _mediator.Send(request.VacationBalances, ct));

                // Check if any update failed
                var failedUpdate = results.FirstOrDefault(r => !r.Success);
                if (failedUpdate != null)
                {
                    return new ResponseResultDTO
                    {
                        Success = false,
                        Message = $"Update failed: {failedUpdate.Message}"
                    };
                }

                return new ResponseResultDTO
                {
                    Success = true,
                    Message = "All employee data updated successfully."
                };
            }
        }

        #endregion
  


}



