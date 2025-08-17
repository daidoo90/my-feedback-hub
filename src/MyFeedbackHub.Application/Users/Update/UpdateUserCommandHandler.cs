using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Users;

public sealed record UpdateUserCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    string Username,
    string PhoneNumber);

public sealed class UpdateUserCommandHandler(
    IUnitOfWork unitOfWork,
    IUserContext currentUser,
    IValidator<UpdateUserCommand> validator)
    : ICommandHandler<UpdateUserCommand>
{
    public async Task<ServiceResult> HandleAsync(UpdateUserCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ServiceDataResult<CreateNewUserCommandResult>.WithError(validationResult.Errors.First().ErrorCode);
        }

        var user = await unitOfWork
            .DbContext
            .Users
            .SingleAsync(u => u.UserId == command.UserId, cancellationToken);

        user.Update(
            command.FirstName,
            command.LastName,
            command.PhoneNumber,
            DateTimeOffset.UtcNow,
            currentUser.UserId);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success;
    }
}
