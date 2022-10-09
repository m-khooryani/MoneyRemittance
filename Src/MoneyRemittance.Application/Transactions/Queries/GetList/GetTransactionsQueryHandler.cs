using Dapper;
using MoneyRemittance.BuildingBlocks.Application.Configuration.Queries;
using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.BuildingBlocks.Dapper;

namespace MoneyRemittance.Application.Transactions.Queries.GetList;

internal class GetTransactionsQueryHandler : PageableQueryHandler<GetTransactionsQuery, TransactionDto>
{
    private readonly DapperContext _dapperContext;

    public GetTransactionsQueryHandler(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public override async Task<PagedDto<TransactionDto>> HandleAsync(GetTransactionsQuery query, CancellationToken cancellationToken)
    {
        var select = Select.FromSql($@"
            SELECT 
            [Transactions].[TransactionId]      AS {nameof(TransactionDto.Id)}, 
            [Transactions].[SenderFirstName]    AS {nameof(TransactionDto.SenderFirstName)}");
        var fromWhere = FromWhere.FromSql(
            $"FROM [Transactions]"
        );
        var orderBy = OrderBy.FromSql(
            "ORDER BY [Transactions].[TransactionId]");

        using var connection = _dapperContext.OpenConnection();
        var transactions = await connection.PagedQueryAsync(query, select, fromWhere, orderBy);

        return transactions;
    }
}
