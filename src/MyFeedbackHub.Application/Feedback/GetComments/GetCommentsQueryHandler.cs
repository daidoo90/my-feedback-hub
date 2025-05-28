using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Feedback.GetComments;

public sealed record GetCommentsQuery(Guid FeedbackId);

public sealed record CommentResponse(
    Guid CommentId,
    string Text,
    Guid? ParentComment,
    DateTimeOffset CreatedOn,
    Guid CreatedById,
    string CreatedBy);

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
            .Join(dbContext.Users,
                comment => comment.CreatedBy,
                user => user.UserId,
                (comment, user) => new
                {
                    comment.CommentId,
                    comment.Text,
                    ParentCommentId = comment.ParentCommentId,
                    user.CreatedOn,
                    CreatedById = user.UserId,
                    CreatedBy = $"{user.FirstName} {user.LastName}"
                })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return ServiceDataResult<IEnumerable<CommentResponse>>.WithData(comments.Select(c => new CommentResponse(
            c.CommentId,
            c.Text,
            c.ParentCommentId,// c.ParentComment != null ? new CommentResponse(c.ParentComment.CommentId, c.ParentComment.Text, null, c.ParentComment.CreatedOn, c.ParentComment.CreatedBy, string.Empty) : null,
            c.CreatedOn,
            c.CreatedById,
            c.CreatedBy)));
    }
}
