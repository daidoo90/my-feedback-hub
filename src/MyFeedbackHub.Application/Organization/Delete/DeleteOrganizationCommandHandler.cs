using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Organization;

public sealed record DeleteOrganizationCommand(Guid OrganizationId);

public sealed class DeleteOrganizationCommandHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    IUserContext currentUser)
    : ICommandHandler<DeleteOrganizationCommand>
{
    public async Task<ServiceResult> HandleAsync(
        DeleteOrganizationCommand command,
        CancellationToken cancellationToken = default)
    {
        var dbContext = dbContextFactory.Create();
        var organization = await dbContext
            .Organizations
            .SingleOrDefaultAsync(x => x.OrganizationId == command.OrganizationId
                                    && x.OrganizationId == currentUser.OrganizationId, cancellationToken);
        if (organization == null)
        {
            return ServiceResult.WithError(ErrorCodes.Organization.OrganizationInvalid);
        }

        organization.Delete(currentUser.UserId);

        var projects = await dbContext
            .Projects
            .Where(p => p.OrganizationId == command.OrganizationId)
            .ToListAsync(cancellationToken);

        foreach (var project in projects)
        {
            project.Delete(currentUser.UserId);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
