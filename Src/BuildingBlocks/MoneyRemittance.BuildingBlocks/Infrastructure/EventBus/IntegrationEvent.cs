using MoneyRemittance.BuildingBlocks.Domain;

namespace MoneyRemittance.BuildingBlocks.Infrastructure.EventBus;

public record IntegrationEvent
{
    public TypedId AggregateId { get; }
    public Guid IntegrationEventId { get; }
    public DateTime OccurredOn { get; }
    public string IntegrationEventName { get; }

    protected IntegrationEvent(
        TypedId aggreateId,
        string integrationEventName)
    {
        AggregateId = aggreateId;
        IntegrationEventId = Guid.NewGuid();
        OccurredOn = Clock.Now;
        IntegrationEventName = integrationEventName;
    }
}
