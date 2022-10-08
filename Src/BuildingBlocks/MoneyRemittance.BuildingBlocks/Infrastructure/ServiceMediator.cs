using Autofac;
using MediatR;
using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.BuildingBlocks.Infrastructure.Configuration;

namespace MoneyRemittance.BuildingBlocks.Infrastructure;

public class ServiceMediator : IServiceMediator
{
    public Task<TResult> CommandAsync<TResult>(ICommand<TResult> command)
    {
        using var scope = CompositionRoot.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();

        return mediator.Send(command);
    }

    public async Task CommandAsync(ICommand command)
    {
        using var scope = CompositionRoot.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();

        await mediator.Send(command);
    }

    public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
    {
        using var scope = CompositionRoot.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();

        return mediator.Send(query);
    }
}
