using Microsoft.Azure.ServiceBus;

namespace MoneyRemittance.BuildingBlocks.AzureServiceBus;

public interface ITopicClientFactory
{
    ITopicClient CreateClient(string topic);
}
