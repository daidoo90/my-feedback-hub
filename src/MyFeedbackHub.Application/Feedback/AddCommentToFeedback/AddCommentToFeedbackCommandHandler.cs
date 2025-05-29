using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Feedback;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Feedback;

public sealed record CreateNewCommentCommand(
    string Text,
    Guid FeedbackId,
    Guid? ParentCommentId);

public sealed class AddCommentToFeedbackCommandHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    IUserContext currentUser,
    IAuthorizationService authorizationService,
    IValidator<CreateNewCommentCommand> validator)
    : ICommandHandler<CreateNewCommentCommand>
{
    public async Task<ServiceResult> HandleAsync(CreateNewCommentCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ServiceResult.WithError(validationResult.Errors.First().ErrorCode);
        }

        var dbContext = dbContextFactory.Create();

        var feedback = await dbContext
            .Feedbacks
            .Include(f => f.Comments)
            .SingleAsync(c => c.FeedbackId == command.FeedbackId, cancellationToken);

        if (!await authorizationService.CanAccessProjectAsync(feedback.ProjectId, cancellationToken))
        {
            return ServiceResult.WithError(ErrorCodes.Feedback.NotFound);
        }

        var newComment = CommentDomain.Create(
            command.Text,
            DateTimeOffset.UtcNow,
            currentUser.UserId,
            command.FeedbackId,
            command.ParentCommentId);

        feedback.AddComment(newComment);

        await dbContext.Comments.AddAsync(newComment, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
