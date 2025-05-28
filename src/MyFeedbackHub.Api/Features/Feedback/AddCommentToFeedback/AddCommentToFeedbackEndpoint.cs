using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Feedback;
using MyFeedbackHub.Application.Shared.Abstractions;

namespace MyFeedbackHub.Api.Features.Feedback;

public sealed record AddCommentRequestDto(
    string Text,
    Guid? ParentCommentId
    );

public sealed class AddCommentToFeedbackEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/feedbacks/{feedbackId}/comments", async (
            Guid feedbackId,
            AddCommentRequestDto request,
            ICommandHandler<CreateNewCommentCommand> commandHandler,
            CancellationToken cancellationToken) =>
        {
            var serviceResult = await commandHandler.HandleAsync(new CreateNewCommentCommand(
                request.Text,
                feedbackId,
                request.ParentCommentId),
                cancellationToken);

            if (serviceResult.HasFailed)
            {
                return serviceResult.ToBadRequest("Adding comment to feedback failure");
            }

            return Results.Created();
        })
        .WithName("AddCommentToFeedback")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Add comment to feedback")
        .WithDescription("Add comment to feedback")
        .WithTags("Feedback")
        .RequireAuthorization();
    }
}
