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
    IUserContext currentUser) : ICommandHandler<UpdateUserCommand>
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

        user.Update(
            command.FirstName,
            command.LastName,
            command.PhoneNumber,
            DateTimeOffset.UtcNow,
            currentUser.UserId);

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
