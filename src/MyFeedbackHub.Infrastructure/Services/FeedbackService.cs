using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Feedback;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Feedback;

namespace MyFeedbackHub.Infrastructure.Services;

public sealed class FeedbackService(IUnitOfWork unitOfWork) : IFeedbackService
{
    public async Task<CommentDomain?> GetCommentByIdAsync(Guid commentId, CancellationToken cancellationToken = default)
    {
        return await unitOfWork
            .DbContext
            .Comments
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.CommentId == commentId, cancellationToken);
    }

    public async Task<FeedbackDomain?> GetFeedbackByIdAsync(Guid feedbackId, CancellationToken cancellationToken = default)
    {

        return await unitOfWork
            .DbContext
            .Feedbacks
            .Include(f => f.Comments)
            .SingleOrDefaultAsync(c => c.FeedbackId == feedbackId, cancellationToken);
    }
}
