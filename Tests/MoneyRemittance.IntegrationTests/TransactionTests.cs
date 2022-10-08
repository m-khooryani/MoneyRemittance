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
    public async Task Test1()
    {
        await _testFixture.ResetAsync();

        var createCommand = new MakeTransactionCommandBuilder()
            .Build();
        await _mediator.CommandAsync(createCommand);
    }
}
