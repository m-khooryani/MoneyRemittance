using System.Data;
using Autofac;
using Microsoft.Extensions.Logging;
using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.BuildingBlocks.Infrastructure.Configuration;

namespace Dapper;

public static class DbConnectionExtension
{
    public static Task<PagedDto<T>> PagedQueryAsync<T>(
        this IDbConnection dbConnection,
        PageableQuery<T> query,
        Select select,
        FromWhere fromWhere,
        OrderBy orderBy)
    {
        return PagedQueryAsync(dbConnection, query, select, fromWhere, orderBy, new DynamicParameters());  
    }

    public static async Task<PagedDto<T>> PagedQueryAsync<T>(
        this IDbConnection dbConnection,
        PageableQuery<T> query,
        Select select,
        FromWhere fromWhere,
        OrderBy orderBy,
        DynamicParameters dynamicParameters)
    {
        var sql = select + " " + fromWhere + " " + orderBy;
        var pageableSql = sql + $" OFFSET {query.PageSize * (query.PageNumber - 1)} ROWS FETCH NEXT {query.PageSize} ROWS ONLY;";
        using var scope = CompositionRoot.BeginLifetimeScope();
        var logger = scope.ResolveOptional<ILogger>();

        logger?.LogInformation("Executing Query on DB:");
        logger?.LogInformation(pageableSql);

        var items = await dbConnection.QueryAsync<T>(pageableSql, dynamicParameters);
        var totalItemsSql = $"SELECT COUNT(1) {fromWhere}";
        logger?.LogInformation("Getting TotalItems count...");
        var totalItems = await dbConnection.ExecuteScalarAsync<int>(totalItemsSql, dynamicParameters);
        logger?.LogInformation("Get TotalItems count executed.");

        var pagedDto = new PagedDto<T>
        {
            Items = items.ToArray(),
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalItems = totalItems,
        };
        pagedDto.TotalPages = (int)Math.Ceiling(pagedDto.TotalItems * 1.0 / pagedDto.PageSize);

        return pagedDto;
    }
}

public class Select
{
    private readonly string _sql;

    private Select(string sql)
    {
        _sql = sql;
    }

    public static Select FromSql(string selectSql)
    {
        return new Select(selectSql);
    }

    public override string ToString()
    {
        return _sql;
    }
}

public class FromWhere
{
    private readonly string _sql;

    private FromWhere(string sql)
    {
        _sql = sql;
    }

    public static FromWhere FromSql(string fromWhereSql)
    {
        return new FromWhere(fromWhereSql);
    }

    public override string ToString()
    {
        return _sql;
    }
}

public class OrderBy
{
    private readonly string _sql;

    private OrderBy(string sql)
    {
        _sql = sql;
    }

    public static OrderBy FromSql(string orderBySql)
    {
        return new OrderBy(orderBySql);
    }

    public override string ToString()
    {
        return _sql;
    }
}
