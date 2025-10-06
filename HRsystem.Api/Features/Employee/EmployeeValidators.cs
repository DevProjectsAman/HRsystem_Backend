namespace HRsystem.Api.Features.Employee
{
    using FluentValidation;
    using HRsystem.Api.Features.Employee.DTO;

    public class EmployeeCreateValidator : AbstractValidator<EmployeeCreateDto>
    {
        public EmployeeCreateValidator()
        {
            RuleFor(x => x.EmployeeOrganization.EmployeeCodeHr)
                .NotEmpty().MaximumLength(55);

            RuleFor(x => x.EmployeeBasicData.EnglishFullName)
                .NotEmpty().MaximumLength(55);


            

            RuleFor(x => x.EmployeeBasicData.NationalId)
                .Matches(@"^\d{14}$").When(x => !string.IsNullOrEmpty(x.EmployeeBasicData.NationalId))
                .WithMessage("National ID must be 14 digits.");

            RuleFor(x => x.EmployeeBasicData.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.EmployeeBasicData.Email));

            RuleFor(x => x.EmployeeBasicData.PrivateMobile)
                .Matches(@"^\+?[0-9]{8,15}$").When(x => !string.IsNullOrEmpty(x.EmployeeBasicData.PrivateMobile));

            RuleFor(x => x.EmployeeOrganization.HireDate)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

            RuleFor(x => x.EmployeeBasicData.Birthdate)
                .LessThan(DateOnly.FromDateTime(DateTime.Today.AddYears(-18)))
                .WithMessage("Employee must be at least 18 years old.");

           // RuleFor(x => x.JobTitleId).GreaterThan(0);
            RuleFor(x => x.EmployeeOrganization.CompanyId).GreaterThan(0);
        }
    }

    public class EmployeeUpdateValidator : AbstractValidator<EmployeeUpdateDto>
    {
        public EmployeeUpdateValidator()
        {
           // Include(new EmployeeCreateValidator());

            RuleFor(x => x.EmployeeId).GreaterThan(0);
        }
    }

}
