using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Feedback.DeleteFeedback;
using MyFeedbackHub.Application.Shared.Abstractions;

namespace MyFeedbackHub.Api.Features.Feedback.DeleteFeedback;

public sealed class DeleteCommentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/feedbacks/{feedbackId}", async (
            Guid feedbackId,
            ICommandHandler<DeleteFeedbackCommand> commandHandler,
            CancellationToken cancellationToken) =>
        {
            var serviceResult = await commandHandler.HandleAsync(new DeleteFeedbackCommand(feedbackId), cancellationToken);

            if (serviceResult.HasFailed)
            {
                return serviceResult.ToBadRequest("Deleting feedback failure");
            }

            return Results.Ok();
        })
        .WithName("DeleteFeedback")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Delete feedback")
        .WithDescription("Delete feedback")
        .WithTags("Feedback")
        .RequireAuthorization();
    }
}