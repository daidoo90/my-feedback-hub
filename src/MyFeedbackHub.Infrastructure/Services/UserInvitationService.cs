using Microsoft.Extensions.Caching.Distributed;
using MyFeedbackHub.Application.Users;

namespace MyFeedbackHub.Infrastructure.Services;

public sealed class UserInvitationService(IDistributedCache cache)
    : IUserInvitationService
{
    public async Task<string?> GetInvitationTokenAsync(string username, CancellationToken cancellationToken = default)
    {
        var key = $"invitation-token:{username}";

        return await cache.GetStringAsync(key, cancellationToken);
    }

    public async Task<string> GenerateAndStoreInvitationTokenAsync(string username, CancellationToken cancellationToken = default)
    {
        var token = Guid.NewGuid().ToString();
        var key = $"invitation-token:{username}";

        await cache.SetStringAsync(key, username, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
        }, cancellationToken);

        return token;
    }
}
