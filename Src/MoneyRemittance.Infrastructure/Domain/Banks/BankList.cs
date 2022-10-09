using MoneyRemittance.Domain.Banks.Services;
using MoneyRemittance.Infrastructure.Configuration.B2BApi;

namespace MoneyRemittance.Infrastructure.Domain.Banks;

internal class BankList : IBankList
{
    private readonly B2BApiClient _apiClient;

    public BankList(B2BApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BankDto[]> GetBanksAsync(string country)
    {
        return await _apiClient.GetBanksAsync(country);
    }
}
