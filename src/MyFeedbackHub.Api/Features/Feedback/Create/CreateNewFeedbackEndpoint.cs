using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Feedback.Create;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Types;

namespace MyFeedbackHub.Api.Features.Feedback.Create;

public sealed record CreateNewFeedbackRequestDto(
    string Title,
    string Description,
    FeedbackType Type,
    Guid ProjectId
    );

public sealed class CreateNewFeedbackEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/feedbacks", async (
            CreateNewFeedbackRequestDto request,
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
        .WithName("CreateNewFeedback")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Create new feedback")
        .WithDescription("Create new feedback")
        .WithTags("Feedback")
        .RequireAuthorization();
    }
}
