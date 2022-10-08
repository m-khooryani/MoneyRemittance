using MediatR;
using MoneyRemittance.BuildingBlocks.Domain;

namespace MoneyRemittance.BuildingBlocks.Application.Events;

public interface IDomainEventNotification<out TEvent> : IDomainNotificationRequest
    where TEvent : IDomainEvent
{
    TEvent DomainEvent { get; }
}

public interface IDomainNotificationRequest : IRequest<Unit>
{

}
