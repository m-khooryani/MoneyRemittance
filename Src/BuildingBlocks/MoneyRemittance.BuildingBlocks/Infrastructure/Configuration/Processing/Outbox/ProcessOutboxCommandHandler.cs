using Autofac;
using MediatR;
using Microsoft.Extensions.Logging;
using MoneyRemittance.BuildingBlocks.Application.Configuration.Commands;
using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.BuildingBlocks.Application.Outbox;
using MoneyRemittance.BuildingBlocks.Domain;
using MoneyRemittance.BuildingBlocks.Infrastructure.CleanArchitecture;
using MoneyRemittance.BuildingBlocks.Infrastructure.RetryPolicy;
using Newtonsoft.Json;
using Polly;

namespace MoneyRemittance.BuildingBlocks.Infrastructure.Configuration.Processing.Outbox;

internal class ProcessOutboxCommandHandler : CommandHandler<ProcessOutboxCommand>
{
    private readonly IOutboxRepository _outboxRepository;
    private readonly IMediator _mediator;
    private readonly CleanArchitectureLayers _layers;
    private readonly PollyConfig _pollyConfig;
    private readonly ILogger _logger;
    private int _executedTimes;

    public ProcessOutboxCommandHandler(
        IOutboxRepository outboxRepository,
        IMediator mediator,
        CleanArchitectureLayers layers,
        PollyConfig pollyConfig,
        ILogger logger)
    {
        _outboxRepository = outboxRepository;
        _mediator = mediator;
        _layers = layers;
        _pollyConfig = pollyConfig;
        _logger = logger;
        _executedTimes = 0;
    }

    public override async Task<Unit> HandleAsync(ProcessOutboxCommand request, CancellationToken cancellationToken)
    {
        var messageId = Guid.Parse(request.MessageId);
        var outboxMessage = await _outboxRepository
            .LoadAsync(messageId, cancellationToken);

        if (outboxMessage is null)
        {
            _logger.LogWarning("outbox message not found! skipping.");
            return Unit.Value;
        }
        var policy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(_pollyConfig.SleepDurations);
        var result = await policy.ExecuteAndCaptureAsync(() => ProcessCommandAndDeleteAsync(outboxMessage));
        if (result.Outcome == OutcomeType.Failure)
        {
            _logger.LogError("failed to process outbox message.");
            outboxMessage.Error = result.FinalException.ToString();
            outboxMessage.ProcessedDate = Clock.Now;
        }

        return Unit.Value;
    }

    private async Task ProcessCommandAndDeleteAsync(OutboxMessage outboxMessage)
    {
        var commandStatus = _executedTimes == _pollyConfig.SleepDurations.Length ?
            CommandExecutionStatus.LastRetry : CommandExecutionStatus.Retry;
        _executedTimes++;

        var type = _layers.ApplicationLayer.GetType(outboxMessage.Type);
        var commandToProcess = JsonConvert.DeserializeObject(outboxMessage.Data, type) as dynamic;

        using var scope = CompositionRoot.BeginLifetimeScope();
        var commandExecutionStatus = scope.Resolve<CommandExecution>();
        commandExecutionStatus.SetCommandExecutionStatus(commandStatus);
        var mediator = scope.Resolve<IMediator>();
        await mediator.Send(commandToProcess);

        _outboxRepository.Remove(outboxMessage);
    }
}
