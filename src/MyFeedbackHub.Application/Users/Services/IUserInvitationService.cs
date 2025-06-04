namespace MyFeedbackHub.Application.Users;

public interface IUserInvitationService
{
    Task<string?> GetInvitationTokenAsync(string username, CancellationToken cancellationToken = default);

    Task<string> GenerateAndStoreInvitationTokenAsync(string username, CancellationToken cancellationToken = default);
}
