using Autofac;
using MoneyRemittance.BuildingBlocks.Application.AggregateHistory;
using MoneyRemittance.BuildingBlocks.Domain;
using MoneyRemittance.BuildingBlocks.Infrastructure.Configuration;
using Newtonsoft.Json;

namespace MoneyRemittance.BuildingBlocks.UnitOfWork.EFCore.UnitOfWorks;

internal class AppendingAggregateHistoryUnitOfWorkDecorator : IUnitOfWork
{
    private readonly IUnitOfWork _decorated;
    private readonly ServiceDbContext _context;

    public AppendingAggregateHistoryUnitOfWorkDecorator(
        IUnitOfWork decorated,
        ServiceDbContext context)
    {
        _decorated = decorated;
        _context = context;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        await SaveEventsAsync(cancellationToken);
        return await _decorated.CommitAsync(cancellationToken);
    }

    private async Task SaveEventsAsync(CancellationToken cancellationToken)
    {
        var aggregates = _context
            .ChangeTracker
            .Entries<AggregateRoot>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
            .ToList();
        if (!aggregates.Any())
        {
            return;
        }
        using var scope = CompositionRoot.BeginLifetimeScope();
        foreach (var aggregate in aggregates)
        {
            var domainEvents = aggregate
                .Entity
                .DomainEvents;
            var type = aggregate.Entity.GetType().FullName;
            int index = 0;
            foreach (var domainEvent in domainEvents)
            {
                var historyItem = AggregateRootHistoryItem.Create(
                    domainEvent.AggregateId,
                    aggregate.Entity.Version + index,
                    domainEvent.GetType().FullName,
                    domainEvent.OccurredAt,
                    type,
                    JsonConvert.SerializeObject(domainEvent));
                await _context.AggregateRootHistory
                    .AddAsync(historyItem, cancellationToken);
                index++;
            }
        }
    }
}
