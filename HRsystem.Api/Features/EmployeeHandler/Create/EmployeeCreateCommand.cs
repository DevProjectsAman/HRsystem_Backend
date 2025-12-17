

using FluentValidation;
using MediatR;
using static global::HRsystem.Api.Enums.EnumsList;

namespace HRsystem.Api.Features.EmployeeHandler.Create
{


    #region Command

    public sealed record CreateEmployeeCommand(
        EmployeeBasicDataCommand EmployeeBasicData,
        EmployeeExtraDataCommand EmployeeExtraData,
        EmployeeOrganizationCommand EmployeeOrganization,
        EmployeeOrganizationHiringCommand EmployeeOrganizationHiring,
        EmployeeWorkLocationsCommand EmployeeWorkLocations,
        EmployeeShiftWorkDaysCommand EmployeeShiftWorkDays,
        EmployeeVacationsBalanceListCommand EmployeeVacationsBalance
    ) : IRequest<int>;


    #region Basic Data

    public sealed record EmployeeBasicDataCommand(
        string EnglishFullName,
        string ArabicFullName,
        string NationalId,
        DateOnly Birthdate,
        string? PlaceOfBirth,
        EnumGenderType Gender,
        string? EmployeePhotoPath
    );

    #endregion

    #region Extra Data

    public sealed record EmployeeExtraDataCommand(
        string? PassportNumber,
        int MaritalStatusId,
        int NationalityId,
        string? Email,
        string PrivateMobile,
        string? BuisnessMobile,
        string? Address,
        EnumReligionType Religion,
        string? Note
    );

    #endregion

    #region Organization

    public sealed record EmployeeOrganizationCommand(
        int CompanyId,
        int DepartmentId,
        int JobLevelId,
        int JobTitleId,
        int ManagerId,
        int? ProjectId
    );

    #endregion

    #region Hiring

    public sealed record EmployeeOrganizationHiringCommand(
        int ContractTypeId,
        string SerialMobile,
        string? EmployeeCodeFinance,
        string? EmployeeCodeHr,
        DateOnly HireDate,
        DateTime StartDate,
        DateTime? EndDate,
        string Status
    );

    #endregion

    #region Work Locations

    public sealed record EmployeeWorkLocationsCommand(
        IReadOnlyList<EmployeeWorkLocationCommand> Locations
    );

    public sealed record EmployeeWorkLocationCommand(
        int CityId,
        int WorkLocationId,
        int CompanyId
    );

    #endregion

    #region Shift

    public sealed record EmployeeShiftWorkDaysCommand(
        int ShiftId,
        int WorkDaysId
    );

    #endregion

    #region Vacation Balances

    public sealed record EmployeeVacationsBalanceListCommand(
        IReadOnlyList<EmployeeVacationBalanceCommand> Balances
    );

    public sealed record EmployeeVacationBalanceCommand(
        int VacationTypeId,
        int Year,
        decimal TotalDays,
        bool? Prorate,
        int? Priority,
        EnumGenderType Gender,
        EnumReligionType Religion
    );

    #endregion



    #endregion





    #region Validator

    public  class CreateEmployeeCommandValidator        : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator()
        {
            RuleFor(x => x.EmployeeBasicData)
                .NotNull()
                .SetValidator(new EmployeeBasicDataValidator());

            RuleFor(x => x.EmployeeExtraData)
                .NotNull()
                .SetValidator(new EmployeeExtraDataValidator());

            RuleFor(x => x.EmployeeOrganization)
                .NotNull()
                .SetValidator(new EmployeeOrganizationValidator());

            RuleFor(x => x.EmployeeOrganizationHiring)
                .NotNull()
                .SetValidator(new EmployeeOrganizationHiringValidator());

            RuleFor(x => x.EmployeeWorkLocations)
                .NotNull()
                .SetValidator(new EmployeeWorkLocationsValidator());

            RuleFor(x => x.EmployeeShiftWorkDays)
                .NotNull()
                .SetValidator(new EmployeeShiftWorkDaysValidator());

            RuleFor(x => x.EmployeeVacationsBalance)
                .NotNull()
                .SetValidator(new EmployeeVacationsBalanceListValidator());
        }
    }


    public sealed class EmployeeBasicDataValidator
     : AbstractValidator<EmployeeBasicDataCommand>
    {
        public EmployeeBasicDataValidator()
        {
            RuleFor(x => x.EnglishFullName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.ArabicFullName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.NationalId)
                .NotEmpty();

            RuleFor(x => x.Birthdate)
                .NotEmpty();

            RuleFor(x => x.Gender)
                .IsInEnum();
        }
    }

    public sealed class EmployeeExtraDataValidator
    : AbstractValidator<EmployeeExtraDataCommand>
    {
        public EmployeeExtraDataValidator()
        {
            RuleFor(x => x.MaritalStatusId)
                .GreaterThan(0);

            RuleFor(x => x.NationalityId)
                .GreaterThan(0);

            RuleFor(x => x.PrivateMobile)
                .NotEmpty();

            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.Religion)
                .IsInEnum();
        }
    }

    public sealed class EmployeeOrganizationValidator
    : AbstractValidator<EmployeeOrganizationCommand>
    {
        public EmployeeOrganizationValidator()
        {
            RuleFor(x => x.CompanyId).GreaterThan(0);
            RuleFor(x => x.DepartmentId).GreaterThan(0);
            RuleFor(x => x.JobLevelId).GreaterThan(0);
            RuleFor(x => x.JobTitleId).GreaterThan(0);
        }
    }

    public sealed class EmployeeOrganizationHiringValidator
    : AbstractValidator<EmployeeOrganizationHiringCommand>
    {
        public EmployeeOrganizationHiringValidator()
        {
            RuleFor(x => x.ContractTypeId)
                .GreaterThan(0);

            RuleFor(x => x.SerialMobile)
                .NotEmpty()
                .MaximumLength(25);

            RuleFor(x => x.HireDate)
                .NotEmpty();

            RuleFor(x => x.StartDate)
                .NotEmpty();

            RuleFor(x => x.Status)
                .NotEmpty()
                .MaximumLength(25);
        }
    }

    public sealed class EmployeeWorkLocationsValidator
    : AbstractValidator<EmployeeWorkLocationsCommand>
    {
        public EmployeeWorkLocationsValidator()
        {
            RuleFor(x => x.Locations)
                .NotNull()
                .NotEmpty();

            RuleForEach(x => x.Locations)
                .SetValidator(new EmployeeWorkLocationValidator());
        }
    }

    public sealed class EmployeeWorkLocationValidator
        : AbstractValidator<EmployeeWorkLocationCommand>
    {
        public EmployeeWorkLocationValidator()
        {
            RuleFor(x => x.CityId).GreaterThan(0);
            RuleFor(x => x.WorkLocationId).GreaterThan(0);
            RuleFor(x => x.CompanyId).GreaterThan(0);
        }
    }

    public sealed class EmployeeShiftWorkDaysValidator
    : AbstractValidator<EmployeeShiftWorkDaysCommand>
    {
        public EmployeeShiftWorkDaysValidator()
        {
            RuleFor(x => x.ShiftId)
                .GreaterThan(0);

            RuleFor(x => x.WorkDaysId)
                .GreaterThan(0);
        }
    }

    public sealed class EmployeeVacationsBalanceListValidator
    : AbstractValidator<EmployeeVacationsBalanceListCommand>
    {
        public EmployeeVacationsBalanceListValidator()
        {
            RuleFor(x => x.Balances)
                .NotNull()
                .NotEmpty();

            RuleForEach(x => x.Balances)
                .SetValidator(new EmployeeVacationBalanceValidator());
        }
    }

    public sealed class EmployeeVacationBalanceValidator
        : AbstractValidator<EmployeeVacationBalanceCommand>
    {
        public EmployeeVacationBalanceValidator()
        {
            RuleFor(x => x.VacationTypeId)
                .GreaterThan(0);

            RuleFor(x => x.Year)
                .GreaterThan(2000);

            RuleFor(x => x.TotalDays)
                .GreaterThan(0);

            RuleFor(x => x.Gender)
                .IsInEnum();

            RuleFor(x => x.Religion)
                .IsInEnum();
        }
    }




    #endregion




}
