namespace MoneyRemittance.Domain.Transactions.Services;

public interface ITransactionSubmitting
{
    Task<TransactionId> SubmitAsync(Transaction transaction);
}
