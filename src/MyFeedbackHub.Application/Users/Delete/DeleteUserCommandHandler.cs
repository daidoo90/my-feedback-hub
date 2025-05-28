using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Types;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users;

public sealed record DeleteUserCommand(Guid UserId);

public sealed class DeleteUserCommandHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    IUserContext currentUser) : ICommandHandler<DeleteUserCommand>
{
    public async Task<ServiceResult> HandleAsync(DeleteUserCommand command, CancellationToken cancellationToken = default)
    {
        var dbContext = await dbContextFactory.CreateAsync(cancellationToken);
        var user = await dbContext
            .Users
            .SingleOrDefaultAsync(u => u.UserId == command.UserId
                                        && u.OrganizationId == currentUser.OrganizationId, cancellationToken);

        if (user == null || user.Status == UserStatusType.Inactive)
        {
            return ServiceResult.WithError(ErrorCodes.User.UserInvalid);
        }

        user.Delete(currentUser.UserId);

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
