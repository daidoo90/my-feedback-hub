using MyFeedbackHub.Application.Shared.Abstractions;

namespace MyFeedbackHub.Infrastructure.Services;

public sealed class AuthorizationService : IAuthorizationService
{
    public Task<bool> CanAccessProjectAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }
}
