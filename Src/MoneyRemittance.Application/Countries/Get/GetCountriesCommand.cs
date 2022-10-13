using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.Domain.Countries.Services;

namespace MoneyRemittance.Application.Countries.Get;

public record GetCountriesCommand() : Command<CountryDto[]>;
