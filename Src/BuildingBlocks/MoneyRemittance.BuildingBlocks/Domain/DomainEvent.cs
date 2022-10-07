namespace MoneyRemittance.BuildingBlocks.Domain;

public record DomainEvent : IDomainEvent
{
    public DateTime OccurredAt { get; }
    public string AggregateId { get; }

    private DomainEvent(string aggregateId)
    {
        AggregateId = aggregateId;
        OccurredAt = Clock.Now;
    }

    public DomainEvent(TypedId aggregateId)
        : this(aggregateId.ToString())
    {
    }
}
