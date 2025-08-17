using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Feedback;

public sealed record UpdateCommentCommand(
    Guid CommentId,
    Guid FeedbackId,
    string Text);

public sealed class UpdateCommentCommandHandler(
    IUnitOfWork unitOfWork,
    IUserContext currentUser)
    : ICommandHandler<UpdateCommentCommand>
{
    public async Task<ServiceResult> HandleAsync(UpdateCommentCommand command, CancellationToken cancellationToken = default)
    {
        // TODO: Place validations here

        var comment = await unitOfWork.DbContext.Comments
            .SingleOrDefaultAsync(c => c.CommentId == command.CommentId
                                        && c.FeedbackId == command.FeedbackId, cancellationToken);

        if (comment == null
            || comment.IsDeleted
            || comment.CreatedBy != currentUser.UserId)
        {
            return ServiceResult.WithError(ErrorCodes.Comment.NotFound);
        }

        comment.Update(
            command.Text,
            DateTimeOffset.UtcNow,
            currentUser.UserId);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
