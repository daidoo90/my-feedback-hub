using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Types;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Feedback.Update;

public sealed record UpdateFeedbackCommand(
    Guid FeedbackId,
    string Title,
    string Description,
    FeedbackStatusType Status);

public sealed class UpdateFeedbackCommandHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    IUserContext currentUser)
    : ICommandHandler<UpdateFeedbackCommand>
{
    public async Task<ServiceResult> HandleAsync(UpdateFeedbackCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(command.Title))
        {
            return ServiceResult.WithError(ErrorCodes.Feedback.TitleInvalid);
        }

        if (string.IsNullOrEmpty(command.Description))
        {
            return ServiceResult.WithError(ErrorCodes.Feedback.DescriptionInvalid);
        }

        var dbContext = await dbContextFactory.CreateAsync(cancellationToken);

        var feedback = await dbContext.Feedbacks
            .SingleOrDefaultAsync(f => f.FeedbackId == command.FeedbackId
                                        && !f.IsDeleted,
                                        cancellationToken);

        if (feedback == null
            || feedback.IsDeleted
            || feedback.CreatedBy != currentUser.UserId)
        {
            return ServiceResult.WithError(ErrorCodes.Feedback.FeedbackInvalid);
        }

        feedback.Update(
            command.Title,
            command.Description,
            command.Status,
            DateTimeOffset.UtcNow,
            currentUser.UserId);

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
