using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users.Services;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Infrastructure.Services;

public sealed class UserService(
    IFeedbackHubDbContextFactory dbContextFactory) : IUserService
{
    public async Task<ServiceDataResult<UserDomain>> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var dbContext = await dbContextFactory.CreateAsync(cancellationToken);
        var user = await dbContext
        .Users
        .AsNoTracking()
        .SingleOrDefaultAsync(u => u.Username == username, cancellationToken);

        if (user == null)
        {
            return ServiceDataResult<UserDomain>.WithError(ErrorCodes.Auth.UsernameOrPasswordInvalid);
        }

        return ServiceDataResult<UserDomain>.WithData(user);
    }

    public async Task<IEnumerable<Guid>> GetProjectIdsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var dbContext = await dbContextFactory.CreateAsync(cancellationToken);
        return await dbContext.ProjectAccess
            .Where(pa => pa.UserId == userId)
            .Select(pa => pa.ProjectId)
            .ToListAsync(cancellationToken);
    }
}
