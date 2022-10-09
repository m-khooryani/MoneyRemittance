using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MoneyRemittance.BuildingBlocks.Extensions.Hosting;

public abstract class SessionMessageHandler<T> : BackgroundService
    where T : MessageBase
{
    private ServiceBusClient _client;
    private readonly ILogger<SessionMessageHandler<T>> _logger;
    private readonly ServiceBusConfig _serviceBusConfig = new();
    private readonly IJsonMessageResolver _jsonResolver;

    public SessionMessageHandler(
        ILogger<SessionMessageHandler<T>> logger,
        IConfiguration configuration,
        IJsonMessageResolver jsonResolver)
    {
        _logger = logger;
        _jsonResolver = jsonResolver;
        var serviceBusConfigSection = configuration.GetSection(ServiceBusConfig.ServiceBus);
        if (!serviceBusConfigSection.Exists())
        {
            var errorMessage = $"Section [{ServiceBusConfig.ServiceBus}] not found in configuration.";
            _logger.LogError(errorMessage);
            throw new Exception(errorMessage);
        }
        configuration.GetSection(ServiceBusConfig.ServiceBus).Bind(_serviceBusConfig);
    }

    protected abstract Task Handle(T request, CancellationToken stoppingToken);

    protected abstract ServiceBusSessionProcessor GetServiceBusSessionProcessor(ServiceBusClient serviceBusClient);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _client = new ServiceBusClient(_serviceBusConfig.Connection);
        var processor = GetServiceBusSessionProcessor(_client);

        processor.ProcessMessageAsync += args1 => MessageHandler(args1, stoppingToken);
        processor.ProcessErrorAsync += ErrorHandler;

        await processor.StartProcessingAsync(stoppingToken);
    }

    private async Task MessageHandler(ProcessSessionMessageEventArgs args, CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation($"Message received; {typeof(T).Name}; SessionId: {args.Message.SessionId}");

            var body = _jsonResolver.Resolve(args.Message.Body.ToString());
            _logger.LogInformation($"MessageBody: {body}");

            var request = JsonConvert.DeserializeObject<T>(body);

            _logger.LogInformation($"Deserialized request: {JsonConvert.SerializeObject(request, Formatting.Indented)}");

            await Handle(request, stoppingToken);

            await args.CompleteMessageAsync(args.Message, stoppingToken);
            await args.SetSessionStateAsync(new BinaryData("processed state"), stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in processing message: " + ex.ToString());
            throw;
        }
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        _logger.LogError("Error in processing...");
        _logger.LogError(args.ErrorSource.ToString());
        _logger.LogError(args.FullyQualifiedNamespace);
        _logger.LogError(args.EntityPath);
        _logger.LogError(args.Exception.ToString());
        return Task.CompletedTask;
    }
}
