namespace MoneyRemittance.BuildingBlocks.Domain;

public class AggregateRoot<TKey> : AggregateRoot
    where TKey : TypedId
{
    public TKey Id { get; protected set; }
}

public class AggregateRoot : Entity
{
    private readonly Queue<IDomainEvent> _domainEvents;
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.ToList().AsReadOnly();
    public int Version { get; private set; }

    protected AggregateRoot()
    {
        _domainEvents = new Queue<IDomainEvent>();
    }

    private void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Enqueue(domainEvent);
    }

    internal void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }

    /// <summary>
    /// Invoke "When" method, then append the event to changes
    /// </summary>
    /// <param name="event"></param>
    protected void Apply(IDomainEvent @event)
    {
        DynamicInvoker.Invoke(this, "When", @event);
        AddDomainEvent(@event);
    }

    internal void Load(IDomainEvent @event)
    {
        Apply(@event);
    }

    public void Load(IEnumerable<IDomainEvent> events)
    {
        foreach (var domainEvent in events)
        {
            var latestVersion = domainEvent;

            DynamicInvoker.Invoke(this, "When", latestVersion);
            Version++;
        }
        ClearDomainEvents();
    }
}
