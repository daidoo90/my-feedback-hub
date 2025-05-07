using MyFeedbackHub.Domain.Feedback;

namespace MyFeedbackHub.Application.Feedback.Services;
public interface IFeedbackService
{
    Task<FeedbackDomain?> GetFeedbackByIdAsync(Guid feedbackId, CancellationToken cancellationToken = default);

    Task<CommentDomain?> GetCommentByIdAsync(Guid commentId, CancellationToken cancellationToken = default);
}
