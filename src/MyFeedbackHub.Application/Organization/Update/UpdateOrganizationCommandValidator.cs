using FluentValidation;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Organization;

public sealed class UpdateOrganizationCommandValidator : AbstractValidator<UpdateOrganizationCommand>
{
    private readonly IOrganizationService _organizationService;

    public UpdateOrganizationCommandValidator(IOrganizationService organizationService)

    {
        _organizationService = organizationService;

        ValidateOrganizationName();
    }

    private void ValidateOrganizationName()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Organization.OrganizationNameInvalid)
            .NotNull()
            .WithErrorCode(ErrorCodes.Organization.OrganizationNameInvalid)
            .MustAsync(async (organizationName, cancellationToken) =>
            {
                var existingOrganization = await _organizationService.GetAsync(organizationName, cancellationToken);

                return existingOrganization == null;
            })
            .WithErrorCode(ErrorCodes.Organization.OrganizationNameInvalid);
    }
}
