using MoneyRemittance.BuildingBlocks.Application.Outbox;
using MoneyRemittance.BuildingBlocks.Application.QueuedCommands;

namespace MoneyRemittance.BuildingBlocks.UnitOfWork.EFCore.QueuedCommands;

internal class QueuedCommandsResolverCachingDecorator : IQueuedCommandsResolver
{
    private readonly IQueuedCommandsResolver _decorated;
    private OutboxMessage[] _queuedCommands;

    public QueuedCommandsResolverCachingDecorator(IQueuedCommandsResolver decorated)
    {
        _decorated = decorated;
    }

    public OutboxMessage[] Resolve()
    {
        if (_queuedCommands is not null)
        {
            return _queuedCommands;
        }
        _queuedCommands = _decorated.Resolve();
        return _queuedCommands;
    }
}
