using FluentValidation;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Feedback;
using MyFeedbackHub.Domain.Types;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Feedback;

public sealed record CreateNewFeedbackCommand(
    string Title,
    string? Description,
    FeedbackType Type,
    Guid ProjectId);

public sealed class AddFeedbackCommandHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    IUserContext currentUser,
    IValidator<CreateNewFeedbackCommand> validator)
    : ICommandHandler<CreateNewFeedbackCommand>
{
    public async Task<ServiceResult> HandleAsync(CreateNewFeedbackCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ServiceResult.WithError(validationResult.Errors.First().ErrorCode);
        }

        var dbContext = dbContextFactory.Create();

        var newFeedback = FeedbackDomain.Create(
            command.Title,
            command.Description,
            command.Type,
            command.ProjectId,
            DateTimeOffset.UtcNow,
            currentUser.UserId);

        await dbContext.Feedbacks.AddAsync(newFeedback, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
