namespace MyFeedbackHub.Api.Services.Abstraction;

public interface IAuthService
{
    string GenerateAccessToken(string username);

    string GenerateRefreshToken();

    bool ValidateRefreshToken(string refreshToken, out string username);
}
