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
    IUserContext userContext)
    : ICommandHandler<CreateNewProjectCommand>
{
    public async Task<ServiceResult> HandleAsync(CreateNewProjectCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(command.Name))
        {
            return ServiceResult.WithError(ErrorCodes.Project.ProjectNameInvalid);
        }

        var project = new ProjectDomain
        {
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedByUserId = userContext.UserId,
            IsDeleted = false,
            Name = command.Name,
            Url = command.Url,
            Description = command.Description,
            OrganizationId = userContext.OrganizationId
        };

        await dbContext.Projects.AddAsync(project, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
