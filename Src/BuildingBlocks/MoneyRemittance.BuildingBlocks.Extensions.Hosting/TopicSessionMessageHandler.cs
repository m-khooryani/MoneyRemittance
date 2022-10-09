using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MoneyRemittance.BuildingBlocks.Extensions.Hosting;

public abstract class TopicSessionMessageHandler<T> : SessionMessageHandler<T>
    where T : MessageBase
{
    private readonly string _topicName;
    private readonly string _subscriptionName;
    private readonly ILogger<TopicSessionMessageHandler<T>> _logger;

    public TopicSessionMessageHandler(
        ILogger<TopicSessionMessageHandler<T>> logger,
        IConfiguration configuration,
        IJsonMessageResolver jsonResolver,
        string topicName,
        string subscriptionName)
        : base(logger, configuration, jsonResolver)
    {
        _logger = logger;
        _topicName = topicName;
        _subscriptionName = subscriptionName;
    }

    protected override ServiceBusSessionProcessor GetServiceBusSessionProcessor(
        ServiceBusClient serviceBusClient)
    {
        _logger.LogDebug($"Creating Sessison Processor, TopicName: {_topicName}, SubscriptionName: {_subscriptionName}");
        return serviceBusClient.CreateSessionProcessor(_topicName, _subscriptionName);
    }
}
