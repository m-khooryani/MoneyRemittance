using MoneyRemittance.Application.Transactions.Commands.Make;
using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.Domain.Transactions;

namespace MoneyRemittance.Application.Transactions.Saga.MakingFailed;

public class MakingTransactionFailedNotification
    : CommandFailedNotification<MakeTransactionCommand, TransactionId>
{
    public MakingTransactionFailedNotification(MakeTransactionCommand command) : base(command)
    {
    }
}
