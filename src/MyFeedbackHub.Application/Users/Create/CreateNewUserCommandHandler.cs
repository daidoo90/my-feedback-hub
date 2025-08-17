using FluentValidation;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users.SendWelcomeEmail;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.Domain.Types;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users;

public sealed record CreateNewUserCommand(
    string Username,
    UserRoleType Role,
    Guid? ProjectId);

public sealed record CreateNewUserCommandResult(Guid Token);

public sealed class CreateNewUserCommandHandler(
    IUnitOfWork unitOfWork,
    IUserContext currentUser,
    IValidator<CreateNewUserCommand> validator,
    IOutboxService outboxService)
    : ICommandHandler<CreateNewUserCommand, CreateNewUserCommandResult>
{
    public async Task<ServiceDataResult<CreateNewUserCommandResult>> HandleAsync(CreateNewUserCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ServiceDataResult<CreateNewUserCommandResult>.WithError(validationResult.Errors.First().ErrorCode);
        }

        var newUser = UserDomain.Create(
            command.Username,
            currentUser.OrganizationId,
            UserStatusType.PendingInvitation,
            command.Role,
            DateTimeOffset.UtcNow,
            currentUser.UserId);

        if (newUser.Role == UserRoleType.ProjectAdmin
            || newUser.Role == UserRoleType.TeamMember)
        {
            newUser.ProjectAccess.Add(new ProjectAccess
            {
                UserId = newUser.UserId,
                ProjectId = command.ProjectId.Value
            });
        }

        await unitOfWork.DbContext.Users.AddAsync(newUser, cancellationToken);

        var messageId = await outboxService.AddAsync(new UserCreatedEvent(newUser), cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceDataResult<CreateNewUserCommandResult>.WithData(new CreateNewUserCommandResult(messageId));
    }
}
