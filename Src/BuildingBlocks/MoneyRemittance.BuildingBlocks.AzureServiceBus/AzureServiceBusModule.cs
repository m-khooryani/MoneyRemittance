using Autofac;
using MoneyRemittance.BuildingBlocks.Application.Outbox;
using MoneyRemittance.BuildingBlocks.Infrastructure.EventBus;

namespace MoneyRemittance.BuildingBlocks.AzureServiceBus;

public class AzureServiceBusModule : Module
{
    /// <summary>
    /// Will be initialized with default factory if is set to null
    /// </summary>
    public ITopicClientFactory? TopicClientFactory { get; init; }
    /// <summary>
    /// Will be initialized with default factory if is set to null
    /// </summary>
    public IQueueClientFactory? QueueClientFactory { get; init; }
    public string OutboxQueueName { get; init; }
    public string ConnectionString { get; init; }

    protected override void Load(ContainerBuilder builder)
    {
        if (string.IsNullOrWhiteSpace(ConnectionString))
        {
            throw new Exception("AzureServiceBus connection string is empty.");
        }
        if (string.IsNullOrWhiteSpace(OutboxQueueName))
        {
            throw new Exception("Outbox queue name is empty.");
        }
        var queueNameProvider = new OutboxConfig
        {
            Name = OutboxQueueName,
        };
        builder
            .RegisterInstance(queueNameProvider)
            .AsSelf()
            .SingleInstance();

        var connectionStringProvider = new ServiceBusConfig()
        {
            ConnectionString = ConnectionString,
        };
        builder
            .RegisterInstance(connectionStringProvider)
            .AsSelf()
            .SingleInstance();

        if (QueueClientFactory is not null)
        {
            builder
                .RegisterInstance(QueueClientFactory)
                .As<IQueueClientFactory>()
                .SingleInstance();
        }
        else
        {
            builder
                .RegisterType(typeof(QueueClientFactory))
                .As<IQueueClientFactory>()
                .SingleInstance();
        }

        if (TopicClientFactory is not null)
        {
            builder
                .RegisterInstance(TopicClientFactory)
                .As<ITopicClientFactory>()
                .SingleInstance();
        }
        else
        {
            builder
                .RegisterType(typeof(TopicClientFactory))
                .As<ITopicClientFactory>()
                .SingleInstance();
        }

        builder.RegisterType(typeof(AzureServiceBusEventBus))
            .As(typeof(IEventBus))
            .SingleInstance();
    }
}