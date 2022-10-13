using MoneyRemittance.BuildingBlocks.Application.Configuration.Commands;
using MoneyRemittance.Domain.ExchangeRates.Services;

namespace MoneyRemittance.Application.ExchangeRates.Get;

internal class GetExchangeRateCommandHandler : CommandHandler<GetExchangeRateCommand, ExchangeRateDto>
{
    private readonly IExchangeRate _exchangeRate;

    public GetExchangeRateCommandHandler(IExchangeRate exchangeRate)
    {
        _exchangeRate = exchangeRate;
    }

    public override async Task<ExchangeRateDto> HandleAsync(GetExchangeRateCommand command, CancellationToken cancellationToken)
    {
        return await _exchangeRate.GetExchangeRateAsync(command.From, command.To);
    }
}
