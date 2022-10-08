using MoneyRemittance.BuildingBlocks.Application.Contracts;

namespace MoneyRemittance.BuildingBlocks.Infrastructure.Configuration.Processing.Outbox;

public record ProcessOutboxCommand : Command
{
    public ProcessOutboxCommand(string outboxMessageId)
    {
        MessageId = outboxMessageId;
    }

    public string MessageId { get; }
}
