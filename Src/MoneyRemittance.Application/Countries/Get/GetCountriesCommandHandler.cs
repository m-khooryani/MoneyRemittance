using MoneyRemittance.BuildingBlocks.Application.Configuration.Commands;
using MoneyRemittance.Domain.Countries.Services;

namespace MoneyRemittance.Application.Countries.Get;

internal class GetCountriesCommandHandler : CommandHandler<GetCountriesCommand, CountryDto[]>
{
    private readonly ICountry _country;

    public GetCountriesCommandHandler(ICountry country)
    {
        _country = country;
    }

    public override async Task<CountryDto[]> HandleAsync(GetCountriesCommand command, CancellationToken cancellationToken)
    {
        return await _country.GetCountriesAsync();
    }
}
