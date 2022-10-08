using Microsoft.Extensions.Logging;
using MoneyRemittance.BuildingBlocks.Application.Configuration.Commands;
using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.BuildingBlocks.Application.Outbox;
using MoneyRemittance.BuildingBlocks.Domain;
using MoneyRemittance.BuildingBlocks.Infrastructure.Serialization;
using Newtonsoft.Json;

namespace MoneyRemittance.BuildingBlocks.Infrastructure.Configuration.Processing.QueuedCommand;

internal class CommandsScheduler : ICommandsScheduler
{
    private readonly ILogger _logger;
    private readonly IOutboxRepository _repository;

    public CommandsScheduler(
        ILogger logger,
        IOutboxRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task EnqueueAsync(ICommand command)
    {
        var queuedCommand = new OutboxMessage(
            Clock.Now,
            command.GetType().FullName,
            JsonConvert.SerializeObject(command, new JsonSerializerSettings
            {
                ContractResolver = new AllPropertiesContractResolver()
            }));

        var json = JsonConvert.SerializeObject(queuedCommand, Formatting.Indented);
        _logger.LogInformation("Enqueue Command:");
        _logger.LogInformation(json);

        await _repository.AddAsync(queuedCommand);
    }

    public async Task EnqueueAsync<TResult>(ICommand<TResult> command)
    {
        var queuedCommand = new OutboxMessage(
            Clock.Now,
            command.GetType().FullName,
            JsonConvert.SerializeObject(command, new JsonSerializerSettings
            {
                ContractResolver = new AllPropertiesContractResolver()
            }));

        var json = JsonConvert.SerializeObject(queuedCommand, Formatting.Indented);
        _logger.LogInformation("Enqueue Command:");
        _logger.LogInformation(json);

        await _repository.AddAsync(queuedCommand);
    }
}
