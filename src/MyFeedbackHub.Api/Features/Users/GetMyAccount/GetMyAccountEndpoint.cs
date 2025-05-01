using MyFeedbackHub.Api.Features.Users.GetById;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users.GetById;
using MyFeedbackHub.Domain;

namespace MyFeedbackHub.Api.Features.Users.GetMyAccount;

public sealed class GetMyAccountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/me", async (
             IQueryHandler<GetByUserIdQueryRequest, UserDomain?> handler,
             IUserContext userContext,
             CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new GetByUserIdQueryRequest(userContext.UserId), cancellationToken);

            return Results.Ok(UserResponseDto.From(result.Data));
        })
        .WithName("GetMyAccount")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Get my account")
        .WithDescription("Get my account")
        .WithTags("User")
        .RequireAuthorization();
    }
}