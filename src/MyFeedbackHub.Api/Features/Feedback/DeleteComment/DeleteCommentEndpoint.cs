using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Feedback.DeleteComment;
using MyFeedbackHub.Application.Shared.Abstractions;

namespace MyFeedbackHub.Api.Features.Feedback.DeleteComment;

public sealed class DeleteCommentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/feedbacks/{feedbackId}/comments/{commentId}", async (
            Guid feedbackId,
            Guid commentId,
            ICommandHandler<DeleteCommentCommand> commandHandler,
            CancellationToken cancellationToken) =>
        {
            var serviceResult = await commandHandler.HandleAsync(new DeleteCommentCommand(commentId, feedbackId),
                cancellationToken);

            if (serviceResult.HasFailed)
            {
                return serviceResult.ToBadRequest("Deleting feedback comment failure");
            }

            return Results.Ok();
        })
        .WithName("DeleteFeedbackComment")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Delete feedback comment")
        .WithDescription("Delete feedback comment")
        .WithTags("Feedback")
        .RequireAuthorization();
    }
}