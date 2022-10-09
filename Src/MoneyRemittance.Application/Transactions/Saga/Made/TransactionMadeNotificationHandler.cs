using MoneyRemittance.Application.Transactions.Commands.ProjectReadModel;
using MoneyRemittance.BuildingBlocks.Application.Configuration.Commands;
using MoneyRemittance.BuildingBlocks.Application.Configuration.Notifications;
using MoneyRemittance.Domain.Transactions.Events;

namespace MoneyRemittance.Application.Transactions.Saga.Made;

internal class TransactionMadeNotificationHandler : DomainNotificationHandler<TransactionMadeNotification, TransactionMadeDomainEvent>
{
    private readonly ICommandsScheduler _commandsScheduler;

    public TransactionMadeNotificationHandler(ICommandsScheduler commandsScheduler)
    {
        _commandsScheduler = commandsScheduler;
    }

    public override async Task HandleAsync(TransactionMadeNotification notification, CancellationToken cancellationToken)
    {
        var projectionCommand = new ProjectTransactionReadModelCommand(notification.DomainEvent.TransactionId);
        await _commandsScheduler.EnqueueAsync(projectionCommand);
    }
}