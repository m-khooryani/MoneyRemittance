using MoneyRemittance.Application.ExchangeRates.Get;

namespace MoneyRemittance.TestHelpers.Application;

public class GetExchangeRateCommandBuilder
{
    private string _from = "SE";
    private string _to = "FI";

    public GetExchangeRateCommand Build()
    {
        return new GetExchangeRateCommand(_from, _to);
    }

    public GetExchangeRateCommandBuilder SetFrom(string from)
    {
        _from = from;
        return this;
    }

    public GetExchangeRateCommandBuilder SetTo(string to)
    {
        _to = to;
        return this;
    }
}
