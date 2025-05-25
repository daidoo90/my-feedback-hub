using FluentValidation;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.Domain.Types;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Organization.Create;

public sealed record CreateNewOrganizationCommand(
    string Username,
    string Password,
    string OrganizationName);

public sealed class CreateNewOrganizationCommandHandler(
    IValidator<CreateNewOrganizationCommand> validator,
    IFeedbackHubDbContextFactory dbContextFactory,
    ICryptoService cryptoService)
    : ICommandHandler<CreateNewOrganizationCommand>
{
    public async Task<ServiceResult> HandleAsync(CreateNewOrganizationCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ServiceResult.WithError(validationResult.Errors.First().ErrorCode);
        }

        var now = DateTimeOffset.UtcNow;
        var organization = OrganizationDomain.Create(command.OrganizationName, now);

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
            command.OrganizationName,
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
