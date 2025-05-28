using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users;
using MyFeedbackHub.Domain.Types;

namespace MyFeedbackHub.Api.Features.Users;

public sealed class DeleteUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/users/{id}", async (
            Guid id,
            ICommandHandler<DeleteUserCommand> commandHandler,
            IUserContext currentUser,
            IUserService userService,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.Role == UserRoleType.Customer
                || currentUser.Role == UserRoleType.TeamMember)
            {
                return Results.Forbid();
            }

            if (currentUser.UserId == id)
            {
                return Results.Forbid();
            }

            if (currentUser.Role == UserRoleType.ProjectAdmin)
            {
                var currentUserProjectIds = await userService.GetProjectIdsAsync(currentUser.UserId, cancellationToken);
                var deletedUserProjectIds = await userService.GetProjectIdsAsync(id, cancellationToken);

                if (!deletedUserProjectIds.Any(p => currentUserProjectIds.Contains(p)))
                {
                    return Results.Forbid();
                }
            }

            var result = await commandHandler.HandleAsync(new DeleteUserCommand(id), cancellationToken);
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