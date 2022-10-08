using MediatR;

namespace MoneyRemittance.BuildingBlocks.Application.Contracts;

public class CommandFailedNotification<TCommand, TResponse> : ICommandFailedNotification<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    public TCommand Command { get; }
    public Guid Id { get; }

    public CommandFailedNotification(TCommand command)
    {
        Command = command;
        Id = Guid.NewGuid();
    }
}
public interface ICommandFailedNotification<out TCommand, TResponse> : ICommandFailedNotification
    where TCommand : ICommand<TResponse>
{
    TCommand Command { get; }
}

public interface ICommandFailedNotification : IRequest<Unit>
{

}


public abstract class CommandFailedNotificationHandler<TNotification, TCommand, TResponse> : IRequestHandler<TNotification>
    where TNotification : CommandFailedNotification<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    public async Task<Unit> Handle(TNotification request, CancellationToken cancellationToken)
    {
        await HandleAsync(request, cancellationToken);
        return Unit.Value;
    }

    public abstract Task HandleAsync(TNotification notification, CancellationToken cancellationToken);
}