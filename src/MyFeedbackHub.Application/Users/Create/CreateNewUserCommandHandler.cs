using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain;
using MyFeedbackHub.SharedKernel.ErrorCodes;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users.Create;

public sealed record CreateNewUserCommand(
    string Username,
    string Password,
    Guid BusinessId);

public sealed class CreateNewUserCommandHandler(
    IFeedbackHubDbContext dbContext,
    ICryptoService cryptoService)
    : ICommandHandler<CreateNewUserCommand>
{
    public async Task<ServiceResult> HandleAsync(CreateNewUserCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(command.Username))
        {
            return ServiceResult.WithError(ErrorCodes.User.Username_Invalid);
        }

        if (string.IsNullOrEmpty(command.Password))
        {
            return ServiceResult.WithError(ErrorCodes.User.Password_Invalid);
        }

        //TODO: Validate business
        var hashedPassword = cryptoService.HashPassword(command.Password, out var salt);

        await dbContext.Users.AddAsync(new UserDomain
        {
            Username = command.Username,
            Password = hashedPassword,
            Salt = Convert.ToBase64String(salt),
            BusinessId = command.BusinessId,
            IsActive = true,
            CreatedOn = DateTime.UtcNow
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
