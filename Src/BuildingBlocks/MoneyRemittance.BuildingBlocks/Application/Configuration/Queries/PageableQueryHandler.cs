using MediatR;
using MoneyRemittance.BuildingBlocks.Application.Contracts;

namespace MoneyRemittance.BuildingBlocks.Application.Configuration.Queries;

public abstract class PageableQueryHandler<TQuery, TResult> :
      IRequestHandler<TQuery, PagedDto<TResult>> where TQuery : IQuery<PagedDto<TResult>>
{
    public async Task<PagedDto<TResult>> Handle(TQuery request, CancellationToken cancellationToken)
    {
        return await HandleAsync(request, cancellationToken);
    }

    public abstract Task<PagedDto<TResult>> HandleAsync(TQuery query, CancellationToken cancellationToken);
}
