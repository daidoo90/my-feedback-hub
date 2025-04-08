using Microsoft.AspNetCore.Mvc;
using MyFeedbackHub.Api.Shared.Utils.Carter;

namespace MyFeedbackHub.Api.Features.Users.Update;

public sealed record UpdateUserRequestDto(
    string Email);

public sealed class UpdateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/users/{id}", ([FromQuery] Guid id, [FromBody] UpdateUserRequestDto newUserRequest) =>
        {
            return Results.Ok();
        })
        .WithName("UpdateUser")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Update user")
        .WithDescription("Update user")
        .WithTags("User")
        .RequireAuthorization();
    }
}
