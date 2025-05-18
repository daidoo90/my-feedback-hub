using MyFeedbackHub.Api.Shared.Utils;
using MyFeedbackHub.Api.Shared.Utils.Carter;
using MyFeedbackHub.Application.Feedback.GetAllFeedbacks;
using MyFeedbackHub.Application.Organization.Services;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users.Services;
using MyFeedbackHub.Domain.Feedback;
using MyFeedbackHub.Domain.Types;

namespace MyFeedbackHub.Api.Features.Feedback.GetAllFeedbacks;

public class FeedbackResponseDto
{
    public Guid FeedbackId { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string? Description { get; private set; } = null;

    public FeedbackStatusType Status { get; private set; }

    public FeedbackType Type { get; private set; }

    internal static FeedbackResponseDto? From(FeedbackDomain? feedbackDomain)
    {
        if (feedbackDomain == null)
        {
            return default;
        }

        return new FeedbackResponseDto()
        {
            FeedbackId = feedbackDomain!.FeedbackId,
            Description = feedbackDomain!.Description,
            Title = feedbackDomain!.Title,
            Status = feedbackDomain.Status,
            Type = feedbackDomain.Type,
        };
    }
}

public sealed class FeedbacksResponseDto
{
    public int TotalCount { get; init; }

    public IEnumerable<FeedbackResponseDto> Feedbacks { get; init; }

    internal static FeedbacksResponseDto? From(GetAllFeedbacksResponse feedbacksResponse)
    {
        return new FeedbacksResponseDto
        {
            TotalCount = feedbacksResponse.TotalCount,
            Feedbacks = feedbacksResponse
                .Feedbacks
                .Select(FeedbackResponseDto.From)
                .ToList(),
        };
    }
}

public sealed class GetFeedbacksEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/feedbacks", async (
           int? pageNumber,
           int? pageSize,
           IQueryHandler<GetAllFeedbacksQuery, GetAllFeedbacksResponse> queryHandler,
           IUserContext currentUser,
           IUserService userService,
           IOrganizationService organizationService,
           CancellationToken cancellationToken) =>
        {
            var projectIds = currentUser.Role == UserRoleType.OrganizationAdmin
                            ? await organizationService.GetProjectsAsync(currentUser.OrganizationId)
                            : await userService.GetProjectIdsAsync(currentUser.UserId, cancellationToken);

            var result = await queryHandler.HandleAsync(new GetAllFeedbacksQuery(
                pageNumber,
                pageSize,
                projectIds), cancellationToken);

            if (result.HasFailed)
            {
                return result.ToBadRequest("Get all feedbacks failure");
            }

            return Results.Ok(FeedbacksResponseDto.From(result!.Data));
        })
       .WithName("GetFeedbacks")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Get feedbacks")
        .WithDescription("Get feedbacks")
        .WithTags("Feedback")
        .RequireAuthorization();
    }
}