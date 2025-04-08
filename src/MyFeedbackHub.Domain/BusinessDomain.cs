namespace MyFeedbackHub.Domain;

public class BusinessDomain
{
    public Guid BusinessId { get; set; }

    public string? Name { get; set; }

    public string? Website { get; set; }

    public int TeamSize { get; set; }

    public string? Source { get; set; }

    public string? Address { get; set; }

    public string? VATNumber { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public ICollection<UserDomain> Users { get; set; } = [];

    public ICollection<BoardDomain> Boards { get; set; } = [];
}
