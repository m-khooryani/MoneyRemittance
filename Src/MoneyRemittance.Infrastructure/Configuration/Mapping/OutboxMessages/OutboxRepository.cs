using Microsoft.EntityFrameworkCore;
using MoneyRemittance.BuildingBlocks.Application.Outbox;
using MoneyRemittance.BuildingBlocks.UnitOfWork.EFCore;

namespace MoneyRemittance.Infrastructure.Configuration.Mapping.OutboxMessages;

internal class OutboxRepository : IOutboxRepository
{
    private readonly ServiceDbContext _context;

    public OutboxRepository(ServiceDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(OutboxMessage message)
    {
        await _context.OutboxMessages.AddAsync(message);
    }

    public async Task<OutboxMessage> LoadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context
            .OutboxMessages
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public void Remove(OutboxMessage outboxMessage)
    {
        _context.OutboxMessages.Remove(outboxMessage);
    }
}
