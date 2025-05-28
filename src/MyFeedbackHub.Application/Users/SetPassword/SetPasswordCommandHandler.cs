using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users;

public sealed record SetPasswordCommand(string Username, string Password);

public sealed class SetPasswordCommandHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    ICryptoService cryptoService,
    IValidator<SetPasswordCommand> validator)
    : ICommandHandler<SetPasswordCommand>
{
    public async Task<ServiceResult> HandleAsync(SetPasswordCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ServiceDataResult<CreateNewUserCommandResult>.WithError(validationResult.Errors.First().ErrorCode);
        }

        var dbContext = await dbContextFactory.CreateAsync(cancellationToken);
        var user = await dbContext.Users
            .SingleAsync(u => u.Username == command.Username, cancellationToken);

        var hashedPassword = cryptoService.HashPassword(command.Password, out var salt);
        user.SetFirstPassword(hashedPassword, Convert.ToBase64String(salt));

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
