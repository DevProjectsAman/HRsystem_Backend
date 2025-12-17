using FluentValidation;

namespace HRsystem.Api.Features.EmployeeHandler.Create
{


    #region Validator

    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommandNew>
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
            RuleFor(x => x.EmployeeWorkLocations)
                .NotNull()
                .NotEmpty();

            RuleForEach(x => x.EmployeeWorkLocations)
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
            RuleFor(x => x.EmployeeVacationBalances)
                .NotNull()
                .NotEmpty();

            RuleForEach(x => x.EmployeeVacationBalances)
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
