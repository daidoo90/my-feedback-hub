using Microsoft.AspNetCore.Mvc;
using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Organization.Create;
using MyFeedbackHub.Application.Shared.Abstractions;

namespace MyFeedbackHub.Api.Features.Organization.Create;

public sealed record CreateNewOrganizationRequestDto(
    string Username,
    string Password,
    string CompanyName);

public sealed class CreateNewOrganizationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/organizations", async (
            [FromBody] CreateNewOrganizationRequestDto request,
            ICommandHandler<CreateNewOrganizationCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new CreateNewOrganizationCommand(
                request.Username,
                request.Password,
                request.CompanyName),
                cancellationToken);

            if (result.HasFailed)
            {
                return result.ToBadRequest("Organization creation failure");
            }

            return Results.Created();
        })
        .WithName("Create new organization")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Create new organization")
        .WithDescription("Create new organization")
        .WithTags("Organization")
        .AllowAnonymous();
    }
}
