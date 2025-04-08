using Microsoft.AspNetCore.Mvc;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Business.Create;
using MyFeedbackHub.Application.Shared.Abstractions;

namespace MyFeedbackHub.Api.Features.Business.Create;

public sealed record CreateNewBusinessRequestDto(
    string Username,
    string Password,
    string? Website);

public sealed class CreateNewBuisinessEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/businesses", async (
            [FromBody] CreateNewBusinessRequestDto request,
            ICommandHandler<CreateNewBusinessCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new CreateNewBusinessCommand(
                request.Username,
                request.Password),
                cancellationToken);

            if (result.HasFailed)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Business creation failure",
                    extensions: new Dictionary<string, object?>()
                    {
                        ["errorCode"] = result.ErrorCode
                    });
            }

            return Results.Created();
        })
        .WithName("Create new business")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Create new business")
        .WithDescription("Create new business")
        .WithTags("Business")
        .AllowAnonymous();
    }
}
