using Microsoft.AspNetCore.Mvc;
using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Project;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users;
using MyFeedbackHub.Domain.Types;

namespace MyFeedbackHub.Api.Features.Project;

public sealed record UpdateProjectRequestDto(
    string? Name,
    string? Url,
    string? Description);

public sealed class UpdateProjectEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/projects/{id}", async (
            Guid id,
            [FromBody] UpdateProjectRequestDto request,
            ICommandHandler<UpdateProjectCommand> queryHandler,
            IUserContext currentUser,
            IUserService userService,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.Role == UserRoleType.TeamMember
                || currentUser.Role == UserRoleType.Customer)
            {
                return Results.Forbid();
            }

            if (currentUser.Role == UserRoleType.ProjectAdmin)
            {
                var currentUserProjectIds = await userService.GetProjectIdsAsync(currentUser.UserId, cancellationToken);
                if (!currentUserProjectIds.Contains(id))
                {
                    return Results.Forbid();
                }
            }

            var result = await queryHandler.HandleAsync(new UpdateProjectCommand(
                id,
                request.Name,
                request.Url,
                request.Description),
                cancellationToken);

            if (result.HasFailed)
            {
                return result.ToBadRequest("Project update failure");
            }

            return Results.Ok();
        })
        .WithName("Update project")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Update project")
        .WithDescription("Update project")
        .WithTags("Project")
        .RequireAuthorization();
    }
}
