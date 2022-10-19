using MoneyRemittance.Domain.States.Services;
using MoneyRemittance.Infrastructure.Configuration.B2BApi;

namespace MoneyRemittance.Infrastructure.Domain.States;

internal class State : IState
{
    private readonly B2BApiClient _apiClient;

    public State(B2BApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<StateDto[]> GetStatesAsync()
    {
        return await _apiClient.GetStateList();
    }
}
