using Autofac;
using Autofac.Core;
using MoneyRemittance.BuildingBlocks.Application.Events;
using MoneyRemittance.BuildingBlocks.Application.Outbox;
using MoneyRemittance.BuildingBlocks.Domain;
using MoneyRemittance.BuildingBlocks.Infrastructure.Serialization;
using Newtonsoft.Json;

namespace MoneyRemittance.BuildingBlocks.Infrastructure.DomainEvents;

internal class DomainEventsDispatcher : IDomainEventsDispatcher
{
    private readonly IDomainEventsAccessor _domainEvents;
    private readonly ILifetimeScope _scope;
    private IOutboxRepository _outboxRepository;

    public DomainEventsDispatcher(
        IDomainEventsAccessor domainEventsProvider,
        ILifetimeScope scope)
    {
        _domainEvents = domainEventsProvider;
        _scope = scope;
    }

    public async Task<IEnumerable<OutboxMessageRefrences>> DispatchAsync()
    {
        var domainEvents = _domainEvents.GetAll();
        var domainEventNotifications = GenerateDomainNotificationsIfExist(domainEvents);

        var outboxIds = await SaveNotificationToOutboxAsync(domainEventNotifications);

        return outboxIds;
    }

    private async Task<OutboxMessageRefrences[]> SaveNotificationToOutboxAsync(IEnumerable<IDomainEventNotification<IDomainEvent>> domainEventNotifications)
    {
        if (!domainEventNotifications.Any())
        {
            return Array.Empty<OutboxMessageRefrences>();
        }
        _outboxRepository = _scope.Resolve<IOutboxRepository>();
        var outboxIds = new List<OutboxMessageRefrences>();
        foreach (var domainEventNotification in domainEventNotifications)
        {
            var type = domainEventNotification.GetType().FullName;
            var data = JsonConvert.SerializeObject(domainEventNotification, new JsonSerializerSettings
            {
                ContractResolver = new AllPropertiesContractResolver()
            });
            var outboxMessage = new OutboxMessage(
                domainEventNotification.DomainEvent.OccurredAt,
                type,
                data);
            outboxIds.Add(new OutboxMessageRefrences(
                outboxMessage.Id,
                domainEventNotification.DomainEvent.AggregateId,
                outboxMessage.OccurredOn,
                outboxMessage.Type));
            await _outboxRepository.AddAsync(outboxMessage);
        }

        return outboxIds.ToArray();
    }

    private IEnumerable<IDomainEventNotification<IDomainEvent>> GenerateDomainNotificationsIfExist(IEnumerable<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            var domainEvenNotificationType = typeof(IDomainEventNotification<>);
            var domainNotificationWithGenericType = domainEvenNotificationType.MakeGenericType(domainEvent.GetType());
            var domainNotification = _scope.ResolveOptional(domainNotificationWithGenericType, new List<Parameter>
            {
                new NamedParameter("domainEvent", domainEvent),
                new NamedParameter("id", Guid.NewGuid())
            });

            if (domainNotification != null)
            {
                yield return domainNotification as IDomainEventNotification<IDomainEvent>;
            }
        }
    }
}

public class OutboxMessageRefrences
{
    public OutboxMessageRefrences(Guid id,
        string aggregateId,
        DateTime occurredOn,
        string type)
    {
        Id = id;
        AggregateId = aggregateId;
        OccurredOn = occurredOn;
        Type = type;
    }

    public Guid Id { get; set; }
    public string AggregateId { get; set; }
    public DateTime OccurredOn { get; set; }
    public string Type { get; set; }
}
