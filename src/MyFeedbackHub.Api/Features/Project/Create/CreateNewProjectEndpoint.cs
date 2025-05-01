using Microsoft.AspNetCore.Mvc;
using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Project.Create;
using MyFeedbackHub.Application.Shared.Abstractions;

namespace MyFeedbackHub.Api.Features.Project.Create;

public sealed record CreateNewProjectRequestDto(
    string Name,
    string Url,
    string Description);

public sealed class CreateNewProjectEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/projects", async (
            [FromBody] CreateNewProjectRequestDto request,
            ICommandHandler<CreateNewProjectCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new CreateNewProjectCommand(
                request.Name,
                request.Url,
                request.Description),
                cancellationToken);

            if (result.HasFailed)
            {
                return result.ToBadRequest("Project creation failure");
            }

            return Results.Created();
        })
        .WithName("Create new project")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Create new project")
        .WithDescription("Create new project")
        .WithTags("Project")
        .RequireAuthorization();
    }
}
