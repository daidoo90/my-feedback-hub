using Microsoft.AspNetCore.Mvc;
using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Project.Update;
using MyFeedbackHub.Application.Shared.Abstractions;

namespace MyFeedbackHub.Api.Features.Project.Update;

public sealed record UpdateProjectRequestDto(
    string Name,
    string Url,
    string Description);

public sealed class UpdateProjectEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/projects/{id}", async (
            Guid id,
            [FromBody] UpdateProjectRequestDto request,
            ICommandHandler<UpdateProjectCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new UpdateProjectCommand(
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
