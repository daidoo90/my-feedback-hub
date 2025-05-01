using MyFeedbackHub.Api.Features.Project.GetById;
using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Project.GetAll;
using MyFeedbackHub.Application.Shared.Abstractions;

namespace MyFeedbackHub.Api.Features.Project.GetAll;

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
            IQueryHandler<GetAllProjectsQueryRequest, GetAllProjectsResponse> handler,
            IUserContext userContext,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new GetAllProjectsQueryRequest(pageNumber, pageSize), cancellationToken);

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
