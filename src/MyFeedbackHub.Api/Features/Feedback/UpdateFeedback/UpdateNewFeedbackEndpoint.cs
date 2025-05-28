using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Feedback;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Types;

namespace MyFeedbackHub.Api.Features.Feedback;

public sealed record UpdateFeedbackRequestDto(
    string Title,
    string Description,
    FeedbackStatusType Status,
    Guid ProjectId
    );

public sealed class UpdateFeedbackEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/feedbacks/{id}", async (
            Guid id,
            UpdateFeedbackRequestDto request,
            ICommandHandler<UpdateFeedbackCommand> commandHandler,
            CancellationToken cancellationToken) =>
        {
            var serviceResult = await commandHandler.HandleAsync(new UpdateFeedbackCommand(
                id,
                request.Title,
                request.Description,
                request.Status),
                cancellationToken);

            if (serviceResult.HasFailed)
            {
                return serviceResult.ToBadRequest("Feedback update failure");
            }

            return Results.Ok();
        })
        .WithName("UpdateFeedback")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Update feedback")
        .WithDescription("Update feedback")
        .WithTags("Feedback")
        .RequireAuthorization();
    }
}
