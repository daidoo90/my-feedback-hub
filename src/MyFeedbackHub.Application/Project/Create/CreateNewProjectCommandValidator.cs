using FluentValidation;
using MyFeedbackHub.Application.Organization;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Project;

public sealed class CreateNewProjectCommandValidator : AbstractValidator<CreateNewProjectCommand>
{
    private readonly IOrganizationService _organizationService;
    private readonly IUserContext _currentUser;

    public CreateNewProjectCommandValidator(
        IOrganizationService organizationService,
        IUserContext currentUser)
    {
        _organizationService = organizationService;
        _currentUser = currentUser;

        ValidateOrganization();
        ValidateName();
    }

    private void ValidateOrganization()
    {
        RuleFor(x => x.Name)
            .MustAsync(async (organizationName, cancellationToken) =>
            {
                var organization = await _organizationService.GetAsync(_currentUser.OrganizationId, cancellationToken);

                return organization != null && !organization.IsDeleted;
            })
            .WithErrorCode(ErrorCodes.General.Error);
    }

    private void ValidateName()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Project.ProjectNameInvalid)
            .NotNull()
            .WithErrorCode(ErrorCodes.Project.ProjectNameInvalid)
            .MustAsync(async (name, cancellationToken) =>
            {
                var projects = await _organizationService.GetAllProjectsAsync(_currentUser.OrganizationId, cancellationToken);

                return !projects.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && !p.IsDeleted);
            })
            .WithErrorCode(ErrorCodes.Project.ProjectNameInvalid);
    }
}