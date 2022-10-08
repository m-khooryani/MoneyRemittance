using System.Collections.Concurrent;
using Microsoft.Azure.ServiceBus;

namespace MoneyRemittance.BuildingBlocks.AzureServiceBus;

internal class TopicClientFactory : ITopicClientFactory
{
    private readonly ServiceBusConfig _serviceBusConfig;

    public TopicClientFactory(ServiceBusConfig serviceBusConfig)
    {
        _serviceBusConfig = serviceBusConfig;
    }

    private readonly ConcurrentDictionary<string, ITopicClient> _clients
        = new();

    public ITopicClient CreateClient(string topic) =>
        _clients.GetOrAdd(topic, t =>
            new TopicClient(
                _serviceBusConfig.ConnectionString, t));
}
