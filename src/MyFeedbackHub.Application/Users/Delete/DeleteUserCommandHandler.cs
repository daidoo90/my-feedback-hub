using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.SharedKernel.ErrorCodes;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users.Delete;

public sealed record DeleteUserCommand(Guid UserId);

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
{
    private readonly IFeedbackHubDbContext _dbContext;

    public DeleteUserCommandHandler(IFeedbackHubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ServiceResult> HandleAsync(DeleteUserCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext
            .Users
            .SingleOrDefaultAsync(u => u.UserId == command.UserId, cancellationToken);

        if (!user?.IsActive ?? true)
        {
            return ServiceResult.WithError(ErrorCodes.User.UserId_Invalid);
        }

        user!.IsActive = false;
        user!.UpdatedOn = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
