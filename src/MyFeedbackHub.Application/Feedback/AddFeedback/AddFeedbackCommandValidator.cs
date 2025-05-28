using FluentValidation;
using MyFeedbackHub.Application.Organization;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users;
using MyFeedbackHub.Domain.Types;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Feedback;

public sealed class AddFeedbackCommandValidator : AbstractValidator<CreateNewFeedbackCommand>
{
    private readonly IUserContext _currentUser;
    private readonly IOrganizationService _organizationService;
    private readonly IUserService _userService;

    public AddFeedbackCommandValidator(
        IUserContext userContext,
        IOrganizationService organizationService,
        IUserService userService)
    {
        _currentUser = userContext;
        _organizationService = organizationService;
        _userService = userService;

        ValidateTitle();
        ValidateProject();
    }

    private void ValidateTitle()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Feedback.TitleInvalid)
            .NotNull()
            .WithErrorCode(ErrorCodes.Feedback.TitleInvalid);
    }

    private void ValidateProject()
    {
        RuleFor(x => x.ProjectId)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorCodes.Project.ProjectInvalid)
            .MustAsync(async (projectId, cancellationToken) =>
            {
                if (_currentUser.Role == UserRoleType.OrganizationAdmin)
                {
                    var organizationProjects = await _organizationService.GetProjectsAsync(_currentUser.OrganizationId, cancellationToken);
                    return organizationProjects.Contains(projectId);
                }
                else
                {
                    IEnumerable<Guid> projects = await _userService.GetProjectIdsAsync(_currentUser.UserId, cancellationToken);
                    return projects.Contains(projectId);
                }
            })
            .WithErrorCode(ErrorCodes.Project.ProjectInvalid);
    }
}
