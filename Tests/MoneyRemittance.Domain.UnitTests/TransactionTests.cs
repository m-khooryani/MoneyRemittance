using MoneyRemittance.Domain.Countries.Services;
using MoneyRemittance.Domain.Transactions;
using MoneyRemittance.Domain.Transactions.Events;
using MoneyRemittance.Domain.Transactions.Rules;
using MoneyRemittance.Domain.Transactions.Services;
using MoneyRemittance.Domain.UnitTests._SeedWork;
using MoneyRemittance.TestHelpers.Domain;
using NSubstitute;
using Xunit;

namespace MoneyRemittance.Domain.UnitTests;

public class TransactionTests : Test
{
    [Fact]
    public async Task Make_Transaction_with_invalid_country_is_not_possible()
    {
        // Arrange
        var transactionSubmitting = Substitute.For<ITransactionSubmitting>();
        transactionSubmitting.SubmitAsync(Arg.Any<Transaction>()).Returns(TransactionId.New());

        var countryExistanceChecking = Substitute.For<ICountryExistanceChecking>();
        countryExistanceChecking.ExistsAsync(Arg.Any<string>()).Returns(false);

        var builder = new TransactionBuilder()
            .SetTransactionSubmitter(transactionSubmitting)
            .SetCountryExistanceChecking(countryExistanceChecking);

        // Act, Assert
        await AssertViolatedRuleAsync<TransactionWithInvalidCountryCanNotBeMadeRule>(async () =>
        {
            _ = await builder.BuildAsync();
        });
    }

    [Fact]
    public async Task Make_Transaction_publish_TransactionMadeEvent()
    {
        // Arrange
        var transactionSubmitting = Substitute.For<ITransactionSubmitting>();
        transactionSubmitting.SubmitAsync(Arg.Any<Transaction>()).Returns(TransactionId.New());

        var countryExistanceChecking = Substitute.For<ICountryExistanceChecking>();
        countryExistanceChecking.ExistsAsync(Arg.Any<string>()).Returns(true);

        var builder = new TransactionBuilder()
            .SetTransactionSubmitter(transactionSubmitting)
            .SetCountryExistanceChecking(countryExistanceChecking);

        // Act
        var transaction = await builder.BuildAsync();

        // Assert
        AssertPublishedDomainEvent<TransactionMadeDomainEvent>(transaction);
    }

    [Fact]
    public async Task Make_Transaction_publish_TransactionExternalIdAssociatedEvent()
    {
        // Arrange
        var transactionSubmitting = Substitute.For<ITransactionSubmitting>();
        transactionSubmitting.SubmitAsync(Arg.Any<Transaction>()).Returns(TransactionId.New());

        var countryExistanceChecking = Substitute.For<ICountryExistanceChecking>();
        countryExistanceChecking.ExistsAsync(Arg.Any<string>()).Returns(true);

        var builder = new TransactionBuilder()
            .SetTransactionSubmitter(transactionSubmitting)
            .SetCountryExistanceChecking(countryExistanceChecking);

        // Act
        var transaction = await builder.BuildAsync();

        // Assert
        AssertPublishedDomainEvent<TransactionExternalIdAssociatedDomainEvent>(transaction);
    }
}
