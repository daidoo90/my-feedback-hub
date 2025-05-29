using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Types;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Feedback;

public sealed record UpdateFeedbackCommand(
    Guid FeedbackId,
    string Title,
    string Description,
    FeedbackStatusType Status);

public sealed class UpdateFeedbackCommandHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    IUserContext currentUser,
    IValidator<UpdateFeedbackCommand> validator)
    : ICommandHandler<UpdateFeedbackCommand>
{
    public async Task<ServiceResult> HandleAsync(UpdateFeedbackCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ServiceResult.WithError(validationResult.Errors.First().ErrorCode);
        }

        var dbContext = dbContextFactory.Create();
        var feedback = await dbContext.Feedbacks
            .SingleAsync(f => f.FeedbackId == command.FeedbackId
                                        && !f.IsDeleted,
                                        cancellationToken);

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
