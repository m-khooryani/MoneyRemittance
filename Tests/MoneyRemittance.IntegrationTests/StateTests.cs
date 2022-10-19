using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.IntegrationTests._SeedWork;
using MoneyRemittance.TestHelpers.Application;
using Xunit;
using Xunit.Abstractions;

namespace MoneyRemittance.IntegrationTests;

[Collection("Database collection")]
public class StateTests
{
    private readonly IServiceMediator _mediator = TestFixture.Mediator;
    private readonly TestFixture _testFixture;

    public StateTests(TestFixture fixture, ITestOutputHelper output)
    {
        _testFixture = fixture;
        TestFixture.Output = output;
    }

    [Fact]
    public async Task Get_States_success()
    {
        await _testFixture.ResetAsync();

        // Get State List
        var createCommand = new GetStatesCommandBuilder()
            .Build();
        var states = await _mediator.CommandAsync(createCommand);

        // Assert
        Assert.Equal(2, states.Length);
    }
}
