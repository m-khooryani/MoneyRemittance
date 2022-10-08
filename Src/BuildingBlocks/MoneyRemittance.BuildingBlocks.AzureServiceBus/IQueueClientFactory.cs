using Microsoft.Azure.ServiceBus;

namespace MoneyRemittance.BuildingBlocks.AzureServiceBus;

public interface IQueueClientFactory
{
    IQueueClient CreateClient(string queueName);
}
