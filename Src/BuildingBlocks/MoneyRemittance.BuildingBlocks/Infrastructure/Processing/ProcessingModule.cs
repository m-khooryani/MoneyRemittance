using System.Reflection;
using Autofac;
using MediatR;
using MoneyRemittance.BuildingBlocks.Application.Configuration.Commands;
using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.BuildingBlocks.Application.Events;
using MoneyRemittance.BuildingBlocks.Infrastructure.Configuration.Processing.QueuedCommand;
using MoneyRemittance.BuildingBlocks.Infrastructure.DomainEvents;
using MoneyRemittance.BuildingBlocks.Infrastructure.Processing.CommandFailedNotificationPipelines;
using MoneyRemittance.BuildingBlocks.Infrastructure.Processing.CommandPipelines;
using MoneyRemittance.BuildingBlocks.Infrastructure.Processing.NotificationPipelines;
using MoneyRemittance.BuildingBlocks.Infrastructure.Processing.QueryPipelines;

namespace MoneyRemittance.BuildingBlocks.Infrastructure.Processing;

public class ProcessingModule : Autofac.Module
{
    private readonly Assembly _applicationAssembly;

    public ProcessingModule(Assembly applicationAssembly)
    {
        _applicationAssembly = applicationAssembly;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<CommandExecution>()
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.RegisterType<DomainEventsDispatcher>()
            .As<IDomainEventsDispatcher>()
            .InstancePerLifetimeScope();

        builder.RegisterType<CommandsScheduler>()
            .As<ICommandsScheduler>()
            .InstancePerLifetimeScope();

        builder.RegisterType<DomainEventsAccessor>()
            .As<IDomainEventsAccessor>()
            .InstancePerLifetimeScope();

        // Command pipelines
        builder.RegisterGeneric(typeof(CommandFailingBehavior<,>))
            .As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(CommandLoggingBehavior<,>))
            .As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(CommandValidationBehavior<,>))
            .As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(CommandUnitOfWorkBehavior<,>))
            .As(typeof(IPipelineBehavior<,>));

        // Query pipelines
        builder.RegisterGeneric(typeof(QueryValidationBehavior<,>))
            .As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(QueryLoggingBehavior<,>)).
            As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(QueryCachingBehavior<,>)).
            As(typeof(IPipelineBehavior<,>));

        // Domain Notification pipelines
        builder.RegisterGeneric(typeof(NotificationLoggingBehavior<,>))
            .As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(NotificationUnitOfWorkBehavior<,>))
            .As(typeof(IPipelineBehavior<,>));

        // CommandFailed Notification pipeline
        builder.RegisterGeneric(typeof(CommandFailedNotificationLoggingBehavior<,>))
            .As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(CommandFailedNotificationUnitOfWorkBehavior<,>))
            .As(typeof(IPipelineBehavior<,>));

        builder.RegisterGenericDecorator(
            typeof(DomainEventsDispatcherNotificationHandlerDecorator<>),
            typeof(INotificationHandler<>));

        builder.RegisterAssemblyTypes(_applicationAssembly)
            .AsClosedTypesOf(typeof(IDomainEventNotification<>))
            .InstancePerDependency()
            .FindConstructorsWith(new AllConstructorFinder());

        builder.RegisterAssemblyTypes(_applicationAssembly)
            .AsClosedTypesOf(typeof(ICommandFailedNotification<,>))
            .InstancePerDependency()
            .FindConstructorsWith(new AllConstructorFinder());
    }
}
