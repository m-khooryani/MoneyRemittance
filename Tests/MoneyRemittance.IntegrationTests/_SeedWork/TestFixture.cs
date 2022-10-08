using System.Reflection;
using Autofac;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.BuildingBlocks.AzureServiceBus;
using MoneyRemittance.BuildingBlocks.Domain;
using MoneyRemittance.BuildingBlocks.Infrastructure;
using MoneyRemittance.BuildingBlocks.Infrastructure.CleanArchitecture;
using MoneyRemittance.BuildingBlocks.Infrastructure.Configuration;
using MoneyRemittance.BuildingBlocks.Infrastructure.Configuration.Processing.Outbox;
using MoneyRemittance.BuildingBlocks.Infrastructure.Logging;
using MoneyRemittance.BuildingBlocks.Infrastructure.Mediation;
using MoneyRemittance.BuildingBlocks.Infrastructure.Processing;
using MoneyRemittance.BuildingBlocks.Infrastructure.RetryPolicy;
using MoneyRemittance.BuildingBlocks.UnitOfWork.EFCore;
using MoneyRemittance.Domain.Countries.Services;
using MoneyRemittance.Domain.Transactions;
using MoneyRemittance.Domain.Transactions.Services;
using MoneyRemittance.Infrastructure;
using MoneyRemittance.Infrastructure.Configuration;
using NSubstitute;
using Xunit.Abstractions;

namespace MoneyRemittance.IntegrationTests._SeedWork;

public class TestFixture : IDisposable
{
    public static ITestOutputHelper Output { get; set; }
    public static IServiceMediator Mediator { get; private set; }

    private readonly string _databaseId = "MoneyRemittanceDBTest_" + Guid.NewGuid().ToString()[..6];

    private static readonly Action<MoneyRemittanceDbContext> _clearDbAction = context =>
    {
        var properties = context
            .GetType()
            .GetProperties()
            .Where(property => property.PropertyType.IsGenericType &&
                property.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));
        foreach (var property in properties)
        {
            var dbSet = context.GetType().GetProperty(property.Name).GetValue(context, null) as dynamic;
            DbSetUtility.Clear(dbSet);
        }
    };

    public TestFixture()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json", true)
            .AddUserSecrets<TestFixture>()
            .AddEnvironmentVariables()
            .Build();

        var dbContextOptionsBuilder = new DbContextOptionsBuilder<MoneyRemittanceDbContext>();
        //dbContextOptionsBuilder
        //    .ReplaceService<IValueConverterSelector, SqlServerTypedIdValueConverterSelector>();

        var dataSource = configuration["dataSource"];
        dbContextOptionsBuilder.UseSqlServer(GetConnectionString());
        dbContextOptionsBuilder.UseLoggerFactory(GetLoggerFactory());
        SetupCompositionRoot(configuration, dbContextOptionsBuilder);

        Mediator = new ServiceMediator();
        InitialDatabase();
    }

    private void SetupCompositionRoot(IConfiguration configuration, DbContextOptionsBuilder<MoneyRemittanceDbContext> contextOptionsBuilder)
    {
        var domainAssembly = Assembly.Load("MoneyRemittance.Domain");
        var infrastructureAssembly = Assembly.Load("MoneyRemittance.Infrastructure");
        var applicationAssembly = Assembly.Load("MoneyRemittance.Application");

        var cleanArchitectureModule = new CleanArchitectureModule(domainAssembly, applicationAssembly);
        var processingModule = new ProcessingModule(applicationAssembly);
        var mediatorModule = new MediatorModule(
            new[]
            {
                applicationAssembly,
                //infrastructureAssembly,
            },
            new[]
            {
                typeof(IRequestHandler<>),
                typeof(IRequestHandler<,>),
                typeof(INotificationHandler<>),
                typeof(IValidator<>),
            });
        var unitOfWorkModule = new EfCoreUnitOfWorkModule<MoneyRemittanceDbContext>(contextOptionsBuilder, infrastructureAssembly);
        var loggingModule = new LoggingModule(GetLoggerFactory());
        //var dapperModule = new DapperModule(GetConnectionString());

        // Domain Services
        var transactionSubmitting = Substitute.For<ITransactionSubmitting>();
        transactionSubmitting.SubmitAsync(Arg.Any<Transaction>()).Returns(TransactionId.New());
        var countryExistanceChecking = Substitute.For<ICountryExistanceChecking>();
        countryExistanceChecking.ExistsAsync(Arg.Any<string>()).Returns(true);
        var domainServiceModule = new DomainServicesModule(transactionSubmitting, countryExistanceChecking);

        var retryPolicyModule = new RetryPolicyModule(new PollyConfig()
        {
            SleepDurations = Array.Empty<TimeSpan>()
        });
        var azureServiceBusModule = new AzureServiceBusModule()
        {
            TopicClientFactory = Substitute.For<ITopicClientFactory>(),
            QueueClientFactory = Substitute.For<IQueueClientFactory>(),
            ConnectionString = "notEmpty",
            OutboxQueueName = "notEmpty",
        };

        CompositionRoot.Initialize(
            cleanArchitectureModule,
            processingModule,
            mediatorModule,
            unitOfWorkModule,
            loggingModule,
            //dapperModule,
            domainServiceModule,
            retryPolicyModule,
            azureServiceBusModule);
    }

    private static LoggerFactory GetLoggerFactory()
    {
        return new LoggerFactory(new[]
        {
            new LogToActionLoggerProvider(
                new Dictionary<string, LogLevel>()
                {
                    { "Default", LogLevel.Debug },
                    { "Microsoft", LogLevel.Trace },
                },
                log =>
                {
                    Output?.WriteLine(log);
                }
            )
        });
    }

    void IDisposable.Dispose()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json", true)
            .AddUserSecrets<TestFixture>()
            .AddEnvironmentVariables()
            .Build();

        var dbContextOptionsBuilder = new DbContextOptionsBuilder<MoneyRemittanceDbContext>();
        //dbContextOptionsBuilder
        //    .ReplaceService<IValueConverterSelector, SqlServerTypedIdValueConverterSelector>();

        var dataSource = configuration["dataSource"];
        dbContextOptionsBuilder.UseSqlServer(GetConnectionString());
        SetupCompositionRoot(configuration, dbContextOptionsBuilder);

        using var scope = CompositionRoot.BeginLifetimeScope();
        var context = scope.Resolve<MoneyRemittanceDbContext>();
        context.Database.EnsureDeleted();
    }

    internal static TService ResolveService<TService>()
        where TService : notnull
    {
        using var scope = CompositionRoot.BeginLifetimeScope();
        var service = scope.Resolve<TService>();
        return service;
    }

    private string GetConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json", true)
            .AddUserSecrets<TestFixture>()
            .AddEnvironmentVariables()
            .Build();
        var dataSource = configuration["dataSource"];
        return $"Data Source={dataSource};Initial Catalog={_databaseId};Integrated Security=True";
    }

    private void InitialDatabase()
    {
        using var scope = CompositionRoot.BeginLifetimeScope();
        var context = scope.Resolve<MoneyRemittanceDbContext>();
        context.Database.EnsureCreated();
    }

    internal async Task ResetAsync()
    {
        await ClearDatabase();
        Clock.Reset();
    }

    private async Task ClearDatabase()
    {
        await using var scope = CompositionRoot.BeginLifetimeScope();
        var context = scope.Resolve<MoneyRemittanceDbContext>();

        _clearDbAction(context);

        await context.SaveChangesAsync();
    }

    internal async Task ProcessLastOutboxMessageAsync()
    {
        await using var scope = CompositionRoot.BeginLifetimeScope();
        var context = scope.Resolve<MoneyRemittanceDbContext>();
        var message = await context.OutboxMessages.OrderBy(x => x.OccurredOn).LastAsync();

        await Mediator.CommandAsync(new ProcessOutboxCommand(message.Id.ToString()));
    }
}
