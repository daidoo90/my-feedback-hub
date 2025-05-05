using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users.Services;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.Domain.Types;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users.Create;

public sealed record CreateNewUserCommand(
    string Username,
    Guid OrganizationId,
    UserRoleType Role,
    Guid? ProjectId);

public sealed record CreateNewUserCommandResult(string InvitationToken);

public sealed class CreateNewUserCommandHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    IUserContext currentUser,
    IUserInvitationService userInvitationService)
    : ICommandHandler<CreateNewUserCommand, CreateNewUserCommandResult>
{
    public async Task<ServiceDataResult<CreateNewUserCommandResult>> HandleAsync(CreateNewUserCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(command.Username))
        {
            return ServiceDataResult<CreateNewUserCommandResult>.WithError(ErrorCodes.User.UsernameInvalid);
        }

        var newUser = UserDomain.Create(
            command.Username,
            command.OrganizationId,
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
