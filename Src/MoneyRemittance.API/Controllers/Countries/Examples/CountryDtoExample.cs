using MoneyRemittance.Domain.Countries.Services;
using Swashbuckle.AspNetCore.Filters;

namespace MoneyRemittance.API.Controllers.Countries.Examples;

public class CountryDtoExample : IExamplesProvider<CountryDto[]>
{
    public CountryDto[] GetExamples()
    {
        return new CountryDto[]
        {
            new CountryDto("Finland", "FI"),
            new CountryDto("Sweden", "SE")
        };
    }
}
