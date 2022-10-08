using MediatR;
using MoneyRemittance.BuildingBlocks.Application.Events;
using MoneyRemittance.BuildingBlocks.Domain;

namespace MoneyRemittance.BuildingBlocks.Infrastructure.Processing.NotificationPipelines;

internal class NotificationUnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IDomainNotificationRequest, IRequest<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public NotificationUnitOfWorkBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var result = await next();
        await _unitOfWork.CommitAsync(cancellationToken);

        return result;
    }
}
