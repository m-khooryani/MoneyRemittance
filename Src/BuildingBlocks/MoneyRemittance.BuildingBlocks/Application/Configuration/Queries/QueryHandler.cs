using MediatR;
using MoneyRemittance.BuildingBlocks.Application.Contracts;

namespace MoneyRemittance.BuildingBlocks.Application.Configuration.Queries;

public abstract class QueryHandler<TQuery, TResult> :
      IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    public async Task<TResult> Handle(TQuery request, CancellationToken cancellationToken)
    {
        return await HandleAsync(request, cancellationToken);
    }

    public abstract Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken);
}
