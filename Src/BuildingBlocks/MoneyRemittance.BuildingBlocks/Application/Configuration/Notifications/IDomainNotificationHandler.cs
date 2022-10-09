using MediatR;
using MoneyRemittance.BuildingBlocks.Application.Events;
using MoneyRemittance.BuildingBlocks.Domain;

namespace MoneyRemittance.BuildingBlocks.Application.Configuration.Notifications;

public abstract class DomainNotificationHandler<TNotification, TEvent> : IRequestHandler<TNotification>
    where TNotification : IDomainEventNotification<TEvent>
    where TEvent : IDomainEvent
{
    public async Task<Unit> Handle(TNotification request, CancellationToken cancellationToken)
    {
        await HandleAsync(request, cancellationToken);
        return Unit.Value;
    }

    public abstract Task HandleAsync(TNotification notification, CancellationToken cancellationToken);
}
