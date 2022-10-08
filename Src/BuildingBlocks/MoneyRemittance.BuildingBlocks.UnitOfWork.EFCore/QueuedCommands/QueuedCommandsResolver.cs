using Microsoft.EntityFrameworkCore;
using MoneyRemittance.BuildingBlocks.Application.Outbox;
using MoneyRemittance.BuildingBlocks.Application.QueuedCommands;

namespace MoneyRemittance.BuildingBlocks.UnitOfWork.EFCore.QueuedCommands;

internal class QueuedCommandsResolver : IQueuedCommandsResolver
{
    private readonly ServiceDbContext _context;

    public QueuedCommandsResolver(ServiceDbContext context)
    {
        _context = context;
    }

    public OutboxMessage[] Resolve()
    {
        return _context
            .ChangeTracker
            .Entries<OutboxMessage>()
            .Where(x => x.State == EntityState.Added)
            .Select(x => x.Entity)
            .ToArray();
    }
}
