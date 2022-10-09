using System.Data;
using System.Data.SqlClient;

namespace MoneyRemittance.BuildingBlocks.Dapper;

public class DapperContext
{
    private readonly string _connectionString;

    public DapperContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection OpenConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
