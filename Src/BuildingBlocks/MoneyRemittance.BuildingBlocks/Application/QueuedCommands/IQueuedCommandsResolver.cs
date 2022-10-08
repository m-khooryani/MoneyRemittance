using MoneyRemittance.BuildingBlocks.Application.Outbox;

namespace MoneyRemittance.BuildingBlocks.Application.QueuedCommands;

public interface IQueuedCommandsResolver
{
    OutboxMessage[] Resolve();
}