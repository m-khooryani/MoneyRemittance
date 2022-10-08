namespace MoneyRemittance.BuildingBlocks.Infrastructure.DomainEvents;

public interface IDomainEventsDispatcher
{
    Task<IEnumerable<OutboxMessageRefrences>> DispatchAsync();
}
