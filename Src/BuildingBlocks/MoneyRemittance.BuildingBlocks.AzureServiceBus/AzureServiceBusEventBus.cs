using System.Text;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using MoneyRemittance.BuildingBlocks.Infrastructure.EventBus;
using Newtonsoft.Json;

namespace MoneyRemittance.BuildingBlocks.AzureServiceBus;

internal class AzureServiceBusEventBus : IEventBus
{
    private readonly IQueueClientFactory _queueClientFactory;
    private readonly ITopicClientFactory _topicClientFactory;
    private readonly ILogger _logger;

    public AzureServiceBusEventBus(
        IQueueClientFactory queueClientFactory,
        ITopicClientFactory topicClientFactory,
        ILogger logger)
    {
        _queueClientFactory = queueClientFactory;
        _topicClientFactory = topicClientFactory;
        _logger = logger;
    }

    public void Dispose()
    {
    }

    public async Task Publish<T>(T @event) where T : IntegrationEvent
    {
        var eventType = @event.GetType();
        _logger.LogInformation($"Publishing {eventType.FullName}...");

        var json = JsonConvert.SerializeObject(@event, Formatting.Indented);
        var messageBody = Encoding.UTF8.GetBytes(json);
        var message = new Message(messageBody)
        {
            MessageId = Guid.NewGuid().ToString(),
            SessionId = @event.AggregateId.ToString()
        };
        _logger.LogInformation("Body: " + json);
        _logger.LogInformation("MessageId: " + message.MessageId);
        _logger.LogInformation("SessionId: " + message.SessionId);

        var client = _topicClientFactory.CreateClient(@event.IntegrationEventName);
        await client.SendAsync(message);
    }

    public Task Publish(string queue, string sessionId, string raw)
    {
        _logger.LogInformation("Publishing message to queue");
        _logger.LogInformation($"Queue: {queue}");
        _logger.LogInformation($"SessionId: {sessionId}");
        _logger.LogInformation($"Data: {raw}");
        var message = new Message(Encoding.UTF8.GetBytes(raw))
        {
            SessionId = sessionId
        };
        return _queueClientFactory
            .CreateClient(queue)
            .SendAsync(message);
    }

    public Task Publish<T>(string queue, string sessionId, T obj)
    {
        return Publish(queue, sessionId, JsonConvert.SerializeObject(obj, Formatting.Indented));
    }
}
