using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Feedback;

public sealed record DeleteCommentCommand(Guid CommentId, Guid FeedbackId);

public sealed class DeleteCommentCommandHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    IUserContext currentUser,
    IAuthorizationService authorizationService)
    : ICommandHandler<DeleteCommentCommand>
{
    public async Task<ServiceResult> HandleAsync(DeleteCommentCommand command, CancellationToken cancellationToken = default)
    {
        var dbContext = dbContextFactory.Create();

        var feedback = await dbContext
            .Feedbacks
            .Include(f => f.Comments)
            .SingleOrDefaultAsync(c => c.FeedbackId == command.FeedbackId, cancellationToken);

        if (feedback == null
            || feedback.IsDeleted)
        {
            return ServiceResult.WithError(ErrorCodes.Feedback.NotFound);
        }

        if (!await authorizationService.CanAccessProjectAsync(feedback.ProjectId, cancellationToken))
        {
            return ServiceResult.WithError(ErrorCodes.Feedback.NotFound);
        }

        var comment = feedback.Comments.SingleOrDefault(c => c.CommentId == command.CommentId);
        if (command == null
            || comment!.IsDeleted
            || comment.CreatedBy != currentUser.UserId)
        {
            return ServiceResult.WithError(ErrorCodes.Comment.NotFound);
        }

        feedback.DeleteComment(command.CommentId, currentUser.UserId);

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
