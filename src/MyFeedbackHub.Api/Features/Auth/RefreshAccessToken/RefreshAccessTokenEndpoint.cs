using MyFeedbackHub.Api.Services.Abstraction;
using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users.Services;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Api.Features.Auth.RefreshAccessToken;

public sealed record RefreshAccessTokenRequest(string RefreshToken);

public sealed record RefreshAccessTokenResponse(string AccessToken);

public sealed class RefreshAccessTokenEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/refresh", async (
            RefreshAccessTokenRequest request,
            IAuthService authService,
            IUserService userService,
            IUserContext userContext,
            CancellationToken cancellation) =>
        {
            if (!authService.ValidateRefreshToken(request.RefreshToken, out var username))
            {
                return Results.BadRequest(nameof(request.RefreshToken));
            }

            var user = await userService.GetByUsernameAsync(username, cancellation);

            if (user == null
                || user.UserId != userContext.UserId
                || user.Status != Domain.Types.UserStatusType.Active)
            {
                return ServiceResult.WithError(ErrorCodes.Auth.UsernameOrPasswordInvalid).ToBadRequest("Refresh access token failure");
            }

            var accessToken = authService.GenerateAccessToken(user);
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
