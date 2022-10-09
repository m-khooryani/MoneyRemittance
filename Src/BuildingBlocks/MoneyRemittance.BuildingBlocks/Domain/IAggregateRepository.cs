namespace MoneyRemittance.BuildingBlocks.Domain;

public interface IAggregateRepository
{
    void Add<TAggregateRoot, TKey>(TAggregateRoot aggregateRoot)
        where TAggregateRoot : AggregateRoot<TKey>
        where TKey : TypedId;
    Task<TAggregateRoot> LoadAsync<TAggregateRoot, TKey>(TKey aggregateId)
        where TAggregateRoot : AggregateRoot<TKey>
        where TKey : TypedId;
    AggregateRoot[] Load();
}
