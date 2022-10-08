using Autofac;
using Autofac.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.BuildingBlocks.Infrastructure.Configuration;

namespace MoneyRemittance.BuildingBlocks.Infrastructure.Processing.CommandPipelines;

internal class CommandFailingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    private readonly ILogger _logger;
    private readonly ILifetimeScope _scope;

    public CommandFailingBehavior(ILogger logger, ILifetimeScope scope)
    {
        _logger = logger;
        _scope = scope;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            return await next();
        }
        catch (Exception)
        {
            try
            {
                await Check(request);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Error in saving command failed notification");
                _logger.LogCritical(ex.ToString());
            }
            throw;
        }
    }

    private async Task Check(TRequest request)
    {
        var t = request.GetType();
        var commandExecution = _scope.Resolve<CommandExecution>();
        if (commandExecution.GetCommandExecutionStatus() == CommandExecutionStatus.Retry)
        {
            _logger.LogDebug("Skip failing behavior since it is in retry mode");
            return;
        }

        var notificationType = typeof(ICommandFailedNotification<,>);
        var notificationWithGenericType = notificationType.MakeGenericType(request.GetType(), t.BaseType.GetGenericArguments().First());
        var commandFailedNotification = _scope.ResolveOptional(notificationWithGenericType, new List<Parameter>
            {
                new NamedParameter("command", request)
            });

        if (commandFailedNotification is null)
        {
            _logger.LogDebug("CommandFailedNotification not found for {Command}, skipping...", request.GetType().Name);
            return;
        }

        using var scope = CompositionRoot.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();
        await mediator.Send(commandFailedNotification);
    }
}
