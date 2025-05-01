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
    IFeedbackHubDbContext dbContext,
    IUserContext userContext)
    : ICommandHandler<UpdateOrganizationCommand>
{
    public async Task<ServiceResult> HandleAsync(UpdateOrganizationCommand command, CancellationToken cancellationToken = default)
    {
        var organization = await dbContext
            .Organizations
            .SingleOrDefaultAsync(o => o.OrganizationId == userContext.OrganizationId, cancellationToken);

        if (organization == null)
        {
            return ServiceResult.WithError(ErrorCodes.Organization.OrganizationInvalid);
        }

        organization.Name = command.Name;
        organization.TaxID = command.TaxId;
        organization.Address.Country = command.Country;
        organization.Address.City = command.City;
        organization.Address.ZipCode = command.ZipCode;
        organization.Address.State = command.State;
        organization.Address.StreetLine1 = command.StreetLine1;
        organization.Address.StreetLine2 = command.StreetLine2;
        organization.UpdatedOn = DateTimeOffset.UtcNow;
        organization.UpdatedOnByUserId = userContext.UserId;

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
