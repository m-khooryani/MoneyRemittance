namespace MoneyRemittance.BuildingBlocks.Application.Contracts;

public interface IServiceMediator
{
    Task<TResult> CommandAsync<TResult>(ICommand<TResult> command);

    Task CommandAsync(ICommand command);

    Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
}
