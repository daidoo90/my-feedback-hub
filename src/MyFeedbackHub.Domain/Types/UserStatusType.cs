namespace MyFeedbackHub.Domain.Types;

public enum UserStatusType
{
    New = 1,
    PendingInvitation = 2,
    InvitationExpired = 3,
    Active = 4,
    Inactive = 5
}
