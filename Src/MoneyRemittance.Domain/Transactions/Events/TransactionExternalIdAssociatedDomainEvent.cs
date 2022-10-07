using MoneyRemittance.BuildingBlocks.Domain;

namespace MoneyRemittance.Domain.Transactions.Events;

public record TransactionExternalIdAssociatedDomainEvent(
    TransactionId TransactionId,
    TransactionId ExternalId) : DomainEvent(TransactionId);
