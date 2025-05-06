using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Feedback.CreateComment;
using MyFeedbackHub.Application.Shared.Abstractions;

namespace MyFeedbackHub.Api.Features.Feedback.AddComment;

public sealed record AddNewCommentRequestDto(
    string Text,
    Guid? ParentCommentId
    );

public sealed class AddNewCommentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/feedbacks/{feedbackId}/comments", async (
            Guid feedbackId,
            AddNewCommentRequestDto request,
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
                return serviceResult.ToBadRequest("Adding feedback comment failure");
            }

            return Results.Created();
        })
        .WithName("AddNewFeedbackComment")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Add new feedback comment")
        .WithDescription("Add new feedback comment")
        .WithTags("Feedback")
        .RequireAuthorization();
    }
}
