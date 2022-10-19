using MoneyRemittance.BuildingBlocks.Application.Configuration.Commands;
using MoneyRemittance.Domain.States.Services;

namespace MoneyRemittance.Application.States.Get;

internal class GetStatesCommandHandler : CommandHandler<GetStatesCommand, StateDto[]>
{
    private readonly IState _state;

    public GetStatesCommandHandler(IState state)
    {
        _state = state;
    }

    public override async Task<StateDto[]> HandleAsync(GetStatesCommand command, CancellationToken cancellationToken)
    {
        return await _state.GetStatesAsync();
    }
}