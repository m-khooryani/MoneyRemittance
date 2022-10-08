using Autofac;
using CacheQ;
using MediatR;
using MoneyRemittance.BuildingBlocks.Application.Contracts;

namespace MoneyRemittance.BuildingBlocks.Infrastructure.Processing.QueryPipelines;

internal class QueryCachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IQuery<TResponse>
{
    private readonly ICachePolicy<TRequest> _cachePolicy;
    private readonly ILifetimeScope _scope;
    private ICacheManager _cacheManager;

    public QueryCachingBehavior(
        IEnumerable<ICachePolicy<TRequest>> cachePolicy,
        ILifetimeScope scope)
    {
        _cachePolicy = cachePolicy.SingleOrDefault();
        _scope = scope;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        if (_cachePolicy is null)
        {
            return await next();
        }
        _cacheManager = _scope.Resolve<ICacheManager>();
        return await ReadOrUpdateCache(request, next);
    }

    private async Task<TResponse> ReadOrUpdateCache(TRequest request, RequestHandlerDelegate<TResponse> next)
    {
        if (_cacheManager.TryGetValue(
                        _cachePolicy,
                        request,
                        out TResponse cachedResult))
        {
            return await Task.FromResult(cachedResult);
        }

        // Read From Handler
        TResponse result = await next();

        // Update Cache
        _cacheManager.SetItem(_cachePolicy, request, result);

        return result;
    }
}
