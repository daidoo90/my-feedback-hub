namespace MyFeedbackHub.Domain;

public class BoardDomain
{
    public Guid BoardId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public bool IsPrivate { get; set; }

    public Guid BusinessId { get; set; }

    public BusinessDomain Business { get; set; } = null!;
}
