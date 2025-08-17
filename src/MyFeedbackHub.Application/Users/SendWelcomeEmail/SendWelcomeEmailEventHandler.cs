using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.Domain.Shared.Domains;

namespace MyFeedbackHub.Application.Users.SendWelcomeEmail;

public sealed class UserCreatedEvent : BaseDomainEvent
{
    public UserDomain User { get; }

    public UserCreatedEvent(UserDomain user) => User = user;
}

public sealed class SendWelcomeEmailEventHandler(
    IEmailService _emailService,
    IUserInvitationService _userInvitationService) : IDomainEventHandler<UserCreatedEvent>
{
    public async Task HandleAsync(UserCreatedEvent @event, CancellationToken cancellationToken = default)
    {
        var newUserEmail = @event.User.Username;

        var token = await _userInvitationService.GenerateAndStoreInvitationTokenAsync(newUserEmail, cancellationToken);

        return;
        //await _emailService.SendEmailAsync(newUserEmail, "Welcome", $"Your invitation token: {token}");
    }
}
