using MoneyRemittance.Application.Beneficiaries.GetName;

namespace MoneyRemittance.TestHelpers.Application;

public class GetBeneficiaryNameCommandBuilder
{
    private string _accountNumber = "10002345610";
    private string _bankCode = "CIB";

    public GetBeneficiaryNameCommand Build()
    {
        return new GetBeneficiaryNameCommand(_accountNumber, _bankCode);
    }

    public GetBeneficiaryNameCommandBuilder SetAccountNumber(string accountNumber)
    {
        _accountNumber = accountNumber;
        return this;
    }

    public GetBeneficiaryNameCommandBuilder SetBankCode(string bankCode)
    {
        _bankCode = bankCode;
        return this;
    }
}
