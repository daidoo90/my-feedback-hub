namespace MyFeedbackHub.Domain;

public class UserDomain
{
    public Guid UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Username { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public string Password { get; set; } = string.Empty;

    public string Salt { get; set; } = string.Empty;

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset UpdatedOn { get; set; }

    public Guid BusinessId { get; set; }

    public bool IsActive { get; set; }

    public BusinessDomain Business { get; set; } = null!;
}
