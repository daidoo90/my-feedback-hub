using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Feedback;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Feedback.CreateComment;

public sealed record CreateNewCommentCommand(
    string Text,
    Guid FeedbackId,
    Guid? ParentCommentId);

public sealed class CreateNewCommentHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    IUserContext currentUser)
    : ICommandHandler<CreateNewCommentCommand>
{
    public async Task<ServiceResult> HandleAsync(CreateNewCommentCommand command, CancellationToken cancellationToken = default)
    {
        // TODO: Place validations here

        var dbContext = await dbContextFactory.CreateAsync(cancellationToken);

        var newComment = CommentDomain.Create(
            command.Text,
            DateTimeOffset.UtcNow,
            currentUser.UserId,
            command.FeedbackId,
            command.ParentCommentId);

        await dbContext.Comments.AddAsync(newComment, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
