using MoneyRemittance.BuildingBlocks.Application.Contracts;
using MoneyRemittance.IntegrationTests._SeedWork;
using MoneyRemittance.TestHelpers.Application;
using Xunit;
using Xunit.Abstractions;

namespace MoneyRemittance.IntegrationTests;

[Collection("Database collection")]
public class BeneficiaryTests
{
    private readonly IServiceMediator _mediator = TestFixture.Mediator;
    private readonly TestFixture _testFixture;

    public BeneficiaryTests(TestFixture fixture, ITestOutputHelper output)
    {
        _testFixture = fixture;
        TestFixture.Output = output;
    }

    [Fact]
    public async Task Get_BeneficiaryName_success()
    {
        await _testFixture.ResetAsync();

        // Get Bank List
        var getBeneficiaryNameCommand = new GetBeneficiaryNameCommandBuilder()
            .Build();
        var benefeciaryNameDto = await _mediator.CommandAsync(getBeneficiaryNameCommand);

        // Assert
        Assert.NotNull(benefeciaryNameDto.AccountName);
    }
}
