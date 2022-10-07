using MoneyRemittance.BuildingBlocks.Domain;
using MoneyRemittance.Domain.Countries.Services;
using MoneyRemittance.Domain.Transactions;
using MoneyRemittance.Domain.Transactions.Services;

namespace MoneyRemittance.TestHelpers.Domain;

public class TransactionBuilder
{
    private ITransactionSubmitting _transactionSubmitting;
    private ICountryExistanceChecking _countryExistanceChecking;
    private TransactionId _transactionId = TransactionId.New();
    private string _senderFirstName = Guid.NewGuid().ToString()[30..];
    private string _senderLastName = Guid.NewGuid().ToString()[30..];
    private string _senderEmail = Guid.NewGuid().ToString()[30..];
    private AddressInfo _senderAddress = new AddressInfoBuilder().Build();
    private DateTimeOffset _dateOfBirth = Clock.Now;
    private string _toFirstName = Guid.NewGuid().ToString()[30..];
    private string _toLastName = Guid.NewGuid().ToString()[30..];
    private string _toCountry = Guid.NewGuid().ToString()[30..];
    private string _toBankAccountName = Guid.NewGuid().ToString()[30..];
    private string _toBankAccountNumber = Guid.NewGuid().ToString()[30..];
    private string _toBankName = Guid.NewGuid().ToString()[30..];
    private string _toBankCode = Guid.NewGuid().ToString()[30..];
    private string _fromAmount = Guid.NewGuid().ToString()[30..];
    private decimal _exchangeRate = 2.5M;
    private decimal _fees = 2.5M;
    private string _transactionNumber = Guid.NewGuid().ToString()[30..];
    private string _fromCurrency = Guid.NewGuid().ToString()[30..];

    public TransactionBuilder SetTransactionSubmitter(ITransactionSubmitting transactionSubmitting)
    {
        _transactionSubmitting = transactionSubmitting;
        return this;
    }

    public TransactionBuilder SetCountryExistanceChecking(ICountryExistanceChecking countryExistanceChecking)
    {
        _countryExistanceChecking = countryExistanceChecking;
        return this;
    }

    public async Task<Transaction> BuildAsync()
    {
        return await Transaction.MakeAsync(
            _transactionSubmitting,
            _countryExistanceChecking,
            _transactionId,
            _senderFirstName,
            _senderLastName,
            _senderEmail,
            _senderAddress,
            _dateOfBirth,
            _toFirstName,
            _toLastName,
            _toCountry,
            _toBankAccountName,
            _toBankAccountNumber,
            _toBankName,
            _toBankCode,
            _fromAmount,
            _exchangeRate,
            _fees,
            _transactionNumber,
            _fromCurrency);
    }
}
