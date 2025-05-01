using MyFeedbackHub.Api.Services.Abstraction;
using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users.GetByUsername;
using MyFeedbackHub.Domain;

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
            IQueryHandler<GetUserByUsernameRequest, UserDomain> getUserByIdHandler,
            IUserContext userContext,
            CancellationToken cancellation) =>
        {
            if (!authService.ValidateRefreshToken(request.RefreshToken, out var username))
            {
                return Results.BadRequest(nameof(request.RefreshToken));
            }

            var handlerResult = await getUserByIdHandler.HandleAsync(new GetUserByUsernameRequest(username), cancellation);
            if (handlerResult.HasFailed
                || handlerResult.Data == null
                || handlerResult.Data.UserId != userContext.UserId
                || handlerResult.Data.Status != Domain.Types.UserStatusType.Active)
            {
                return handlerResult.ToBadRequest("Refresh access token failure");
            }

            var accessToken = authService.GenerateAccessToken(handlerResult!.Data);
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
