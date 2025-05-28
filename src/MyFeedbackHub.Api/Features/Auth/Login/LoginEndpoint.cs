using MyFeedbackHub.Api.Services.Abstraction;
using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Api.Features.Auth.Login;

public sealed record LoginRequestDto(
    string Username,
    string Password);

public sealed record LoginResponseDto(
    string AccessToken,
    string RefreshToken);

public sealed class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/login", async (
            LoginRequestDto request,
            IAuthService authService,
            IUserService userService,
            ICryptoService cryptoService,
            CancellationToken cancellationToken) =>
        {
            var user = await userService.GetByUsernameAsync(request.Username, cancellationToken);
            if (user == null)
            {
                return ServiceResult.WithError(ErrorCodes.Auth.UsernameOrPasswordInvalid).ToBadRequest("User authentication failure");
            }

            if (user == null
                || user.Status != Domain.Types.UserStatusType.Active)
            {
                return ServiceResult.WithError(ErrorCodes.Auth.UsernameOrPasswordInvalid).ToBadRequest("Authentication failure");
            }

            var isPasswordCorrect = cryptoService.VerifyPassword(request.Password, user.Password, user.Salt);
            if (!isPasswordCorrect)
            {
                return ServiceResult.WithError(ErrorCodes.Auth.UsernameOrPasswordInvalid).ToBadRequest("Authentication failure");
            }

            var accessToken = authService.GenerateAccessToken(user);
            var refreshToken = authService.GenerateRefreshToken();

            return Results.Ok(new LoginResponseDto(accessToken, refreshToken));
        })
        .WithName("Login")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Get access token & refresh token")
        .WithDescription("Get access token & refresh token")
        .WithTags("Auth")
        .AllowAnonymous();
    }
}
