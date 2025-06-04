using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Application.Organization;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Types;

namespace MyFeedbackHub.Api.Features.Organization.Delete;

public sealed class DeleteOrganizationEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/organizations/{id}", async (
            Guid id,
            IUserContext currentUser,
            ICommandHandler<DeleteOrganizationCommand> commandHandler,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.Role != UserRoleType.OrganizationAdmin)
            {
                return Results.Forbid();
            }

            var result = await commandHandler.HandleAsync(
                new DeleteOrganizationCommand(id),
                cancellationToken);

            if (result.HasFailed)
            {
                return result.ToBadRequest("Organization deletion failure");
            }

            return Results.Ok();
        })
        .WithName("Delete organization")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Delete organization")
        .WithDescription("Delete organization")
        .WithTags("Organization");
    }
}