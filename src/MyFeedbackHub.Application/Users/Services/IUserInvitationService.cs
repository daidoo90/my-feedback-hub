namespace MyFeedbackHub.Application.Users.Services;

public interface IUserInvitationService
{
    Task<string?> GetUserByInvitationTokenAsync(string token, CancellationToken cancellationToken = default);

    Task<string> GenerateAndStoreInvitationTokenAsync(string username, CancellationToken cancellationToken = default);
}
