using MyFeedbackHub.Api.Services.Abstraction;
using MyFeedbackHub.Api.Shared.Utils.Carter;

namespace MyFeedbackHub.Api.Features.Auth.RefreshAccessToken;

public sealed record RefreshAccessTokenRequest(string RefreshToken);

public sealed record RefreshAccessTokenResponse(string AccessToken);

public sealed class RefreshAccessTokenEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/refresh", (
            RefreshAccessTokenRequest request,
            IAuthService authService) =>
        {
            if (!authService.ValidateRefreshToken(request.RefreshToken, out var username))
            {
                return Results.BadRequest(nameof(request.RefreshToken));
            }

            // TODO: Validate username from refresh token is username of the current user

            var accessToken = authService.GenerateAccessToken(username);
            return Results.Ok(new RefreshAccessTokenResponse(accessToken));
        })
        .WithName("RefreshAccessToken")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Refresh access token")
        .WithDescription("Refresh access token")
        .WithTags("Auth")
        .RequireAuthorization();
    }
}
