using MoneyRemittance.BuildingBlocks.Domain;

namespace MoneyRemittance.BuildingBlocks.Infrastructure.DomainEvents;

internal interface IDomainEventsAccessor
{
    IDomainEvent[] GetAll();
    void ClearAllDomainEvents();
}
