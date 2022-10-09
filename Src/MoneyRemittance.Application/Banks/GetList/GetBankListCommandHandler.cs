using MoneyRemittance.BuildingBlocks.Application.Configuration.Commands;
using MoneyRemittance.Domain.Banks.Services;

namespace MoneyRemittance.Application.Banks.GetList;

internal class GetBankListCommandHandler : CommandHandler<GetBankListCommand, BankDto[]>
{
    private readonly IBankList _bankList;

    public GetBankListCommandHandler(IBankList bankList)
    {
        _bankList = bankList;
    }

    public override async Task<BankDto[]> HandleAsync(GetBankListCommand command, CancellationToken cancellationToken)
    {
        return await _bankList.GetBanksAsync(command.Country);
    }
}
