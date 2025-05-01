using MyFeedbackHub.Api.Services.Abstraction;
using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users.GetByUsername;
using MyFeedbackHub.Domain;
using MyFeedbackHub.Domain.Types;
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
            IQueryHandler<GetUserByUsernameRequest, UserDomain> queryHandler,
            ICryptoService cryptoService) =>
        {
            var handlerResult = await queryHandler.HandleAsync(new GetUserByUsernameRequest(request.Username));
            if (handlerResult.HasFailed
                || handlerResult.Data == null
                || handlerResult.Data.Status != UserStatusType.Active)
            {
                return ServiceResult.WithError(ErrorCodes.User.UserInvalid).ToBadRequest("Authentication failure");
            }

            var isPasswordCorrect = cryptoService.VerifyPassword(request.Password, handlerResult.Data.Password, handlerResult.Data.Salt);
            if (!isPasswordCorrect)
            {
                return ServiceResult.WithError(ErrorCodes.User.PasswordInvalid).ToBadRequest("Authentication failure");
            }

            var accessToken = authService.GenerateAccessToken(handlerResult.Data);
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
