using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.Domain.Types;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users.Create;

public sealed record CreateNewUserCommand(
    string Username,
    string Password,
    Guid OrganizationId,
    UserRoleType Role,
    Guid? ProjectId);

public sealed class CreateNewUserCommandHandler(
    IFeedbackHubDbContext dbContext,
    ICryptoService cryptoService,
    IUserContext userContext)
    : ICommandHandler<CreateNewUserCommand>
{
    public async Task<ServiceResult> HandleAsync(CreateNewUserCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(command.Username))
        {
            return ServiceResult.WithError(ErrorCodes.User.UsernameInvalid);
        }

        if (string.IsNullOrEmpty(command.Password))
        {
            return ServiceResult.WithError(ErrorCodes.User.PasswordInvalid);
        }

        var hashedPassword = cryptoService.HashPassword(command.Password, out var salt);
        var newUser = new UserDomain
        {
            Username = command.Username,
            Password = hashedPassword,
            Salt = Convert.ToBase64String(salt),
            OrganizationId = command.OrganizationId,
            Role = command.Role,
            Status = UserStatusType.Active,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedByUserId = userContext.UserId,
        };

        if (newUser.Role == UserRoleType.ProjectAdmin
            || newUser.Role == UserRoleType.TeamMember)
        {
            newUser.ProjectAccess.Add(new ProjectAccess
            {
                UserId = newUser.UserId,
                ProjectId = command.ProjectId.Value
            });
        }

        await dbContext.Users.AddAsync(newUser, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
