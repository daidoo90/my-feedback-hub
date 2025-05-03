using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Project.GetById;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.Domain.Types;

namespace MyFeedbackHub.Api.Features.Project.GetById;

public sealed class ProjectResponseDto
{
    public Guid ProjectId { get; init; }
    public string Name { get; init; }
    public string Url { get; init; }
    public string Description { get; init; }

    internal static ProjectResponseDto? From(ProjectDomain? projectDomain)
    {
        if (projectDomain == null)
        {
            return default;
        }

        return new ProjectResponseDto
        {
            ProjectId = projectDomain!.ProjectId,
            Name = projectDomain!.Name,
            Url = projectDomain!.Url,
            Description = projectDomain!.Description
        };
    }
}

public sealed class GetProjectByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/projects/{id}", async (
            Guid id,
            IQueryHandler<GetProjectByIdQuery, ProjectDomain?> queryHandler,
            IUserService userService,
            IUserContext currentUser,
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

            var result = await queryHandler.HandleAsync(new GetProjectByIdQuery(id), cancellationToken);

            if (result.HasFailed)
            {
                return result.ToBadRequest("Get project by id failure");
            }

            if (result.Data != null
                && currentUser.OrganizationId != result.Data!.OrganizationId)
            {
                return Results.Forbid();
            }

            return Results.Ok(ProjectResponseDto.From(result.Data));
        })
        .WithName("Get project by id")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Get project")
        .WithDescription("Get project")
        .WithTags("Project")
        .RequireAuthorization();
    }
}
