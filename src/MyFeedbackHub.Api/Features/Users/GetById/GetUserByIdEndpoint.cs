using Microsoft.AspNetCore.Mvc;
using MyFeedbackHub.Api.Shared.Utils.Carter;

namespace MyFeedbackHub.Api.Features.Users.GetById;

public sealed class GetUserByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/{id}", ([FromQuery] Guid id) =>
        {
            return Results.Ok();
        })
        .WithName("GetUser")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Get user")
        .WithDescription("Get user")
        .WithTags("User")
        .RequireAuthorization();
    }
}