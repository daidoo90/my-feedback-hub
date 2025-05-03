using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;

namespace MyFeedbackHub.Infrastructure.Services;

public class UserService(IFeedbackHubDbContext feedbackHubDbContext) : IUserService
{
    public async Task<IEnumerable<Guid>> GetProjectIdsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await feedbackHubDbContext.ProjectAccess
            .Where(pa => pa.UserId == userId)
            .Select(pa => pa.ProjectId)
            .ToListAsync(cancellationToken);
    }
}
