using MoneyRemittance.Domain.Beneficiaries.Services;
using MoneyRemittance.Infrastructure.Configuration.B2BApi;

namespace MoneyRemittance.Infrastructure.Domain.Beneficiaries;

internal class Beneficiary : IBeneficiary
{
    private readonly B2BApiClient _apiClient;

    public Beneficiary(B2BApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<BeneficiaryNameDto> GetNameAsync(string accountNumber, string bankCode)
    {
        return await _apiClient.GetBeneficiaryNameAsync(accountNumber, bankCode);
    }
}
