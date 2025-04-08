using MyFeedbackHub.Api.Shared.Utils.Carter;
using System.Collections.Concurrent;

namespace MyFeedbackHub.Api.Features.Feedback.Post;

public sealed record PostFeedbackRequestDto(string Feedback);

public sealed class PostFeedbackEndpoint : ICarterModule
{
    // Temporary in memory storage because of not having DB
    private readonly ConcurrentBag<string> _feedbacks = new();

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/feedbacks", (PostFeedbackRequestDto request) =>
        {
            _feedbacks.Add(request.Feedback);

            return Results.Ok();
        })
        .WithName("PostFeedback")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Post new feedback")
        .WithDescription("Post new feedback")
        .WithTags("Feedback")
        .AllowAnonymous();
    }
}
