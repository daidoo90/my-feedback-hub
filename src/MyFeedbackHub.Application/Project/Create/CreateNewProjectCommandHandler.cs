using FluentValidation;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Project.Create;

public sealed record CreateNewProjectCommand(
    string Name,
    string Url,
    string Description);

public sealed class CreateNewProjectCommandHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    IUserContext currentUser,
    IValidator<CreateNewProjectCommand> validator)
    : ICommandHandler<CreateNewProjectCommand>
{
    public async Task<ServiceResult> HandleAsync(CreateNewProjectCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ServiceResult.WithError(validationResult.Errors.First().ErrorCode);
        }

        var project = ProjectDomain.Create(
            command.Name,
            currentUser.OrganizationId,
            command.Url,
            command.Description,
            DateTimeOffset.UtcNow,
            currentUser.UserId);

        var dbContext = await dbContextFactory.CreateAsync(cancellationToken);
        await dbContext.Projects.AddAsync(project, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
