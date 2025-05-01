using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users.Update;

public sealed record UpdateUserCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    string Username,
    string PhoneNumber);

public sealed class UpdateUserCommandHandler(
    IFeedbackHubDbContext dbContext,
    IUserContext userContext) : ICommandHandler<UpdateUserCommand>
{
    public async Task<ServiceResult> HandleAsync(UpdateUserCommand command, CancellationToken cancellationToken = default)
    {
        var user = await dbContext
            .Users
            .SingleOrDefaultAsync(u => u.UserId == command.UserId, cancellationToken);

        if (user?.Status != Domain.Types.UserStatusType.Active)
        {
            return ServiceResult.WithError(ErrorCodes.User.UserInvalid);
        }

        user.FirstName = command.FirstName;
        user.LastName = command.LastName;
        user.PhoneNumber = command.PhoneNumber;
        user.Username = command.Username;
        user.UpdatedOn = DateTimeOffset.UtcNow;
        user.UpdatedOnByUserId = userContext.UserId;

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
