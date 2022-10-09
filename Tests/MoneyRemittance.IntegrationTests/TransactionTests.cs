using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.IntegrationTests._SeedWork;
using MoneyRemittance.TestHelpers.Application;
using Xunit;
using Xunit.Abstractions;

namespace MoneyRemittance.IntegrationTests;

[Collection("Database collection")]
public class TransactionTests
{
    private readonly IServiceMediator _mediator = TestFixture.Mediator;
    private readonly TestFixture _testFixture;

    public TransactionTests(TestFixture fixture, ITestOutputHelper output)
    {
        _testFixture = fixture;
        TestFixture.Output = output;
    }

    [Fact]
    public async Task Make_Transaction_success()
    {
        await _testFixture.ResetAsync();

        // Make Transaction
        var createCommand = new MakeTransactionCommandBuilder()
            .Build();
        await _mediator.CommandAsync(createCommand);

        // Queue Projection Command
        await _testFixture.ProcessLastOutboxMessageAsync();
        // Process Projection Command
        await _testFixture.ProcessLastOutboxMessageAsync();

        // Transactions Query
        var query = new GetTransactionsQueryBuilder()
            .Build();
        var transactions = await _mediator.QueryAsync(query);

        // Assert
        Assert.Single(transactions.Items);
    }
}
