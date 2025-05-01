using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain;
using MyFeedbackHub.Domain.Types;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Organization.Create;

public sealed record CreateNewOrganizationCommand(
    string Username,
    string Password,
    string CompanyName);

public sealed class CreateNewOrganizationCommandHandler(
    IFeedbackHubDbContext dbContext,
    ICryptoService cryptoService)
    : ICommandHandler<CreateNewOrganizationCommand>
{
    public async Task<ServiceResult> HandleAsync(CreateNewOrganizationCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(command.Username))
        {
            return ServiceResult.WithError(ErrorCodes.User.UsernameInvalid);
        }

        if (string.IsNullOrEmpty(command.Password))
        {
            return ServiceResult.WithError(ErrorCodes.User.PasswordInvalid);
        }

        if (string.IsNullOrEmpty(command.CompanyName))
        {
            return ServiceResult.WithError(ErrorCodes.Project.ProjectNameInvalid);
        }

        var now = DateTimeOffset.UtcNow;
        var organization = new OrganizationDomain
        {
            Name = command.CompanyName,
            CreatedOn = now,
            IsDeleted = false
        };

        await dbContext.Organizations.AddAsync(organization, cancellationToken);

        var project = new ProjectDomain
        {
            CreatedOn = now,
            IsDeleted = false,
            Name = "Default Project",
            Organization = organization
        };

        await dbContext.Projects.AddAsync(project, cancellationToken);

        var hashedPassword = cryptoService.HashPassword(command.Password, out var salt);

        var user = new UserDomain
        {
            Username = command.Username,
            Password = hashedPassword,
            Salt = Convert.ToBase64String(salt),
            OrganizationId = organization.OrganizationId,
            CreatedOn = DateTime.UtcNow,
            Status = UserStatusType.Active,
            Role = UserRoleType.OrganizationAdmin,
            Organization = organization
        };

        await dbContext.Users.AddAsync(user, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        organization.CreatedByUserId = user.UserId;
        project.CreatedByUserId = user.UserId;

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
