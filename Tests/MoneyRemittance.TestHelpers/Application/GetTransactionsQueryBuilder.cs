using MoneyRemittance.Application.Transactions.Queries.GetList;

namespace MoneyRemittance.TestHelpers.Application;

public class GetTransactionsQueryBuilder
{
    private int _pageNumber = 1;
    private int _pageSize = 10;

    public GetTransactionsQuery Build()
    {
        return new GetTransactionsQuery(_pageNumber, _pageSize);
    }

    public GetTransactionsQueryBuilder SetPageNumber(int pageNumber)
    {
        _pageNumber = pageNumber;
        return this;
    }

    public GetTransactionsQueryBuilder SetPageSize(int pageSize)
    {
        _pageSize = pageSize;
        return this;
    }
}
