namespace B2BAPI.Controllers.Transactions;

public record TransactionStatusResponse(Guid TransactionId, TransactionStatus Status);
