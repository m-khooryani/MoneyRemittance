using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MoneyRemittance.BuildingBlocks.Extensions.Hosting;

public abstract class QueueSessionMessageHandler<T> : SessionMessageHandler<T>
    where T : MessageBase
{
    private readonly ILogger<QueueSessionMessageHandler<T>> _logger;
    private readonly string _queueName;

    public QueueSessionMessageHandler(
        ILogger<QueueSessionMessageHandler<T>> logger,
        IConfiguration configuration,
        IJsonMessageResolver jsonResolver,
        string queueName)
        : base(logger, configuration, jsonResolver)
    {
        _logger = logger;
        _queueName = queueName;
    }

    protected override ServiceBusSessionProcessor GetServiceBusSessionProcessor(
        ServiceBusClient serviceBusClient)
    {
        _logger.LogDebug($"Creating Sessison Processor, QueueName: {_queueName}");
        return serviceBusClient.CreateSessionProcessor(_queueName);
    }
}
