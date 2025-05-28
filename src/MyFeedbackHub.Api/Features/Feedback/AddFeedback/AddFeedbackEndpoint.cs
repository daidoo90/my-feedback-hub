using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Feedback;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Types;

namespace MyFeedbackHub.Api.Features.Feedback;

public sealed record AddFeedbackRequestDto(
    string Title,
    string? Description,
    FeedbackType Type,
    Guid ProjectId
    );

public sealed class AddFeedbackEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/feedbacks", async (
            AddFeedbackRequestDto request,
            ICommandHandler<CreateNewFeedbackCommand> commandHandler,
            CancellationToken cancellationToken) =>
        {
            var serviceResult = await commandHandler.HandleAsync(new CreateNewFeedbackCommand(
                request.Title,
                request.Description,
                request.Type,
                request.ProjectId),
                cancellationToken);

            if (serviceResult.HasFailed)
            {
                return serviceResult.ToBadRequest("Feedback creation failure");
            }

            return Results.Created();
        })
        .WithName("AddFeedback")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Add feedback")
        .WithDescription("Add feedback")
        .WithTags("Feedback")
        .RequireAuthorization();
    }
}
