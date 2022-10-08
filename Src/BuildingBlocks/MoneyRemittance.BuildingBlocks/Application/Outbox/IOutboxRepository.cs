namespace MoneyRemittance.BuildingBlocks.Application.Outbox;

public interface IOutboxRepository
{
    Task AddAsync(OutboxMessage message);
    Task<OutboxMessage> LoadAsync(Guid id, CancellationToken cancellationToken = default);
    void Remove(OutboxMessage outboxMessage);
}
