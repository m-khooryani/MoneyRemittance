using Autofac;
using Microsoft.Extensions.Logging;
using MoneyRemittance.BuildingBlocks.Application.Outbox;
using MoneyRemittance.BuildingBlocks.Domain;
using MoneyRemittance.BuildingBlocks.Infrastructure.Configuration;
using MoneyRemittance.BuildingBlocks.Infrastructure.DomainEvents;
using MoneyRemittance.BuildingBlocks.Infrastructure.EventBus;

namespace MoneyRemittance.BuildingBlocks.UnitOfWork.EFCore.UnitOfWorks;

internal class PublishingOutboxMessagesUnitOfWorkDecorator : IUnitOfWork
{
    private readonly IUnitOfWork _decorated;
    private readonly IDomainEventsDispatcher _domainEventsDispatcher;
    private readonly ILogger _logger;

    public PublishingOutboxMessagesUnitOfWorkDecorator(
        IUnitOfWork decorated,
        IDomainEventsDispatcher domainEventsDispatcher,
        ILogger logger)
    {
        _decorated = decorated;
        _domainEventsDispatcher = domainEventsDispatcher;
        _logger = logger;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        var outboxMessages = await _domainEventsDispatcher.DispatchAsync();

        var changes = await _decorated.CommitAsync(cancellationToken);

        PublishOutboxMessages(outboxMessages);

        return changes;
    }

    private void PublishOutboxMessages(IEnumerable<OutboxMessageRefrences> outboxMessages)
    {
        if (!outboxMessages.Any())
        {
            _logger.LogInformation("No OutboxMessage to publish, skipping...");
            return;
        }
        _logger.LogInformation($"sending message to outbox queue({outboxMessages.Count()})...");
        using var scope = CompositionRoot.BeginLifetimeScope();
        var messagePublisher = scope.Resolve<IEventBus>();
        var outboxConfig = scope.Resolve<OutboxConfig>();
        outboxMessages
            .ToList()
            .ForEach(x => messagePublisher.Publish(
                outboxConfig.Name,
                x.AggregateId,
                x));
        _logger.LogInformation("message are just sent to outbox queue");
    }
}
