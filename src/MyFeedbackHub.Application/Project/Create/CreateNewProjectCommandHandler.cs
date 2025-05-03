using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Project.Create;

public sealed record CreateNewProjectCommand(
    string Name,
    string Url,
    string Description);

public sealed class CreateNewProjectCommandHandler(
    IFeedbackHubDbContext dbContext,
    IUserContext currentUser)
    : ICommandHandler<CreateNewProjectCommand>
{
    public async Task<ServiceResult> HandleAsync(CreateNewProjectCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(command.Name))
        {
            return ServiceResult.WithError(ErrorCodes.Project.ProjectNameInvalid);
        }

        var project = ProjectDomain.Create(
            command.Name,
            currentUser.OrganizationId,
            DateTimeOffset.UtcNow,
            currentUser.UserId);

        await dbContext.Projects.AddAsync(project, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
