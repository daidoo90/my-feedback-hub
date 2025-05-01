using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Project.GetById;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain;

namespace MyFeedbackHub.Api.Features.Project.GetById;

public sealed class ProjectResponseDto
{
    public Guid UserId { get; init; }
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
            UserId = projectDomain!.ProjectId,
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
            IQueryHandler<GetProjectByIdQueryRequest, ProjectDomain?> handler,
            IUserContext userContext,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new GetProjectByIdQueryRequest(id), cancellationToken);

            if (result.HasFailed)
            {
                return result.ToBadRequest("Get project by id failure");
            }

            if (result.Data != null
                && userContext.OrganizationId != result.Data!.OrganizationId)
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
