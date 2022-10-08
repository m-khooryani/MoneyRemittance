using MediatR;
using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.BuildingBlocks.Domain;

namespace MoneyRemittance.BuildingBlocks.Infrastructure.Processing.CommandFailedNotificationPipelines;

internal class CommandFailedNotificationUnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommandFailedNotification, IRequest<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public CommandFailedNotificationUnitOfWorkBehavior(IUnitOfWork unitOfWork)
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

