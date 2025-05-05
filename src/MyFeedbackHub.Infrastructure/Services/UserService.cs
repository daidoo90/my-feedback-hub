using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using StackExchange.Redis;

namespace MyFeedbackHub.Infrastructure.Services;

public class UserService(
    IFeedbackHubDbContextFactory dbContextFactory,
    IConnectionMultiplexer redis) : IUserService
{
    public async Task<IEnumerable<Guid>> GetProjectIdsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var dbContext = await dbContextFactory.CreateAsync(cancellationToken);
        return await dbContext.ProjectAccess
            .Where(pa => pa.UserId == userId)
            .Select(pa => pa.ProjectId)
            .ToListAsync(cancellationToken);
    }

    public async Task<string> GetUserByInvitationToken(string token)
    {
        var db = redis.GetDatabase();

        return await db.StringGetAsync(token);
    }

    public async Task SetInvitationTokenAsync(string username, string token)
    {
        var db = redis.GetDatabase();

        await db.StringSetAsync(token, username);
    }
}
