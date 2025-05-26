using FluentValidation;
using MyFeedbackHub.Application.Organization.Services;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Project.Update;

public sealed class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    private readonly IOrganizationService _organizationService;
    private readonly IUserContext _currentUser;

    public UpdateProjectCommandValidator(
        IOrganizationService organizationService,
        IUserContext currentUser)

    {
        _organizationService = organizationService;
        _currentUser = currentUser;

        ValidateOrganization();
        ValidateProject();
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

    private void ValidateProject()
    {
        RuleFor(x => x.ProjectId)
            .NotEqual(x => Guid.Empty)
            .WithErrorCode(ErrorCodes.Project.ProjectInvalid)
            .MustAsync(async (projectId, cancellationToken) =>
            {
                var projects = await _organizationService.GetAllProjectsAsync(_currentUser.OrganizationId, cancellationToken);
                var project = projects.SingleOrDefault(p => p.ProjectId == projectId);

                return project != null &&
                       !project.IsDeleted;
            })
            .WithErrorCode(ErrorCodes.Project.ProjectInvalid);
    }

    private void ValidateName()
    {
        RuleFor(x => x)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Project.ProjectNameInvalid)
            .NotNull()
            .WithErrorCode(ErrorCodes.Project.ProjectNameInvalid)
            .MustAsync(async (project, cancellationToken) =>
            {
                var projects = await _organizationService.GetAllProjectsAsync(_currentUser.OrganizationId, cancellationToken);

                var existingProject = projects.SingleOrDefault(p => p.Name.Equals(project.Name, StringComparison.OrdinalIgnoreCase) && !p.IsDeleted);
                if (existingProject != null)
                {
                    return existingProject.ProjectId == project.ProjectId;
                }

                return true;
            })
            .WithErrorCode(ErrorCodes.Project.ProjectNameInvalid);
    }
}