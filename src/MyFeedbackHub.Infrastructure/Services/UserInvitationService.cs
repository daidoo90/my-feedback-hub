using Microsoft.Extensions.Caching.Distributed;
using MyFeedbackHub.Application.Users.Services;

namespace MyFeedbackHub.Infrastructure.Services;

public sealed class UserInvitationService(IDistributedCache cache)
    : IUserInvitationService
{
    public async Task<string?> GetUserByInvitationTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var key = $"invite:{token}";
        var username = await cache.GetStringAsync(key, cancellationToken);

        return username;
    }

    public async Task<string> GenerateAndStoreInvitationTokenAsync(string username, CancellationToken cancellationToken = default)
    {
        var token = Guid.NewGuid().ToString();
        var key = $"invite:{token}";

        await cache.SetStringAsync(key, username, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
        }, cancellationToken);

        return token;
    }
}
