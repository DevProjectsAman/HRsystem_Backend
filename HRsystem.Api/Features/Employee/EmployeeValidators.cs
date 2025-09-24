namespace HRsystem.Api.Features.Employee
{
    using FluentValidation;
    using HRsystem.Api.Features.Employee.DTO;

    public class EmployeeCreateValidator : AbstractValidator<EmployeeCreateDto>
    {
        public EmployeeCreateValidator()
        {
            RuleFor(x => x.EmployeeCodeHr)
                .NotEmpty().MaximumLength(55);

            RuleFor(x => x.FirstName)
                .NotEmpty().MaximumLength(55);

            RuleFor(x => x.LastName)
                .NotEmpty().MaximumLength(55);

            

            RuleFor(x => x.NationalId)
                .Matches(@"^\d{14}$").When(x => !string.IsNullOrEmpty(x.NationalId))
                .WithMessage("National ID must be 14 digits.");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.PrivateMobile)
                .Matches(@"^\+?[0-9]{8,15}$").When(x => !string.IsNullOrEmpty(x.PrivateMobile));

            RuleFor(x => x.HireDate)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

            RuleFor(x => x.Birthdate)
                .LessThan(DateOnly.FromDateTime(DateTime.Today.AddYears(-18)))
                .WithMessage("Employee must be at least 18 years old.");

           // RuleFor(x => x.JobTitleId).GreaterThan(0);
            RuleFor(x => x.CompanyId).GreaterThan(0);
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
