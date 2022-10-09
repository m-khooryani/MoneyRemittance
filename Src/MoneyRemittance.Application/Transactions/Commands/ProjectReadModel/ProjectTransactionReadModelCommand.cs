using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.Domain.Transactions;

namespace MoneyRemittance.Application.Transactions.Commands.ProjectReadModel;

public record ProjectTransactionReadModelCommand(
    TransactionId TransactionId) : Command;
