using MyFeedbackHub.Application.Feedback.Services;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Feedback;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Feedback.CreateComment;

public sealed record CreateNewCommentCommand(
    string Text,
    Guid FeedbackId,
    Guid? ParentCommentId);

public sealed class CreateNewCommentCommandHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    IUserContext currentUser,
    IFeedbackService feedbackService,
    IAuthorizationService authorizationService)
    : ICommandHandler<CreateNewCommentCommand>
{
    public async Task<ServiceResult> HandleAsync(CreateNewCommentCommand command, CancellationToken cancellationToken = default)
    {
        var dbContext = await dbContextFactory.CreateAsync(cancellationToken);

        var feedback = await feedbackService.GetFeedbackByIdAsync(command.FeedbackId, cancellationToken);
        if (feedback == null
            || feedback.IsDeleted)
        {
            return ServiceResult.WithError(ErrorCodes.Feedback.NotFound);
        }

        if (!await authorizationService.CanAccessProjectAsync(feedback.ProjectId, cancellationToken))
        {
            return ServiceResult.WithError(ErrorCodes.Feedback.NotFound);
        }

        if (string.IsNullOrEmpty(command.Text))
        {
            return ServiceResult.WithError(ErrorCodes.Comment.CommentInvalid);
        }

        if (command.ParentCommentId != null)
        {
            var parentComment = await feedbackService.GetCommentByIdAsync(command.ParentCommentId.Value, cancellationToken);
            if (parentComment == null)
            {
                return ServiceResult.WithError(ErrorCodes.Comment.CommentInvalid);
            }
        }

        var newComment = CommentDomain.Create(
            command.Text,
            DateTimeOffset.UtcNow,
            currentUser.UserId,
            command.FeedbackId,
            command.ParentCommentId);

        feedback.AddComment(newComment);

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
