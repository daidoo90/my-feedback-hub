using FluentValidation;
using MyFeedbackHub.Application.Organization;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Types;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IOrganizationService _organizationService;
    private readonly IUserContext _currentUser;
    private readonly IUserService _userService;

    public UpdateUserCommandValidator(
        IOrganizationService organizationService,
        IUserContext currentUser,
        IUserService userService)
    {
        _organizationService = organizationService;
        _currentUser = currentUser;
        _userService = userService;

        ValidateUsernameUniqueness();
        ValidateUsername();
    }
    private void ValidateUsernameUniqueness()
    {
        RuleFor(x => x)
            .MustAsync(async (user, cancellationToken) =>
            {
                var existingUser = await _userService.GetByUsernameAsync(user.Username, cancellationToken);
                if (existingUser != null)
                {
                    return existingUser.UserId == user.UserId && existingUser.Status == UserStatusType.Active;
                }

                return true;
            })
            .WithErrorCode(ErrorCodes.User.UsernameInvalid);
    }

    private void ValidateUsername()
    {
        RuleFor(x => x.Username)
             .NotEmpty()
             .WithErrorCode(ErrorCodes.User.UsernameInvalid)
             .NotNull()
             .WithErrorCode(ErrorCodes.User.UsernameInvalid)
             .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
             .WithErrorCode(ErrorCodes.User.UsernameInvalid);
    }
}
