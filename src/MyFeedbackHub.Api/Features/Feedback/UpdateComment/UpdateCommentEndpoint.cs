using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Feedback;
using MyFeedbackHub.Application.Shared.Abstractions;

namespace MyFeedbackHub.Api.Features.Feedback;

public sealed record UpdateCommentRequestDto(
    string Text,
    Guid? ParentCommentId
    );

public sealed class UpdateCommentEndponoit : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/feedbacks/{feedbackId}/comments/{commentId}", async (
            Guid feedbackId,
            Guid commentId,
            UpdateCommentRequestDto request,
            ICommandHandler<UpdateCommentCommand> commandHandler,
            CancellationToken cancellationToken) =>
        {
            var serviceResult = await commandHandler.HandleAsync(new UpdateCommentCommand(
                commentId,
                feedbackId,
                request.Text),
                cancellationToken);

            if (serviceResult.HasFailed)
            {
                return serviceResult.ToBadRequest("Updating feedback comment failure");
            }

            return Results.Ok();
        })
        .WithName("UpdateFeedbackComment")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Update feedback comment")
        .WithDescription("Update feedback comment")
        .WithTags("Feedback")
        .RequireAuthorization();
    }
}