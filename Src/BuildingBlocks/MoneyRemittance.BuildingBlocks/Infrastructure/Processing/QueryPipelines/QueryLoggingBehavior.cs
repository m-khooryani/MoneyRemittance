using MediatR;
using Microsoft.Extensions.Logging;
using MoneyRemittance.BuildingBlocks.Application.Contracts;
using Newtonsoft.Json;

namespace MoneyRemittance.BuildingBlocks.Infrastructure.Processing.QueryPipelines;

internal class QueryLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IQuery<TResponse>
{
    private readonly ILogger _logger;

    public QueryLoggingBehavior(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        _logger.LogInformation($"{request.GetType().Name} is processing: {Environment.NewLine}{JsonConvert.SerializeObject(request, Formatting.Indented)}");
        try
        {
            TResponse result = await next();
            _logger.LogInformation($"Result: {Environment.NewLine}{JsonConvert.SerializeObject(result, Formatting.Indented)}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception:{Environment.NewLine}{ex}");
            throw;
        }
        finally
        {
            _logger.LogDebug($"{request.GetType().Name} is processed.");
        }
    }
}
