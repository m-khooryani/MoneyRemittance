using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.BuildingBlocks.Extensions.Hosting;
using MoneyRemittance.BuildingBlocks.Infrastructure.Configuration.Processing.Outbox;

namespace MoneyRemittance.API.Configuration.AzureServiceBus;

class OutboxMessageHandler : QueueSessionMessageHandler<OutboxMessage>
{
    private readonly IServiceMediator _mediator;

    public OutboxMessageHandler(
        ILogger<QueueSessionMessageHandler<OutboxMessage>> logger,
        IServiceMediator mediator,
        IConfiguration configuration,
        IJsonMessageResolver messageResolver)
        : base(logger, configuration, messageResolver, AzureServiceBusConstants.OutboxQueueName)
    {
        _mediator = mediator;
    }

    protected override async Task Handle(OutboxMessage request, CancellationToken stoppingToken)
    {
        await _mediator.CommandAsync(
            new ProcessOutboxCommand(request.Id.ToString()));
    }
}

internal record OutboxMessage(Guid Id) : MessageBase;