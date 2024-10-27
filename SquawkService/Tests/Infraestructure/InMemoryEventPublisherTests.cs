using Moq;
using ParrotInc.SquawkService.Domain.Interfaces;
using ParrotInc.SquawkService.Infrastructure.EventPublishing;

public class InMemoryEventPublisherTests
{
    [Fact]
    public async Task Publish_ShouldAddEventsToQueue()
    {
        // Arrange
        var eventPublisher = new InMemoryEventPublisher();
        var testEvent = new Mock<IDomainEvent>().Object;

        // Act
        await eventPublisher.Publish(new[] { testEvent });

        await Task.Delay(100);
    }

    [Fact]
    public async Task RegisterEventHandler_ShouldInvokeHandlerOnEventPublish()
    {
        // Arrange
        var eventPublisher = new InMemoryEventPublisher();
        var wasCalled = false;
        var testEvent = new Mock<IDomainEvent>().Object;

        eventPublisher.RegisterEventHandler(async e =>
        {
            Assert.Equal(testEvent, e);
            wasCalled = true;
            await Task.CompletedTask;
        });

        // Act
        await eventPublisher.Publish(new[] { testEvent });

        await Task.Delay(4000);

        // Assert
        Assert.True(wasCalled, "The event handler was not called.");
    }

    [Fact]
    public async Task ProcessEventsAsync_ShouldHandleExceptions()
    {
        // Arrange
        var eventPublisher = new InMemoryEventPublisher();
        var testEvent = new Mock<IDomainEvent>().Object;

        eventPublisher.RegisterEventHandler(e =>
        {
            throw new Exception("Handler failed");
        });

        // Act
        await eventPublisher.Publish(new[] { testEvent });

        await Task.Delay(100);
    }

}
