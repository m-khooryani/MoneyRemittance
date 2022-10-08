using MoneyRemittance.BuildingBlocks.Domain;

namespace MoneyRemittance.BuildingBlocks.Application.Events;

public class DomainNotification<T> : IDomainEventNotification<T>
    where T : IDomainEvent
{
    public T DomainEvent { get; }

    public Guid Id { get; }

    public DomainNotification(T domainEvent)
    {
        Id = Guid.NewGuid();
        DomainEvent = domainEvent;
    }
}
