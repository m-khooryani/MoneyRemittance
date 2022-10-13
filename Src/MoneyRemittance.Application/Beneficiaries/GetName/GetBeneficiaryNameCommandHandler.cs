using MoneyRemittance.BuildingBlocks.Application.Configuration.Commands;
using MoneyRemittance.Domain.Beneficiaries.Services;

namespace MoneyRemittance.Application.Beneficiaries.GetName;

internal class GetBeneficiaryNameCommandHandler : CommandHandler<GetBeneficiaryNameCommand, BeneficiaryNameDto>
{
    private readonly IBeneficiary _beneficiary;

    public GetBeneficiaryNameCommandHandler(IBeneficiary beneficiary)
    {
        _beneficiary = beneficiary;
    }

    public override async Task<BeneficiaryNameDto> HandleAsync(GetBeneficiaryNameCommand command, CancellationToken cancellationToken)
    {
        return await _beneficiary.GetNameAsync(command.AccountNumber, command.BankCode);
    }
}
