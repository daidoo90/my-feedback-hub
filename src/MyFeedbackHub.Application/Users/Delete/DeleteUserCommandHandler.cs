using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Types;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users.Delete;

public sealed record DeleteUserCommand(Guid UserId);

public sealed class DeleteUserCommandHandler(
    IFeedbackHubDbContext dbContext,
    IUserContext userContext) : ICommandHandler<DeleteUserCommand>
{
    public async Task<ServiceResult> HandleAsync(DeleteUserCommand command, CancellationToken cancellationToken = default)
    {
        var user = await dbContext
            .Users
            .SingleOrDefaultAsync(u => u.UserId == command.UserId
                                        && u.OrganizationId == userContext.OrganizationId, cancellationToken);

        if (user == null || user.Status == UserStatusType.Inactive)
        {
            return ServiceResult.WithError(ErrorCodes.User.UserInvalid);
        }

        user.Status = UserStatusType.Inactive;
        user.DeletedOn = DateTimeOffset.UtcNow;
        user.DeletedByUserId = userContext.UserId;

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
