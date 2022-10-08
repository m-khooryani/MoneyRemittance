using Autofac;
using Microsoft.Extensions.Logging;

namespace MoneyRemittance.BuildingBlocks.Infrastructure.Logging;

public class LoggingModule : Module
{
    private readonly ILogger _logger;

    public LoggingModule(ILoggerFactory loggerFactory, string categoryName = "MoneyRemittanceLogger")
    {
        _logger = loggerFactory.CreateLogger(categoryName);
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(_logger)
            .As<ILogger>()
            .SingleInstance();
    }
}
