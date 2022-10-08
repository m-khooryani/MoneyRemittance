using MoneyRemittance.BuildingBlocks.Application.Contracts;

namespace MoneyRemittance.BuildingBlocks.Application.Configuration.Commands;

public interface ICommandsScheduler
{
    Task EnqueueAsync(ICommand command);
    Task EnqueueAsync<TResult>(ICommand<TResult> command);
}
