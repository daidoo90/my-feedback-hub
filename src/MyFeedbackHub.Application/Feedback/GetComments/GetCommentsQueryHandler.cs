using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Feedback.GetComments;

public sealed record GetCommentsQuery(Guid FeedbackId);

public sealed record CommentResponse(
    Guid CommentId,
    string Text,
    Guid? ParentCommentId,
    DateTimeOffset CreatedOn,
    Guid CreatedBy);

public sealed class GetCommentsQueryHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    IUserContext currentUser)
    : IQueryHandler<GetCommentsQuery, IEnumerable<CommentResponse>>
{
    public async Task<ServiceDataResult<IEnumerable<CommentResponse>>> HandleAsync(GetCommentsQuery query, CancellationToken cancellationToken = default)
    {
        // TODO: Get feedback and validate that current user has access to this project

        var dbContext = await dbContextFactory.CreateAsync(cancellationToken);

        var comments = await dbContext
            .Comments
            .Where(c => c.FeedbackId == query.FeedbackId
                        && !c.Feedback.IsDeleted
                        && !c.IsDeleted)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return ServiceDataResult<IEnumerable<CommentResponse>>.WithData(comments.Select(c => new CommentResponse(
            c.CommentId,
            c.Text,
            c.ParentCommentId,
            c.CreatedOn,
            c.CreatedBy)));
    }
}
