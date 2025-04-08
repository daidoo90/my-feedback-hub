using Microsoft.AspNetCore.Mvc;
using MyFeedbackHub.Api.Shared.Utils.Carter;

namespace MyFeedbackHub.Api.Features.Users.DeleteUser;

public sealed class DeleteUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/users", ([FromQuery] Guid id) =>
        {
            return Results.Ok();
        })
        .WithName("DeleteUser")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Delete user")
        .WithDescription("Delete user")
        .WithTags("User")
        .RequireAuthorization();
    }
}