using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Organization.Update;

public sealed record UpdateOrganizationCommand(
    string Name,
    string TaxId,
    string Country,
    string City,
    string ZipCode,
    string State,
    string StreetLine1,
    string StreetLine2);

public sealed class UpdateOrganizationCommandHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    IUserContext currentUser)
    : ICommandHandler<UpdateOrganizationCommand>
{
    public async Task<ServiceResult> HandleAsync(UpdateOrganizationCommand command, CancellationToken cancellationToken = default)
    {
        var dbContext = await dbContextFactory.CreateAsync(cancellationToken);
        var organization = await dbContext
            .Organizations
            .SingleOrDefaultAsync(o => o.OrganizationId == currentUser.OrganizationId, cancellationToken);

        if (organization == null)
        {
            return ServiceResult.WithError(ErrorCodes.Organization.OrganizationInvalid);
        }

        organization.Update(
            command.Name,
            command.TaxId,
            DateTimeOffset.UtcNow,
            currentUser.UserId);

        organization.Address.Update(
            command.Country,
            command.City,
            command.ZipCode,
            command.State,
            command.StreetLine1,
            command.StreetLine2);

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
