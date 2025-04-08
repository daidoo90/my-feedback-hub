using MyFeedbackHub.Api.Services.Abstraction;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users.GetById;
using MyFeedbackHub.Domain;

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
            IQueryHandler<GetByIdQueryRequest, UserDomain> handler,
            ICryptoService cryptoService) =>
        {
            var user = await handler.HandleAsync(new GetByIdQueryRequest(request.Username));
            if (user == null)
            {
                return Results.BadRequest("Invalid user.");
            }

            var isPasswordCorrect = cryptoService.VerifyPassword(request.Password, user.Data.Password, user.Data.Salt);
            if (!isPasswordCorrect)
            {
                return Results.BadRequest("Your password is not correct.");
            }

            var accessToken = authService.GenerateAccessToken(request.Username);
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
