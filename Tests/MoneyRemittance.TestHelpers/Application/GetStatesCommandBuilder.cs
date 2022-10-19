using MoneyRemittance.Application.States.Get;

namespace MoneyRemittance.TestHelpers.Application;

public class GetStatesCommandBuilder
{
    public GetStatesCommand Build()
    {
        return new GetStatesCommand();
    }
}
