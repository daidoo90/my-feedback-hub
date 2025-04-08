using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain;
using MyFeedbackHub.SharedKernel.ErrorCodes;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Business.Create;

public sealed record CreateNewBusinessCommand(
    string Username,
    string Password);

public sealed class CreateNewBusinessCommandHandler(
    IFeedbackHubDbContext dbContext,
    ICryptoService cryptoService)
    : ICommandHandler<CreateNewBusinessCommand>
{
    public async Task<ServiceResult> HandleAsync(CreateNewBusinessCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(command.Username))
        {
            return ServiceResult.WithError(ErrorCodes.User.Username_Invalid);
        }

        if (string.IsNullOrEmpty(command.Password))
        {
            return ServiceResult.WithError(ErrorCodes.User.Password_Invalid);
        }

        var business = new BusinessDomain
        {
            CreatedOn = DateTime.UtcNow
        };

        await dbContext.Businesses.AddAsync(business, cancellationToken);

        var hashedPassword = cryptoService.HashPassword(command.Password, out var salt);

        await dbContext.Users.AddAsync(new UserDomain
        {
            Username = command.Username,
            Password = hashedPassword,
            Salt = Convert.ToBase64String(salt),
            BusinessId = business.BusinessId,
            CreatedOn = DateTime.UtcNow,
            IsActive = true
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
