using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users;
using MyFeedbackHub.Domain.Types;

namespace MyFeedbackHub.Api.Features.Users;

public sealed record CreateNewUserRequestDto(
    string Username,
    UserRoleType Role,
    Guid? ProjectId);

public sealed class CreateNewUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/users", async (
            CreateNewUserRequestDto request,
            ICommandHandler<CreateNewUserCommand, CreateNewUserCommandResult> commandHandler,
            IUserContext currentUser,
            IUserService userService,
            CancellationToken cancellationToken = default) =>
        {
            if (currentUser.Role == UserRoleType.TeamMember
                || currentUser.Role == UserRoleType.Customer)
            {
                return Results.Forbid();
            }

            if (currentUser.Role == UserRoleType.ProjectAdmin)
            {
                var currentUserProjectIds = await userService.GetProjectIdsAsync(currentUser.UserId, cancellationToken);
                if (request.ProjectId.HasValue && !currentUserProjectIds.Contains(request.ProjectId.Value))
                {
                    return Results.Forbid();
                }
            }

            var result = await commandHandler.HandleAsync(
                new CreateNewUserCommand(
                    request.Username,
                    request.Role,
                    request.ProjectId),
                cancellationToken);

            if (result.HasFailed)
            {
                return result.ToBadRequest("User creation failure");
            }

            return Results.Ok(result.Data);
        })
        .WithName("CreateNewUser")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Create new user")
        .WithDescription("Create new user")
        .WithTags("User")
        .RequireAuthorization();
    }
}
