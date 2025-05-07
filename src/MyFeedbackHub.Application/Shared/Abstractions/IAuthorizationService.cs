namespace MyFeedbackHub.Application.Shared.Abstractions;
public interface IAuthorizationService
{
    Task<bool> CanAccessProjectAsync(Guid projectId, CancellationToken cancellationToken = default);
}
