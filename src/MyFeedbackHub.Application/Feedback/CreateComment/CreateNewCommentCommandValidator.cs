using FluentValidation;
using MyFeedbackHub.Application.Feedback.Services;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Feedback.CreateComment;

public sealed class CreateNewCommentCommandValidator : AbstractValidator<CreateNewCommentCommand>
{
    private readonly IFeedbackService _feedbackService;

    public CreateNewCommentCommandValidator(
        IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;

        ValidateComment();
        ValidateParentComment();
        ValidateFeedback();
    }

    private void ValidateParentComment()
    {
        RuleFor(x => x.ParentCommentId)
            .MustAsync(async (parentCommentId, cancellationToken) =>
            {
                if (!parentCommentId.HasValue)
                {
                    return true;
                }

                var parentComment = await _feedbackService.GetCommentByIdAsync(parentCommentId.Value, cancellationToken);
                return parentComment != null && !parentComment.IsDeleted;
            })
            .WithErrorCode(ErrorCodes.Comment.CommentInvalid);
    }

    private void ValidateFeedback()
    {
        RuleFor(x => x.FeedbackId)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorCodes.Feedback.FeedbackInvalid)
            .MustAsync(async (feedbackId, cancellationToken) =>
            {
                var feedback = await _feedbackService.GetFeedbackByIdAsync(feedbackId, cancellationToken);
                return feedback != null && !feedback.IsDeleted;
            })
            .WithErrorCode(ErrorCodes.Feedback.FeedbackInvalid);
    }

    private void ValidateComment()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Comment.CommentInvalid)
            .NotNull()
            .WithErrorCode(ErrorCodes.Comment.CommentInvalid);
    }
}
