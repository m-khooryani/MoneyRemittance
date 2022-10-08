using MoneyRemittance.Application.Transactions.Commands.Make;
using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.BuildingBlocks.Infrastructure.EventBus;
using MoneyRemittance.Domain.Transactions;
using MoneyRemittance.IntegrationEvents.Transactions;

namespace MoneyRemittance.Application.Transactions.Saga.MakingFailed;

internal class MakingTransactionFailedNotificationHandler
    : CommandFailedNotificationHandler<MakingTransactionFailedNotification, MakeTransactionCommand, TransactionId>
{
    private readonly IEventBus _eventBus;

    public MakingTransactionFailedNotificationHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public override async Task HandleAsync(MakingTransactionFailedNotification notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new MakingTransactionFailedIntegrationEvent(notification.Command.TransactionId);
        await _eventBus.Publish(integrationEvent);
    }
}