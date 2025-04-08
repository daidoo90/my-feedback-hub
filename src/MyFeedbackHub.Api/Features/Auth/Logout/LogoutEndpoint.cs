using MyFeedbackHub.Api.Shared.Utils.Carter;

namespace MyFeedbackHub.Api.Features.Auth.Logout;

public sealed class LogoutEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/logout", () =>
        {
            return Results.Ok();
        })
        .WithName("Logout")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Logout")
        .WithDescription("Logout")
        .WithTags("Auth")
        .RequireAuthorization();
    }
}
