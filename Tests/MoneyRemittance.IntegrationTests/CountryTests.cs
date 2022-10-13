using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.IntegrationTests._SeedWork;
using MoneyRemittance.TestHelpers.Application;
using Xunit;
using Xunit.Abstractions;

namespace MoneyRemittance.IntegrationTests;

[Collection("Database collection")]
public class CountryTests
{
    private readonly IServiceMediator _mediator = TestFixture.Mediator;
    private readonly TestFixture _testFixture;

    public CountryTests(TestFixture fixture, ITestOutputHelper output)
    {
        _testFixture = fixture;
        TestFixture.Output = output;
    }

    [Fact]
    public async Task Get_Countries_success()
    {
        await _testFixture.ResetAsync();

        // Get Bank List
        var createCommand = new GetCountriesCommandBuilder()
            .Build();
        var countries = await _mediator.CommandAsync(createCommand);

        // Assert
        Assert.Equal(2, countries.Length);
    }
}
