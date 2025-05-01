using MyFeedbackHub.Domain;

namespace MyFeedbackHub.Api.Services.Abstraction;

public interface IAuthService
{
    string GenerateAccessToken(UserDomain user);

    string GenerateRefreshToken();

    bool ValidateRefreshToken(string refreshToken, out string username);
}
