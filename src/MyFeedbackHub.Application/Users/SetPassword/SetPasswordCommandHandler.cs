using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users.Create;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users.SetPassword;

public sealed record SetPasswordCommand(string Username, string Password);

public sealed class SetPasswordCommandHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    ICryptoService cryptoService)
    : ICommandHandler<SetPasswordCommand>
{
    public async Task<ServiceResult> HandleAsync(SetPasswordCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(command.Username))
        {
            return ServiceDataResult<CreateNewUserCommandResult>.WithError(ErrorCodes.User.UsernameInvalid);
        }

        var dbContext = await dbContextFactory.CreateAsync(cancellationToken);
        var user = await dbContext.Users
            .SingleOrDefaultAsync(u => u.Username == command.Username, cancellationToken);

        if (user == null)
        {
            return ServiceDataResult<CreateNewUserCommandResult>.WithError(ErrorCodes.User.UsernameInvalid);
        }

        var hashedPassword = cryptoService.HashPassword(command.Password, out var salt);
        user.SetFirstPassword(hashedPassword, Convert.ToBase64String(salt));

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
