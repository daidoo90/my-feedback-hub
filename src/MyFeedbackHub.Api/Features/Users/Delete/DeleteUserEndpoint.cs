using Microsoft.AspNetCore.Mvc;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users.Delete;

namespace MyFeedbackHub.Api.Features.Users.Delete;

public sealed class DeleteUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/users", async (
            [FromQuery] Guid id,
            ICommandHandler<DeleteUserCommand> command,
            CancellationToken cancellationToken) =>
        {
            var result = await command.HandleAsync(new DeleteUserCommand(id), cancellationToken);
            if (result.HasFailed)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "User deletion failed",
                    extensions: new Dictionary<string, object?>()
                    {
                        ["errorCode"] = result.ErrorCode
                    });
            }

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