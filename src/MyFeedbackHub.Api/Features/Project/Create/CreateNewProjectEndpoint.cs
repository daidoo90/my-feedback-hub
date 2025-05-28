using Microsoft.AspNetCore.Mvc;
using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Project;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Types;

namespace MyFeedbackHub.Api.Features.Project;

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
            ICommandHandler<CreateNewProjectCommand> queryHandler,
            IUserContext currentUser,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.Role != UserRoleType.OrganizationAdmin)
            {
                return Results.Forbid();
            }

            var result = await queryHandler.HandleAsync(new CreateNewProjectCommand(
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
