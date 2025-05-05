using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Feedback;
using MyFeedbackHub.Domain.Types;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Feedback.Create;

public sealed record CreateNewFeedbackCommand(
    string Title,
    string Description,
    FeedbackType Type,
    Guid ProjectId);

public sealed class CreateNewFeedbackCommandHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    IUserContext currentUser)
    : ICommandHandler<CreateNewFeedbackCommand>
{
    public async Task<ServiceResult> HandleAsync(CreateNewFeedbackCommand command, CancellationToken cancellationToken = default)
    {
        // TODO: Add feedback metadata validations
        // TODO: Validate user permissions and user-project relation

        var dbContext = await dbContextFactory.CreateAsync(cancellationToken);

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
