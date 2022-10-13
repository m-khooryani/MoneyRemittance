using MoneyRemittance.Application.Countries.Get;

namespace MoneyRemittance.TestHelpers.Application;

public class GetCountriesCommandBuilder
{
    public GetCountriesCommand Build()
    {
        return new GetCountriesCommand();
    }
}
