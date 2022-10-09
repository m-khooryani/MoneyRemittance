using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.IntegrationTests._SeedWork;
using MoneyRemittance.TestHelpers.Application;
using Xunit;
using Xunit.Abstractions;

namespace MoneyRemittance.IntegrationTests;

[Collection("Database collection")]
public class BankTests
{
    private readonly IServiceMediator _mediator = TestFixture.Mediator;
    private readonly TestFixture _testFixture;

    public BankTests(TestFixture fixture, ITestOutputHelper output)
    {
        _testFixture = fixture;
        TestFixture.Output = output;
    }

    [Fact]
    public async Task Get_BankList_success()
    {
        await _testFixture.ResetAsync();

        // Get Bank List
        var createCommand = new GetBankListCommandBuilder()
            .SetCountry("US")
            .Build();
        var banks = await _mediator.CommandAsync(createCommand);

        // Assert
        Assert.Equal(2, banks.Length);
    }
}
