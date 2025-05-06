using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Feedback.DeleteComment;

public sealed record DeleteCommentCommand(Guid CommentId, Guid FeedbackId);

public sealed class DeleteCommentCommandHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    IUserContext currentUser)
    : ICommandHandler<DeleteCommentCommand>
{
    public async Task<ServiceResult> HandleAsync(DeleteCommentCommand command, CancellationToken cancellationToken = default)
    {
        // TODO: Place validations here

        var dbContext = await dbContextFactory.CreateAsync(cancellationToken);

        var comment = await dbContext.Comments
            .SingleOrDefaultAsync(c => c.CommentId == command.CommentId
                                        && c.FeedbackId == command.FeedbackId, cancellationToken);

        if (comment == null
            || comment.IsDeleted
            || comment.CreatedBy != currentUser.UserId)
        {
            return ServiceResult.WithError(ErrorCodes.Comment.CommentInvalid);
        }

        comment.Delete(
            DateTimeOffset.UtcNow,
            currentUser.UserId);

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
