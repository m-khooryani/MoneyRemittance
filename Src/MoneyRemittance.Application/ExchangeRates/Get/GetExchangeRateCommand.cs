using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.Domain.ExchangeRates.Services;

namespace MoneyRemittance.Application.ExchangeRates.Get;

public record GetExchangeRateCommand(
    string From,
    string To) : Command<ExchangeRateDto>;
