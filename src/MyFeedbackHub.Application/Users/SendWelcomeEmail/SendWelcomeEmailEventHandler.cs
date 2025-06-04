using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Domain.Organization;
using MyFeedbackHub.Domain.Shared.Domains;

namespace MyFeedbackHub.Application.Users.SendWelcomeEmail;

public sealed class UserCreatedDomainEvent : BaseDomainEvent
{
    public UserDomain User { get; }

    public UserCreatedDomainEvent(UserDomain user) => User = user;
}

public sealed class SendWelcomeEmailEventHandler(
    IEmailService _emailService,
    IUserInvitationService _userInvitationService) : IDomainEventHandler<UserCreatedDomainEvent>
{
    public async Task HandleAsync(UserCreatedDomainEvent @event, CancellationToken cancellationToken = default)
    {
        var newUserEmail = @event.User.Username;

        var token = await _userInvitationService.GenerateAndStoreInvitationTokenAsync(newUserEmail, cancellationToken);

        return;
        //await _emailService.SendEmailAsync(newUserEmail, "Welcome", $"Your invitation token: {token}");
    }
}
