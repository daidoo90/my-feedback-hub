using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Feedback.UpdateComment;

public sealed record UpdateCommentCommand(
    Guid CommentId,
    Guid FeedbackId,
    string Text);

public sealed class UpdateCommentHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    IUserContext currentUser)
    : ICommandHandler<UpdateCommentCommand>
{
    public async Task<ServiceResult> HandleAsync(UpdateCommentCommand command, CancellationToken cancellationToken = default)
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

        comment.Update(
            command.Text,
            DateTimeOffset.UtcNow,
            currentUser.UserId);

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
