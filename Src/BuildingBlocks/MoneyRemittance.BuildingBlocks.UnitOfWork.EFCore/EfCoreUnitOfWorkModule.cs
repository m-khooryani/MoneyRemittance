using System.Reflection;
using Autofac;
using Microsoft.EntityFrameworkCore;
using MoneyRemittance.BuildingBlocks.Application.QueuedCommands;
using MoneyRemittance.BuildingBlocks.Domain;
using MoneyRemittance.BuildingBlocks.Infrastructure;
using MoneyRemittance.BuildingBlocks.UnitOfWork.EFCore.QueuedCommands;
using MoneyRemittance.BuildingBlocks.UnitOfWork.EFCore.UnitOfWorks;

namespace MoneyRemittance.BuildingBlocks.UnitOfWork.EFCore;

public class EfCoreUnitOfWorkModule<T> : Autofac.Module where T : ServiceDbContext
{
    private readonly DbContextOptionsBuilder<T> _dbContextOptions;
    private readonly Assembly _assembly;

    public EfCoreUnitOfWorkModule(DbContextOptionsBuilder<T> dbContextOptions,
        Assembly infrastructureAssembly)
    {
        _dbContextOptions = dbContextOptions;
        _assembly = infrastructureAssembly;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder
            .Register(c =>
            {
                return (T)Activator.CreateInstance(typeof(T), _dbContextOptions.Options);
            })
            .AsSelf()
            .As<ServiceDbContext>()
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(_assembly)
            .Where(type => type.Name.EndsWith("Repository"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .FindConstructorsWith(new AllConstructorFinder());

        builder
            .RegisterType<AggregateRepository>()
            .As<IAggregateRepository>()
            .InstancePerLifetimeScope();

        // UnitOfWork
        builder.RegisterType<EfCoreUnitOfWork>()
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();
        builder.RegisterDecorator(
            typeof(AppendingAggregateHistoryUnitOfWorkDecorator),
            typeof(IUnitOfWork));
        builder.RegisterDecorator(
            typeof(PublishingOutboxMessagesUnitOfWorkDecorator),
            typeof(IUnitOfWork));
        builder.RegisterDecorator(
            typeof(PublishingQueuedCommandsUnitOfWorkDecorator),
            typeof(IUnitOfWork));
        builder.RegisterDecorator(
            typeof(LoggingUnitOfWorkDecorator),
            typeof(IUnitOfWork));

        // QueuedCommandResolver
        builder.RegisterType<QueuedCommandsResolver>()
            .As<IQueuedCommandsResolver>()
            .InstancePerLifetimeScope();
        builder.RegisterDecorator(
            typeof(QueuedCommandsResolverCachingDecorator),
            typeof(IQueuedCommandsResolver));
    }
}
