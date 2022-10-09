using MoneyRemittance.BuildingBlocks.Application.Events;
using MoneyRemittance.Domain.Transactions.Events;

namespace MoneyRemittance.Application.Transactions.Saga.Made;

public class TransactionMadeNotification : DomainNotification<TransactionMadeDomainEvent>
{
    public TransactionMadeNotification(TransactionMadeDomainEvent domainEvent) : base(domainEvent)
    {
    }
}
