using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users;

public sealed record SetPasswordCommand(string Username, string Password);

public sealed class SetPasswordCommandHandler(
    IUnitOfWork unitOfWork,
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

        var user = await unitOfWork
            .DbContext
            .Users
            .SingleAsync(u => u.Username == command.Username, cancellationToken);

        var hashedPassword = cryptoService.HashPassword(command.Password, out var salt);
        user.SetFirstPassword(hashedPassword, Convert.ToBase64String(salt));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
