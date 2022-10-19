using MoneyRemittance.Domain.ExchangeRates.Services;
using Swashbuckle.AspNetCore.Filters;

namespace MoneyRemittance.API.Controllers.ExchangeRates.Examples;

internal class ExchangeRateDtoExample : IExamplesProvider<ExchangeRateDto>
{
    public ExchangeRateDto GetExamples()
    {
        return new ExchangeRateDto("Sweden", "Finland", "1.012", "ERT");
    }
}
