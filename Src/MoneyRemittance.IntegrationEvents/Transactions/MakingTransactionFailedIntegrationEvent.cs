using MoneyRemittance.BuildingBlocks.Infrastructure.EventBus;
using MoneyRemittance.Domain.Transactions;

namespace MoneyRemittance.IntegrationEvents.Transactions;

public record MakingTransactionFailedIntegrationEvent
    (TransactionId TransactionId)
    : IntegrationEvent(TransactionId, "makingTransactionFailedEvent");
