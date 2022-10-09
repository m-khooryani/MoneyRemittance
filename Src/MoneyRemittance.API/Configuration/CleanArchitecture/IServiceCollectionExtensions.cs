using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyRemittance.API.Configuration.AzureServiceBus;
using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.BuildingBlocks.AzureServiceBus;
using MoneyRemittance.BuildingBlocks.Dapper;
using MoneyRemittance.BuildingBlocks.Infrastructure;
using MoneyRemittance.BuildingBlocks.Infrastructure.CleanArchitecture;
using MoneyRemittance.BuildingBlocks.Infrastructure.Configuration;
using MoneyRemittance.BuildingBlocks.Infrastructure.Logging;
using MoneyRemittance.BuildingBlocks.Infrastructure.Mediation;
using MoneyRemittance.BuildingBlocks.Infrastructure.Processing;
using MoneyRemittance.BuildingBlocks.Infrastructure.RetryPolicy;
using MoneyRemittance.BuildingBlocks.UnitOfWork.EFCore;
using MoneyRemittance.Infrastructure;
using MoneyRemittance.Infrastructure.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

internal static partial class IServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessLogicServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IServiceMediator, ServiceMediator>();

        var loggerFactory = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();

        var domainAssembly = Assembly.Load("MoneyRemittance.Domain");
        var infrastructureAssembly = Assembly.Load("MoneyRemittance.Infrastructure");
        var applicationAssembly = Assembly.Load("MoneyRemittance.Application");

        var contextOptionsBuilder = new DbContextOptionsBuilder<MoneyRemittanceDbContext>();
        contextOptionsBuilder.UseLoggerFactory(loggerFactory);
        contextOptionsBuilder.UseSqlServer(configuration["ConnectionString"]);

        var cleanArchitectureModule = new CleanArchitectureModule(domainAssembly, applicationAssembly);
        var processingModule = new ProcessingModule(applicationAssembly);
        var mediatorModule = new MediatorModule(
            new[]
            {
                applicationAssembly
            },
            new[]
            {
                typeof(IRequestHandler<>),
                typeof(IRequestHandler<,>),
                typeof(INotificationHandler<>),
                typeof(IValidator<>),
            });
        var unitOfWorkModule = new EfCoreUnitOfWorkModule<MoneyRemittanceDbContext>(contextOptionsBuilder, infrastructureAssembly);
        var loggingModule = new LoggingModule(loggerFactory);
        var dapperModule = new DapperModule(configuration["ConnectionString"]);
        var domainServiceModule = new DomainServicesModule();
        var retryPolicyModule = new RetryPolicyModule(new PollyConfig()
        {
            SleepDurations = new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2),
            }
        });
        var azureServiceBusModule = new AzureServiceBusModule()
        {
            TopicClientFactory = null,
            QueueClientFactory = null,
            ConnectionString = configuration.GetSection("ServiceBusConfig")["Connection"].ToString(),
            OutboxQueueName = AzureServiceBusConstants.OutboxQueueName,
        };

        CompositionRoot.Initialize(
            cleanArchitectureModule,
            processingModule,
            mediatorModule,
            unitOfWorkModule,
            loggingModule,
            dapperModule,
            domainServiceModule,
            retryPolicyModule,
            azureServiceBusModule);

        return services;
    }
}
