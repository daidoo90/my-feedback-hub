using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Feedback;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Feedback;

namespace MyFeedbackHub.Infrastructure.Services;

public sealed class FeedbackService(IFeedbackHubDbContextFactory dbContextFactory) : IFeedbackService
{
    public async Task<CommentDomain?> GetCommentByIdAsync(Guid commentId, CancellationToken cancellationToken = default)
    {
        var dbContext = dbContextFactory.Create();

        return await dbContext
            .Comments
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.CommentId == commentId, cancellationToken);
    }

    public async Task<FeedbackDomain?> GetFeedbackByIdAsync(Guid feedbackId, CancellationToken cancellationToken = default)
    {
        var dbContext = dbContextFactory.Create();

        return await dbContext
            .Feedbacks
            .Include(f => f.Comments)
            .SingleOrDefaultAsync(c => c.FeedbackId == feedbackId, cancellationToken);
    }
}
