using MoneyRemittance.BuildingBlocks.Domain;

namespace MoneyRemittance.BuildingBlocks.Infrastructure.DomainEvents;

internal class DomainEventsAccessor : IDomainEventsAccessor
{
    private readonly IAggregateRepository _aggregateRepository;

    public DomainEventsAccessor(IAggregateRepository aggregateRepository)
    {
        _aggregateRepository = aggregateRepository;
    }

    public IDomainEvent[] GetAll()
    {
        return _aggregateRepository
            .Load()
            .Where(x => x.DomainEvents != null && x.DomainEvents.Any())
            .SelectMany(x => x.DomainEvents)
            .ToArray();
    }

    public void ClearAllDomainEvents()
    {
        var domainEntities = _aggregateRepository
            .Load()
            .Where(x => x.DomainEvents != null && x.DomainEvents.Any())
            .ToList();

        domainEntities
            .ForEach(entity => entity.ClearDomainEvents());
    }
}
