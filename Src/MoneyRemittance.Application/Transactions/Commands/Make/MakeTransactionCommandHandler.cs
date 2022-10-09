using MoneyRemittance.BuildingBlocks.Application.Configuration.Commands;
using MoneyRemittance.BuildingBlocks.Domain;
using MoneyRemittance.Domain.Countries.Services;
using MoneyRemittance.Domain.Transactions;
using MoneyRemittance.Domain.Transactions.Services;

namespace MoneyRemittance.Application.Transactions.Commands.Make;

internal class MakeTransactionCommandHandler : CommandHandler<MakeTransactionCommand, TransactionId>
{
    private readonly IAggregateRepository _aggregateRepository;
    private readonly ITransactionSubmitting _transactionSubmitting;
    private readonly ICountryExistanceChecking _countryExistanceChecking;

    public MakeTransactionCommandHandler(
        IAggregateRepository aggregateRepository,
        ITransactionSubmitting transactionSubmitting, 
        ICountryExistanceChecking countryExistanceChecking)
    {
        _aggregateRepository = aggregateRepository;
        _transactionSubmitting = transactionSubmitting;
        _countryExistanceChecking = countryExistanceChecking;
    }

    public override async Task<TransactionId> HandleAsync(MakeTransactionCommand command, CancellationToken cancellationToken)
    {
        var transaction = await Transaction.MakeAsync(
            _transactionSubmitting,
            _countryExistanceChecking,
            command.TransactionId,
            command.SenderFirstName,
            command.SenderLastName,
            command.SenderEmail,
            command.SenderAddress,
            command.DateOfBirth,
            command.ToFirstName,
            command.ToLastName,
            command.ToCountry,
            command.ToBankAccountName,
            command.ToBankAccountNumber,
            command.ToBankName,
            command.ToBankCode,
            command.FromAmount,
            command.ExchangeRate,
            command.Fees,
            command.TransactionNumber,
            command.FromCurrency);

        _aggregateRepository.Add<Transaction, TransactionId>(transaction);

        return transaction.TransactionExternalId;
    }
}
