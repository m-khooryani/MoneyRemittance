using MoneyRemittance.Application.Banks.GetList;

namespace MoneyRemittance.TestHelpers.Application;

public class GetBankListCommandBuilder
{
    private string _country = "US";

    public GetBankListCommand Build()
    {
        return new GetBankListCommand(_country);
    }

    public GetBankListCommandBuilder SetCountry(string country)
    {
        _country = country;
        return this;
    }
}
