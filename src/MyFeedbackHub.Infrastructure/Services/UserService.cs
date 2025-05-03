using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;

namespace MyFeedbackHub.Infrastructure.Services;

public class UserService(IFeedbackHubDbContextFactory dbContextFactory) : IUserService
{
    public async Task<IEnumerable<Guid>> GetProjectIdsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var dbContext = await dbContextFactory.CreateAsync(cancellationToken);
        return await dbContext.ProjectAccess
            .Where(pa => pa.UserId == userId)
            .Select(pa => pa.ProjectId)
            .ToListAsync(cancellationToken);
    }
}
