using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Types;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Feedback;

public sealed record GetFeedbackByIdQuery(Guid FeedbackId);

public sealed record GetFeedbackByIdResponse(
    Guid FeedbackId,
    string Title,
    string? Description,
    FeedbackStatusType Status,
    DateTimeOffset CreatedOn,
    Guid CreatedById,
    string CreatedBy);

public sealed class GetFeedbackByIdQueryHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    IUserContext currentUser,
    IAuthorizationService authorizationService)
    : IQueryHandler<GetFeedbackByIdQuery, GetFeedbackByIdResponse>
{
    public async Task<ServiceDataResult<GetFeedbackByIdResponse>> HandleAsync(GetFeedbackByIdQuery query, CancellationToken cancellationToken = default)
    {
        var dbContext = dbContextFactory.Create();

        var feedback = await dbContext
            .Feedbacks
            .Include(f => f.Comments)
            .Where(f => f.FeedbackId == query.FeedbackId
                        && !f.IsDeleted)
            .Join(dbContext.Users,
                feedback => feedback.CreatedBy,
                user => user.UserId,
                (feedback, user) => new
                {
                    feedback.FeedbackId,
                    feedback.Title,
                    feedback.Description,
                    feedback.Status,
                    feedback.CreatedOn,
                    CreatedById = feedback.CreatedBy,
                    CreatedBy = $"{user.FirstName} {user.LastName}",
                    feedback.ProjectId
                })
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);

        if (feedback == null)
        {
            return ServiceDataResult<GetFeedbackByIdResponse>.WithError(ErrorCodes.Feedback.NotFound);
        }

        if (!await authorizationService.CanAccessProjectAsync(feedback.ProjectId, cancellationToken))
        {
            return ServiceDataResult<GetFeedbackByIdResponse>.WithError(ErrorCodes.Feedback.NotFound);
        }

        return ServiceDataResult<GetFeedbackByIdResponse>.WithData(new GetFeedbackByIdResponse(
            feedback.FeedbackId,
            feedback.Title,
            feedback.Description,
            feedback.Status,
            feedback.CreatedOn,
            feedback.CreatedById,
            feedback.CreatedBy));
    }
}
