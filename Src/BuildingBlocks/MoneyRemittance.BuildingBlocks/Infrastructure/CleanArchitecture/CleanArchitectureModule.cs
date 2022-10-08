using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace MoneyRemittance.BuildingBlocks.Infrastructure.CleanArchitecture;

public class CleanArchitectureModule : Module
{
    private readonly Assembly _domainAssembly;
    private readonly Assembly _applicationAssembly;

    public CleanArchitectureModule(
        Assembly domainAssembly,
        Assembly applicationAssembly)
    {
        _domainAssembly = domainAssembly;
        _applicationAssembly = applicationAssembly;
    }

    protected override void Load(ContainerBuilder builder)
    {
        var cleanArchitecture = new CleanArchitectureLayers()
        {
            DomainLayer = _domainAssembly,
            ApplicationLayer = _applicationAssembly,
        };
        builder.RegisterInstance(cleanArchitecture)
            .AsSelf()
            .SingleInstance();
    }
}