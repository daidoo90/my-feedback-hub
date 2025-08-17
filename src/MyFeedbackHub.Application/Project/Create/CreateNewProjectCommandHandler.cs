using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Project;

public sealed record CreateNewProjectCommand(
    string Name,
    string Url,
    string Description);

public sealed class CreateNewProjectCommandHandler(
    IUnitOfWork unitOfWork,
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

        var organization = await unitOfWork
            .DbContext
            .Organizations
            .Include(o => o.Projects)
            .SingleAsync(x => x.OrganizationId == currentUser.OrganizationId, cancellationToken: cancellationToken);

        var newProject = ProjectDomain.Create(
            command.Name,
            organization.OrganizationId,
            command.Url,
            command.Description,
            DateTimeOffset.UtcNow,
            currentUser.UserId);

        organization.AddProject(newProject);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
