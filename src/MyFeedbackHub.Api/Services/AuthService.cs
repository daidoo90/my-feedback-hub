using Microsoft.IdentityModel.Tokens;
using MyFeedbackHub.Api.Services.Abstraction;
using MyFeedbackHub.SharedKernel.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MyFeedbackHub.Api.Services;

public class AuthService(IdentityConfigurations identityConfigurations) : IAuthService
{
    private readonly IdentityConfigurations _identityConfigurations = identityConfigurations;

    public string GenerateAccessToken(string username)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(ClaimTypes.Name, "Daniel Dimitrov"),
            new Claim(ClaimTypes.Email, username),
            new Claim("userId", Guid.NewGuid().ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_identityConfigurations.SecretKey)), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _identityConfigurations.Issuer,
            Audience = _identityConfigurations.Audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        var token = tokenHandler.WriteToken(securityToken);

        return token;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[50];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber)
            .Replace('/', '_')
            .Replace('+', '-')
            .TrimEnd('=');

        // TODO: Generate expiration date and store refresh token
    }

    public bool ValidateRefreshToken(string refreshToken, out string username)
    {
        username = "dani.damyanov.dimitrov@gmail.com";

        // TODO: Validate refresh token from redis cache

        return true;
    }
}
