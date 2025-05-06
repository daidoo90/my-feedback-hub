using MyFeedbackHub.Application.Organization.Services;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users.Services;
using MyFeedbackHub.Domain.Feedback;
using MyFeedbackHub.Domain.Types;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Feedback.Create;

public sealed record CreateNewFeedbackCommand(
    string Title,
    string Description,
    FeedbackType Type,
    Guid ProjectId);

public sealed class CreateNewFeedbackCommandHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    IUserContext currentUser,
    IUserService userService,
    IOrganizationService organizationService)
    : ICommandHandler<CreateNewFeedbackCommand>
{
    public async Task<ServiceResult> HandleAsync(CreateNewFeedbackCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(command.Title))
        {
            return ServiceResult.WithError(ErrorCodes.Feedback.TitleInvalid);
        }

        if (string.IsNullOrEmpty(command.Description))
        {
            return ServiceResult.WithError(ErrorCodes.Feedback.DescriptionInvalid);
        }

        var projects = await userService.GetProjectIdsAsync(currentUser.UserId, cancellationToken);
        if (!projects.Contains(command.ProjectId))
        {
            return ServiceResult.WithError(ErrorCodes.Project.ProjectInvalid);
        }

        if (currentUser.Role == UserRoleType.OrganizationAdmin)
        {
            var organizationProjects = await organizationService.GetProjectsAsync(currentUser.OrganizationId, cancellationToken);
            if (!organizationProjects.Contains(command.ProjectId))
            {
                return ServiceResult.WithError(ErrorCodes.Project.ProjectInvalid);
            }
        }

        var dbContext = await dbContextFactory.CreateAsync(cancellationToken);

        var newFeedback = FeedbackDomain.Create(
            command.Title,
            command.Description,
            command.Type,
            command.ProjectId,
            DateTimeOffset.UtcNow,
            currentUser.UserId);

        await dbContext.Feedbacks.AddAsync(newFeedback, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
