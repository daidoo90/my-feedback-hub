using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.SharedKernel.ErrorCodes;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users.Update;

public sealed record UpdateUserCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    string Username);

public sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
{
    private readonly IFeedbackHubDbContext _dbContext;

    public UpdateUserCommandHandler(IFeedbackHubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ServiceResult> HandleAsync(UpdateUserCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext
            .Users
            .SingleOrDefaultAsync(u => u.UserId == command.UserId, cancellationToken);

        if (!user?.IsActive ?? true)
        {
            return ServiceResult.WithError(ErrorCodes.User.UserId_Invalid);
        }

        user!.FirstName = command.FirstName;
        user!.LastName = command.LastName;
        user!.Username = command.Username;
        user!.UpdatedOn = DateTimeOffset.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
