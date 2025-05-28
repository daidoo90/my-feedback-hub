using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Project;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users;
using MyFeedbackHub.Domain.Types;

namespace MyFeedbackHub.Api.Features.Project;

public sealed class ProjectsResponseDto
{
    public int TotalCount { get; init; }

    public IEnumerable<ProjectResponseDto> Projects { get; init; }

    internal static ProjectsResponseDto? From(GetAllProjectsResponse projectsResponse)
    {
        return new ProjectsResponseDto
        {
            TotalCount = projectsResponse.TotalCount,
            Projects = projectsResponse
                .Projects
                .Select(ProjectResponseDto.From)
                .ToList(),
        };
    }
}

public sealed class GetAllProjectsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/projects", async (
            int? pageNumber,
            int? pageSize,
            IQueryHandler<GetAllProjectsQuery, GetAllProjectsResponse> queryHandler,
            IUserContext currentUser,
            IUserService userService,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.Role == UserRoleType.Customer)
            {
                return Results.Forbid();
            }

            var projectIds = currentUser.Role == UserRoleType.OrganizationAdmin
                            ? []
                            : await userService.GetProjectIdsAsync(currentUser.UserId, cancellationToken);

            var result = await queryHandler.HandleAsync(new GetAllProjectsQuery(pageNumber, pageSize, currentUser.OrganizationId, projectIds), cancellationToken);

            if (result.HasFailed)
            {
                return result.ToBadRequest("Get all projects failure");
            }

            return Results.Ok(ProjectsResponseDto.From(result!.Data));
        })
        .WithName("Get all projects")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Get all projects")
        .WithDescription("Get all projects")
        .WithTags("Project")
        .RequireAuthorization();
    }
}
