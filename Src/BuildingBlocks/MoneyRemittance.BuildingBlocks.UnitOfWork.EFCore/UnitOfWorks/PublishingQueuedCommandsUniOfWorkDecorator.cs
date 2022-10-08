using Autofac;
using Microsoft.Extensions.Logging;
using MoneyRemittance.BuildingBlocks.Application.Outbox;
using MoneyRemittance.BuildingBlocks.Application.QueuedCommands;
using MoneyRemittance.BuildingBlocks.Domain;
using MoneyRemittance.BuildingBlocks.Infrastructure.Configuration;
using MoneyRemittance.BuildingBlocks.Infrastructure.EventBus;

namespace MoneyRemittance.BuildingBlocks.UnitOfWork.EFCore.UnitOfWorks;

internal class PublishingQueuedCommandsUnitOfWorkDecorator : IUnitOfWork
{
    private readonly IUnitOfWork _decorated;
    private readonly IQueuedCommandsResolver _queuedCommandsResolver;
    private readonly ILogger _logger;

    public PublishingQueuedCommandsUnitOfWorkDecorator(
        IUnitOfWork decorated,
        IQueuedCommandsResolver queuedCommandsResolver,
        ILogger logger)
    {
        _decorated = decorated;
        _queuedCommandsResolver = queuedCommandsResolver;
        _logger = logger;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        var commands = _queuedCommandsResolver.Resolve();

        var changes = await _decorated.CommitAsync(cancellationToken);

        PublishQueuedCommands(commands);

        return changes;
    }

    private void PublishQueuedCommands(IEnumerable<OutboxMessage> queuedCommands)
    {
        if (!queuedCommands.Any())
        {
            _logger.LogInformation("No QueuedCommand to publish, skipping...");
            return;
        }
        using var scope = CompositionRoot.BeginLifetimeScope();
        var messagePublisher = scope.Resolve<IEventBus>();
        var outboxConfig = scope.Resolve<OutboxConfig>();
        _logger.LogInformation($"sending message to queuedCommands queue({queuedCommands.Count()})...");
        queuedCommands
            .ToList()
            .ForEach(x => messagePublisher.Publish(
                outboxConfig.Name,
                "queuedCommandsSession",
                x));
        _logger.LogInformation("message are just sent to queuedCommands queue");
    }
}
