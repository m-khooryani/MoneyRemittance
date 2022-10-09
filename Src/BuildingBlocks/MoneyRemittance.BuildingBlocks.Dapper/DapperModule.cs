using System.Diagnostics.CodeAnalysis;
using Autofac;

namespace MoneyRemittance.BuildingBlocks.Dapper;

[ExcludeFromCodeCoverage]
public class DapperModule : Module
{
    private readonly DapperContext _dapperContext;

    public DapperModule(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString));
        }
        _dapperContext = new DapperContext(connectionString);
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterInstance(_dapperContext)
            .AsSelf()
            .SingleInstance();
    }
}
