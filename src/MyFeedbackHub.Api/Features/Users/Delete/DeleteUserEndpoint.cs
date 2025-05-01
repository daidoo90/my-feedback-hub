using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users.Delete;
using MyFeedbackHub.Domain.Types;

namespace MyFeedbackHub.Api.Features.Users.Delete;

public sealed class DeleteUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/users/{id}", async (
            Guid id,
            ICommandHandler<DeleteUserCommand> command,
            IUserContext userContext,
            CancellationToken cancellationToken) =>
        {
            if (userContext.Role == UserRoleType.Customer
                || userContext.Role == UserRoleType.TeamMember)
            {
                return Results.Forbid();
            }

            var result = await command.HandleAsync(new DeleteUserCommand(id), cancellationToken);
            if (result.HasFailed)
            {
                return result.ToBadRequest("User deletion failure");
            }

            return Results.Ok();
        })
        .WithName("DeleteUser")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Delete user")
        .WithDescription("Delete user")
        .WithTags("User")
        .RequireAuthorization();
    }
}