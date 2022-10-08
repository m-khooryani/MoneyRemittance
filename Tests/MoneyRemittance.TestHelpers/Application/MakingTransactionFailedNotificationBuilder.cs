using MoneyRemittance.Application.Transactions.Commands.Make;
using MoneyRemittance.Application.Transactions.Saga.MakingFailed;

namespace MoneyRemittance.TestHelpers.Application;

public class MakingTransactionFailedNotificationBuilder
{
    private MakeTransactionCommand _makeTransactionCommand = new MakeTransactionCommandBuilder().Build();

    public MakingTransactionFailedNotification Build()
    {
        return new MakingTransactionFailedNotification(_makeTransactionCommand);
    }
}
