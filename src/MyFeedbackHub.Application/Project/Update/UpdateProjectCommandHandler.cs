using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Feedback;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Project;

public sealed record UpdateProjectCommand(
    Guid ProjectId,
    string? Name,
    string? Url,
    string? Description);

public sealed class UpdateProjectCommandHandler(
    IFeedbackHubDbContextFactory dbContextFactory,
    IUserContext currentUser,
    IAuthorizationService authorizationService,
    IValidator<UpdateProjectCommand> validator)
    : ICommandHandler<UpdateProjectCommand>
{
    public async Task<ServiceResult> HandleAsync(UpdateProjectCommand command, CancellationToken cancellationToken = default)
    {
        if (!await authorizationService.CanAccessProjectAsync(command.ProjectId, cancellationToken))
        {
            return ServiceDataResult<GetFeedbackByIdResponse>.WithError(ErrorCodes.Feedback.NotFound);
        }

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ServiceResult.WithError(validationResult.Errors.First().ErrorCode);
        }

        var dbContext = await dbContextFactory.CreateAsync(cancellationToken);
        var project = await dbContext
            .Projects
            .SingleOrDefaultAsync(p => p.ProjectId == command.ProjectId
                                        && p.OrganizationId == currentUser.OrganizationId,
            cancellationToken);

        project.Update(
            command.Name,
            command.Url,
            command.Description,
            DateTimeOffset.UtcNow,
            currentUser.UserId);

        await dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
