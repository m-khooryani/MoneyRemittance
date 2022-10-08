using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoneyRemittance.BuildingBlocks.Application.AggregateHistory;
using MoneyRemittance.BuildingBlocks.Application.Exceptions;
using MoneyRemittance.BuildingBlocks.Domain;
using MoneyRemittance.BuildingBlocks.Infrastructure.CleanArchitecture;
using Newtonsoft.Json;

namespace MoneyRemittance.BuildingBlocks.UnitOfWork.EFCore;

internal class AggregateRepository : IAggregateRepository
{
    private readonly ServiceDbContext _context;
    private readonly CleanArchitectureLayers _cleanArchitectureLayers;
    private readonly ILogger _logger;
    private readonly Dictionary<TypedId, AggregateRoot> _loadedAggregates;
    private const int ChunkSize = 100;

    public AggregateRepository(
        ServiceDbContext context,
        CleanArchitectureLayers cleanArchitectureLayers,
        ILogger logger)
    {
        _loadedAggregates = new();
        _context = context;
        _cleanArchitectureLayers = cleanArchitectureLayers;
        _logger = logger;
    }

    public async Task AddAsync<TAggregateRoot, TKey>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : AggregateRoot<TKey>
        where TKey : TypedId
    {
        await _context.AddAsync(aggregateRoot);

        _loadedAggregates.Add(aggregateRoot.Id, aggregateRoot);
    }

    public async Task<TAggregateRoot> LoadAsync<TAggregateRoot, TId>(
        TId aggregateId)
        where TAggregateRoot : AggregateRoot<TId>
        where TId : TypedId
    {
        if (_loadedAggregates.ContainsKey(aggregateId))
        {
            return _loadedAggregates[aggregateId] as TAggregateRoot;
        }
        var aggregateRoot = await Load<TAggregateRoot, TId>(aggregateId);

        _loadedAggregates.Add(aggregateId, aggregateRoot);
        _context.Attach(aggregateRoot);
        return aggregateRoot;
    }

    private async Task<TAggregateRoot> Load<TAggregateRoot, TId>(TId aggregateId)
        where TAggregateRoot : AggregateRoot<TId>
        where TId : TypedId
    {
        var historyChunks = AsyncChunk(_context.AggregateRootHistory.Where(x => x.AggregateId == aggregateId.ToString()).OrderBy(x => x.Datetime), ChunkSize);

        var history = new List<AggregateRootHistoryItem>();
        await foreach (var historyChunk in historyChunks)
        {
            history.AddRange(historyChunk);
        }

        if (!history.Any())
        {
            throw new EntityNotFoundException($"Entity not found with Id: {aggregateId}");
        }

        _logger.LogInformation("Restoring aggregate state by applying {eventsCount}", history.Count);

        var aggregateType = history.First().Type;
        var type = _cleanArchitectureLayers.DomainLayer.GetType(aggregateType);
        var aggregate = (TAggregateRoot)Activator.CreateInstance(type, true);

        var domainEvents = new Queue<IDomainEvent>();
        foreach (var historyItem in history)
        {
            _logger.LogDebug("Applying {eventName}...", historyItem.EventType);
            var domainEventType = _cleanArchitectureLayers.DomainLayer.GetType(historyItem.EventType);
            var domainEvent = JsonConvert.DeserializeObject(historyItem.Data, domainEventType);

            domainEvents.Enqueue(domainEvent as IDomainEvent);
        }
        aggregate.Load(domainEvents);

        return aggregate;
    }

    private static async IAsyncEnumerable<IEnumerable<TSource>> AsyncChunk<TSource>(IOrderedQueryable<TSource> source, int chunkSize)
    {
        for (int i = 0; i < source.Count(); i += chunkSize)
        {
            yield return await source
                .Skip(i)
                .Take(chunkSize)
                .ToArrayAsync();
        }
    }

    AggregateRoot[] IAggregateRepository.Load()
    {
        return _loadedAggregates.Values.ToArray();
    }
}
