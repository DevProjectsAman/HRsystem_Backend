using FluentValidation;
using HRsystem.Api.Features.Organization.Groups.Create;

public class CreateGroupValidator : AbstractValidator<CreateGroupCommand>
{
    public CreateGroupValidator()
    {
        RuleFor(x => x.GroupName)
            .NotEmpty().WithMessage("GroupName is required")
            .MaximumLength(100).WithMessage("GroupName cannot exceed 100 characters");
    }
}
