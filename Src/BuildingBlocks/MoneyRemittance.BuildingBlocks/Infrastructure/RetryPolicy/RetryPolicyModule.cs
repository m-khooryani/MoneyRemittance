using Autofac;

namespace MoneyRemittance.BuildingBlocks.Infrastructure.RetryPolicy;

public class RetryPolicyModule : Module
{
    private readonly PollyConfig _pollyConfig;

    public RetryPolicyModule(PollyConfig pollyConfig)
    {
        _pollyConfig = pollyConfig;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterInstance(_pollyConfig)
            .AsSelf()
            .SingleInstance();
    }
}
