using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Organization;

public sealed record DeleteOrganizationCommand(Guid OrganizationId);

public sealed class DeleteOrganizationCommandHandler(
    IUnitOfWork unitOfWork,
    IUserContext currentUser)
    : ICommandHandler<DeleteOrganizationCommand>
{
    public async Task<ServiceResult> HandleAsync(
        DeleteOrganizationCommand command,
        CancellationToken cancellationToken = default)
    {
        var organization = await unitOfWork
            .DbContext
            .Organizations
            .SingleOrDefaultAsync(x => x.OrganizationId == command.OrganizationId
                                    && x.OrganizationId == currentUser.OrganizationId, cancellationToken);
        if (organization == null)
        {
            return ServiceResult.WithError(ErrorCodes.Organization.OrganizationInvalid);
        }

        organization.Delete(currentUser.UserId);

        var projects = await unitOfWork
            .DbContext
            .Projects
            .Where(p => p.OrganizationId == command.OrganizationId)
            .ToListAsync(cancellationToken);

        foreach (var project in projects)
        {
            project.Delete(currentUser.UserId);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
