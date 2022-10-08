namespace MoneyRemittance.BuildingBlocks.Infrastructure.EventBus;

public interface IEventBus : IDisposable
{
    Task Publish<T>(T @event)
        where T : IntegrationEvent;
    Task Publish(string queue, string sessionId, string raw);
    Task Publish<T>(string queue, string sessionId, T obj);
}
