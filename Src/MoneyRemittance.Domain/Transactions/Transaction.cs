using MoneyRemittance.BuildingBlocks.Domain;
using MoneyRemittance.Domain.Countries.Services;
using MoneyRemittance.Domain.Transactions.Events;
using MoneyRemittance.Domain.Transactions.Rules;
using MoneyRemittance.Domain.Transactions.Services;

namespace MoneyRemittance.Domain.Transactions;

public class Transaction : AggregateRoot<TransactionId>
{
    public TransactionId TransactionExternalId { get; private set; }
    public string SenderFirstName { get; private set; }
    public string SenderLastName { get; private set; }
    public string SenderEmail { get; private set; }
    public AddressInfo SenderAddress { get; private set; }
    public DateTimeOffset DateOfBirth { get; private set; }
    public string ToFirstName { get; private set; }
    public string ToLastName { get; private set; }
    public string ToCountry { get; private set; }
    public string ToBankAccountName { get; private set; }
    public string ToBankAccountNumber { get; private set; }
    public string ToBankName { get; private set; }
    public string ToBankCode { get; private set; }
    public string FromAmount { get; private set; }
    public decimal ExchangeRate { get; private set; }
    public decimal Fees { get; private set; }
    public string TransactionNumber { get; private set; }
    public string FromCurrency { get; private set; }

    private Transaction() { }

    public static async Task<Transaction> MakeAsync(
        ITransactionSubmitting transactionSubmitting, 
        ICountryExistanceChecking countryExistanceChecking,
        TransactionId transactionId,
        string senderFirstName,
        string senderLastName,
        string senderEmail,
        AddressInfo senderAddress,
        DateTimeOffset dateOfBirth,
        string toFirstName,
        string toLastName,
        string toCountry,
        string toBankAccountName,
        string toBankAccountNumber,
        string toBankName,
        string toBankCode,
        string fromAmount,
        decimal exchangeRate,
        decimal fees,
        string transactionNumber,
        string fromCurrency)
    {
        // Business Rules
        await CheckRuleAsync(new TransactionWithInvalidCountryCanNotBeMadeRule(toCountry, countryExistanceChecking));

        // Make
        var @event = new TransactionMadeDomainEvent(
            transactionId,
            senderFirstName,
            senderLastName,
            senderEmail,
            senderAddress,
            dateOfBirth,
            toFirstName,
            toLastName,
            toCountry,
            toBankAccountName,
            toBankAccountNumber,
            toBankName,
            toBankCode,
            fromAmount,
            exchangeRate,
            fees,
            transactionNumber,
            fromCurrency);
        var transaction = new Transaction();
        transaction.Apply(@event);

        // External Id
        var externalId = await transactionSubmitting.SubmitAsync(transaction);
        var externalIdAssociatedEvent = new TransactionExternalIdAssociatedDomainEvent(transactionId, externalId);
        transaction.Apply(externalIdAssociatedEvent);

        return transaction;
    }

    protected void When(TransactionMadeDomainEvent @event)
    {
        Id = @event.TransactionId;
        SenderFirstName = @event.SenderFirstName;
        SenderLastName = @event.SenderLastName;
        SenderEmail = @event.SenderEmail;
        SenderAddress = @event.SenderAddress;
        DateOfBirth = @event.DateOfBirth;
        ToFirstName = @event.ToFirstName;
        ToLastName = @event.ToLastName;
        ToCountry = @event.ToCountry;
        ToBankAccountName = @event.ToBankAccountName;
        ToBankAccountNumber = @event.ToBankAccountNumber;
        ToBankName = @event.ToBankName;
        ToBankCode = @event.ToBankCode;
        FromAmount = @event.FromAmount;
        ExchangeRate = @event.ExchangeRate;
        Fees = @event.Fees;
        TransactionNumber = @event.TransactionNumber;
        FromCurrency = @event.FromCurrency;
    }

    protected void When(TransactionExternalIdAssociatedDomainEvent @event)
    {
        TransactionExternalId = @event.ExternalId;
    }
}
