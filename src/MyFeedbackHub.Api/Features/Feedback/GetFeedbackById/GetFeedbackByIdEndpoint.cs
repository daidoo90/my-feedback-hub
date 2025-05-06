using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Feedback.GetFeedbackById;
using MyFeedbackHub.Application.Shared.Abstractions;

namespace MyFeedbackHub.Api.Features.Feedback.GetFeedbackById;

public sealed class GetFeedbackByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/feedbacks/{feedbackId}", async (
            Guid feedbackId,
            IQueryHandler<GetFeedbackByIdQuery, GetFeedbackByIdResponse> queryHandler,
            IUserContext currentUser,
            CancellationToken cancellationToken) =>
        {
            var result = await queryHandler.HandleAsync(new GetFeedbackByIdQuery(feedbackId), cancellationToken);

            if (result.HasFailed)
            {
                return result.ToBadRequest("Get feedback failure");
            }

            return Results.Ok(result.Data);
        })
        .WithName("Get feedback by id")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Get feedback")
        .WithDescription("Get organization")
        .WithTags("Feedback")
        .RequireAuthorization();
    }
}