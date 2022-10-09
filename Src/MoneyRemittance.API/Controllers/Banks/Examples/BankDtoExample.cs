using MoneyRemittance.Domain.Banks.Services;
using Swashbuckle.AspNetCore.Filters;

namespace MoneyRemittance.API.Controllers.Banks.Examples;

public class BankDtoExample : IExamplesProvider<BankDto[]>
{
    public BankDto[] GetExamples()
    {
        return new BankDto[]
        {
            new BankDto("JPMorgan Chase", "JPM"),
            new BankDto("Citigroup", "CITI"),
        };
    }
}
