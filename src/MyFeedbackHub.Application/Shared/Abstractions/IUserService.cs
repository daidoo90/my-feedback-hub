namespace MyFeedbackHub.Application.Shared.Abstractions;

public interface IUserService
{
    Task<IEnumerable<Guid>> GetProjectIdsAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<string> GetUserByInvitationToken(string token);

    Task SetInvitationTokenAsync(string username, string token);
}
