using MediatR;
using Microsoft.Extensions.Logging;
using MoneyRemittance.BuildingBlocks.Application.Events;
using Newtonsoft.Json;

namespace MoneyRemittance.BuildingBlocks.Infrastructure.Processing.NotificationPipelines;

internal class NotificationLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IDomainNotificationRequest, IRequest<TResponse>
{
    private readonly ILogger _logger;

    public NotificationLoggingBehavior(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        _logger.LogInformation("{requestName} is processing: {environment}{request}",
            request.GetType().Name,
            Environment.NewLine,
            JsonConvert.SerializeObject(request, Formatting.Indented)
        );
        try
        {
            TResponse result = await next();
            if (typeof(TResponse) != typeof(Unit))
            {
                _logger.LogInformation("Result: {environment}{result}",
                    Environment.NewLine,
                    result);
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
        finally
        {
            _logger.LogInformation($"request {request.GetType().Name} is processed.");
        }
    }
}
