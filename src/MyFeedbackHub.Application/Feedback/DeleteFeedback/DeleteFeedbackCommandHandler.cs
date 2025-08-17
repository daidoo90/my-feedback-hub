using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Feedback;

public sealed record DeleteFeedbackCommand(Guid FeedbackId);

public class DeleteFeedbackCommandHandler(
    IUnitOfWork unitOfWork,
    IUserContext currentUser)
    : ICommandHandler<DeleteFeedbackCommand>
{
    public async Task<ServiceResult> HandleAsync(DeleteFeedbackCommand command, CancellationToken cancellationToken = default)
    {
        var feedback = await unitOfWork
            .DbContext
            .Feedbacks
            .SingleOrDefaultAsync(f => f.FeedbackId == command.FeedbackId
                                      && f.CreatedBy == currentUser.UserId, cancellationToken);

        if (feedback == null
            || feedback.IsDeleted
            || feedback.CreatedBy != currentUser.UserId)
        {
            return ServiceResult.WithError(ErrorCodes.Feedback.FeedbackInvalid);
        }

        feedback.Delete(DateTimeOffset.UtcNow, currentUser.UserId);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
