using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Project.Update;

public sealed record UpdateProjectCommand(
    Guid ProjectId,
    string? Name,
    string? Url,
    string? Description);

public sealed class UpdateProjectCommandHandler(
    IFeedbackHubDbContext dbContext,
    IUserContext userContext)
    : ICommandHandler<UpdateProjectCommand>
{
    public async Task<ServiceResult> HandleAsync(UpdateProjectCommand command, CancellationToken cancellationToken = default)
    {
        var project = await dbContext
            .Projects
            .SingleOrDefaultAsync(p => p.ProjectId == command.ProjectId
                                        && p.OrganizationId == userContext.OrganizationId,
            cancellationToken);

        if (project == null)
        {
            return ServiceResult.WithError(ErrorCodes.Project.ProjectInvalid);
        }

        project.Name = !string.IsNullOrEmpty(command.Name) ? command.Name : project.Name;
        project.Url = !string.IsNullOrEmpty(command.Url) ? command.Url : project.Url;
        project.Description = !string.IsNullOrEmpty(command.Description) ? command.Description : project.Description;
        project.UpdatedOn = DateTimeOffset.UtcNow;
        project.UpdatedOnByUserId = userContext.UserId;

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
