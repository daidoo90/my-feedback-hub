using FluentValidation;
using MyFeedbackHub.Application.Users.Services;
using MyFeedbackHub.Domain.Types;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users.SetPassword;

public sealed class SetPasswordCommandValidator : AbstractValidator<SetPasswordCommand>
{
    private readonly IUserService _userService;

    public SetPasswordCommandValidator(IUserService userService)
    {
        _userService = userService;

        ValidateUsername();
        ValidatePassword();
    }
    private void ValidateUsername()
    {
        RuleFor(x => x.Username)
            .NotNull()
            .WithErrorCode(ErrorCodes.User.UsernameInvalid)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.User.UsernameInvalid)
            .MustAsync(async (username, cancellationToken) =>
            {
                var user = await _userService.GetByUsernameAsync(username, cancellationToken);

                return user != null && user.Status == UserStatusType.PendingInvitation;
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
