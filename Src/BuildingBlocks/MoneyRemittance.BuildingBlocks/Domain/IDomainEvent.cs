namespace MoneyRemittance.BuildingBlocks.Domain;

public interface IDomainEvent
{
    DateTime OccurredAt { get; }
    string AggregateId { get; }
}
