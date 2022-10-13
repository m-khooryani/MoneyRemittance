using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.IntegrationTests._SeedWork;
using MoneyRemittance.TestHelpers.Application;
using Xunit;
using Xunit.Abstractions;

namespace MoneyRemittance.IntegrationTests;

[Collection("Database collection")]
public class ExchangeRateTests
{
    private readonly IServiceMediator _mediator = TestFixture.Mediator;
    private readonly TestFixture _testFixture;

    public ExchangeRateTests(TestFixture fixture, ITestOutputHelper output)
    {
        _testFixture = fixture;
        TestFixture.Output = output;
    }

    [Fact]
    public async Task Get_ExchangeRate_success()
    {
        await _testFixture.ResetAsync();

        // Get Beneficiary Name
        var getExchangeRateCommand = new GetExchangeRateCommandBuilder()
            .Build();
        var exchangeRateDto = await _mediator.CommandAsync(getExchangeRateCommand);

        // Assert
        Assert.Equal("Sweden", exchangeRateDto.SourceCountry);
    }
}
