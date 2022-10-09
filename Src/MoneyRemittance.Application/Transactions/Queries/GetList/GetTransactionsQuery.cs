using MoneyRemittance.BuildingBlocks.Application.Contracts;

namespace MoneyRemittance.Application.Transactions.Queries.GetList;

public record GetTransactionsQuery(
    int PageNumber,
    int PageSize) : PageableQuery<TransactionDto>(PageNumber, PageSize);
