using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Feedback.GetComments;
using MyFeedbackHub.Application.Shared.Abstractions;

namespace MyFeedbackHub.Api.Features.Feedback.GetComments;

public sealed class GetCommentsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/feedbacks/{feedbackId}/comments", async (
            Guid feedbackId,
            IQueryHandler<GetCommentsQuery, IEnumerable<CommentResponse>> queryHandler,
            IUserContext currentUser,
            CancellationToken cancellationToken) =>
        {
            var result = await queryHandler.HandleAsync(new GetCommentsQuery(feedbackId), cancellationToken);

            if (result.HasFailed)
            {
                return result.ToBadRequest("Get all comments failure");
            }

            return Results.Ok(result!.Data);
        })
        .WithName("Get all comments")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Get all comments")
        .WithDescription("Get all comments")
        .WithTags("Feedback")
        .RequireAuthorization();
    }
}

