using FluentValidation;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.Domain.Types;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users;

public sealed record CreateNewUserCommand(
    string Username,
    UserRoleType Role,
    Guid? ProjectId);

public sealed record CreateNewUserCommandResult(string InvitationToken);

public sealed class CreateNewUserCommandHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    IUserContext currentUser,
    IUserInvitationService userInvitationService,
    IValidator<CreateNewUserCommand> validator)
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

        var dbContext = await dbContextFactory.CreateAsync(cancellationToken);
        await dbContext.Users.AddAsync(newUser, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        var invitationToken = await userInvitationService.GenerateAndStoreInvitationTokenAsync(newUser.Username, cancellationToken);

        return ServiceDataResult<CreateNewUserCommandResult>.WithData(new CreateNewUserCommandResult(invitationToken));
    }
}
