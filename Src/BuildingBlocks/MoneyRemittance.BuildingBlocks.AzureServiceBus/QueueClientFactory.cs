using Microsoft.Azure.ServiceBus;
using System.Collections.Concurrent;

namespace MoneyRemittance.BuildingBlocks.AzureServiceBus;

internal class QueueClientFactory : IQueueClientFactory
{
    private readonly ServiceBusConfig _serviceBusConfig;
    private readonly ConcurrentDictionary<string, QueueClient> _cache
        = new();

    public QueueClientFactory(ServiceBusConfig serviceBusConfig)
    {
        _serviceBusConfig = serviceBusConfig;
    }

    public IQueueClient CreateClient(string queueName)
    {
        return _cache.GetOrAdd(queueName, new QueueClient(_serviceBusConfig.ConnectionString, queueName));
    }
}
