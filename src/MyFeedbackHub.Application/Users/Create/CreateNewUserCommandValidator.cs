using FluentValidation;
using MyFeedbackHub.Application.Organization;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users;

public sealed class CreateNewUserCommandValidator : AbstractValidator<CreateNewUserCommand>
{
    private readonly IOrganizationService _organizationService;
    private readonly IUserContext _currentUser;
    private readonly IUserService _userService;

    public CreateNewUserCommandValidator(
        IOrganizationService organizationService,
        IUserContext currentUser,
        IUserService userService)
    {
        _organizationService = organizationService;
        _currentUser = currentUser;
        _userService = userService;

        ValidateUsername();
        ValidateProject();
    }

    private void ValidateProject()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Project.ProjectInvalid)
            .NotNull()
            .WithErrorCode(ErrorCodes.Project.ProjectInvalid)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorCodes.Project.ProjectInvalid)
            .MustAsync(async (projectId, cancellationToken) =>
            {
                var projects = await _organizationService.GetAllProjectsAsync(_currentUser.OrganizationId, cancellationToken);

                return projects.Any(p => p.ProjectId == projectId &&
                                         !p.IsDeleted);
            })
            .WithErrorCode(ErrorCodes.Project.ProjectInvalid);
    }

    private void ValidateUsername()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.User.UsernameInvalid)
            .NotNull()
            .WithErrorCode(ErrorCodes.User.UsernameInvalid)
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
            .WithErrorCode(ErrorCodes.User.UsernameInvalid)
            .MustAsync(async (username, cancellationToken) =>
            {
                var existingUser = await _userService.GetByUsernameAsync(username, cancellationToken);
                return existingUser == null;

            })
            .WithErrorCode(ErrorCodes.User.UsernameInvalid);
    }
}