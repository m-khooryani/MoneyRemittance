using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.Domain.Banks.Services;

namespace MoneyRemittance.Application.Banks.GetList;

public record GetBankListCommand(string Country) 
    : Command<BankDto[]>;
