using FluentValidation;
using MyFeedbackHub.Application.Organization.Services;
using MyFeedbackHub.Application.Users.Services;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Organization.Create;

public sealed class CreateNewOrganizationCommandValidator : AbstractValidator<CreateNewOrganizationCommand>
{
    private readonly IOrganizationService _organizationService;
    private readonly IUserService _userService;

    public CreateNewOrganizationCommandValidator(
        IOrganizationService organizationService,
        IUserService userService)

    {
        _organizationService = organizationService;
        _userService = userService;

        ValidateOrganizationName();
        ValidatePassword();
        ValidateUsername();
    }

    private void ValidateOrganizationName()
    {
        RuleFor(x => x.OrganizationName)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Organization.OrganizationNameInvalid)
            .NotNull()
            .WithErrorCode(ErrorCodes.Organization.OrganizationNameInvalid)
            .MustAsync(async (organizationName, cancellationToken) =>
            {
                var existingOrganization = await _organizationService.GetAsync(organizationName, cancellationToken);

                return existingOrganization == null;
            })
            .WithErrorCode(ErrorCodes.Organization.OrganizationNameInvalid);
    }

    private void ValidateUsername()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.User.UsernameInvalid)
            .NotNull()
            .WithErrorCode(ErrorCodes.User.UsernameInvalid)
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
            .WithErrorCode(ErrorCodes.User.UsernameInvalid)
            .MustAsync(async (username, cancellationToken) =>
            {
                var existingUser = await _userService.GetByUsernameAsync(username, cancellationToken);
                return existingUser == null;

            })
            .WithErrorCode(ErrorCodes.User.UsernameInvalid);
    }

    private void ValidatePassword()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.User.PasswordInvalid)
            .NotNull()
            .WithErrorCode(ErrorCodes.User.PasswordInvalid)
            .Matches(@"^(?=.*[A-Za-z])(?=.*\d).{8,}$")
            .WithErrorCode(ErrorCodes.User.PasswordNotStrong);
    }
}
