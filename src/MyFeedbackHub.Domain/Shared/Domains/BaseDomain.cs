namespace MyFeedbackHub.Domain.Shared.Domains;

public abstract class BaseDomain
{
    private readonly List<BaseDomainEvent> _events = [];

    public IReadOnlyCollection<BaseDomainEvent> DomainEvents => _events.AsReadOnly();

    public void AddDomainEvent(BaseDomainEvent domainEvent)
    {
        _events.Add(domainEvent);
    }

    public void ClearDomainEvents() => _events.Clear();
}
