using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.Domain.Types;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Organization.Create;

public sealed record CreateNewOrganizationCommand(
    string Username,
    string Password,
    string CompanyName);

public sealed class CreateNewOrganizationCommandHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
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
        var organization = OrganizationDomain.Create(command.CompanyName, now);

        var hashedPassword = cryptoService.HashPassword(command.Password, out var salt);

        var user = UserDomain.Create(
            command.Username,
            hashedPassword,
            Convert.ToBase64String(salt),
            organization,
            UserStatusType.Active,
            UserRoleType.OrganizationAdmin,
            now,
            null);

        var project = ProjectDomain.Create(
            command.CompanyName,
            organization,
            now,
            user.UserId);

        organization.SetCreatedBy(user.UserId);

        var dbContext = await dbContextFactory.CreateAsync(cancellationToken);
        await dbContext.Organizations.AddAsync(organization, cancellationToken);
        await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.Projects.AddAsync(project, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
