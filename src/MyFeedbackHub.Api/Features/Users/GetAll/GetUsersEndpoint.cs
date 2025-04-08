using MyFeedbackHub.Api.Shared.Utils.Carter;

namespace MyFeedbackHub.Api.Features.Users.GetAll;

public sealed class GetUsersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users", () =>
        {
            return Results.Ok();
        })
        .WithName("GetUsers")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Get users")
        .WithDescription("Get users")
        .WithTags("User")
        .RequireAuthorization();
    }
}