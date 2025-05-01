using Microsoft.AspNetCore.Mvc;
using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Organization.Update;
using MyFeedbackHub.Application.Shared.Abstractions;

namespace MyFeedbackHub.Api.Features.Organization.Update;

public sealed record UpdateOrganizationRequestDto(
    string Name,
    string TaxId,
    string Country,
    string City,
    string ZipCode,
    string State,
    string StreetLine1,
    string StreetLine2
    );

public sealed class UpdateOrganizationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/organizations", async (
            [FromBody] UpdateOrganizationRequestDto request,
            ICommandHandler<UpdateOrganizationCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new UpdateOrganizationCommand(
                request.Name,
                request.TaxId,
                request.Country,
                request.City,
                request.ZipCode,
                request.State,
                request.StreetLine1,
                request.StreetLine2),
                cancellationToken);

            if (result.HasFailed)
            {
                return result.ToBadRequest("Organization update failure");
            }

            return Results.Created();
        })
        .WithName("Update organization")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Update organization")
        .WithDescription("Update organization")
        .WithTags("Organization")
        .AllowAnonymous();
    }
}
