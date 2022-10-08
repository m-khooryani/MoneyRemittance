using MoneyRemittance.Application.Transactions.Saga.MakingFailed;
using MoneyRemittance.BuildingBlocks.Infrastructure.EventBus;
using MoneyRemittance.IntegrationEvents.Transactions;
using MoneyRemittance.TestHelpers.Application;
using NSubstitute;
using Xunit;

namespace MoneyRemittance.Application.UnitTests;

public class TransactionTests
{
    [Fact]
    public async Task MakingTransactionFailedNotification_should_publish_MakingTransactionFailedIntegrationEvent()
    {
        // Arrange
        var eventBus = Substitute.For<IEventBus>();
        var notification = new MakingTransactionFailedNotificationBuilder().Build();
        var handler = new MakingTransactionFailedNotificationHandler(eventBus);

        // Act
        await handler.Handle(notification, CancellationToken.None);

        // Assert
        await eventBus.Received(1)
            .Publish(Arg.Any<MakingTransactionFailedIntegrationEvent>());
    }
}