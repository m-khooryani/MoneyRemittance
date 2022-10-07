using MoneyRemittance.BuildingBlocks.Domain;
using MoneyRemittance.Domain.Countries.Services;

namespace MoneyRemittance.Domain.Transactions.Rules;

public class TransactionWithInvalidCountryCanNotBeMadeRule : IBusinessRule
{
    private readonly string _country;
    private readonly ICountryExistanceChecking _countryExistanceChecking;

    public TransactionWithInvalidCountryCanNotBeMadeRule(
        string country,
        ICountryExistanceChecking countryExistanceChecking)
    {
        _country = country;
        _countryExistanceChecking = countryExistanceChecking;
    }

    public string Description => "Transaction with invalid country can not be made.";

    public async Task<bool> IsViolatedAsync()
    {
        return !await _countryExistanceChecking.ExistsAsync(_country);
    }
}
