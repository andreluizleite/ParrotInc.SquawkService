using Moq;
using ParrotInc.SquawkService.Application.EventPublishers;
using ParrotInc.SquawkService.Domain.Entities;
using ParrotInc.SquawkService.Domain.Interfaces;
using ParrotInc.SquawkService.Domain.Events;
using ParrotInc.SquawkService.Application.Services;
using ParrotInc.SquawkService.Application.Commands;
using ParrotInc.SquawkService.Application.CommandHandlers;
using Microsoft.Extensions.Logging;

public class SquawkEventPublisherTests
{
    [Fact]
    public async Task CreateSquawk_ShouldPublishSquawkCreatedEvent()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var content = "This is a test publisher queue.";

        var eventPublisher = new InMemoryEventPublisher();

        // Create a mock for ISquawkAppService
        var mockSquawkDomainService = new Mock<ISquawkDomainService>();

        //Logger
        var logger = new Mock<ILogger<CreateSquawkCommandHandler>>();

        var squawkId = new SquawkId();

        var expectedSquawk = await Squawk.CreateSquawkAsync(userId, content, eventPublisher);

        // Set up the mock to return the expected squawk
        mockSquawkDomainService
            .Setup(service => service.CreateSquawkAsync(userId, content))
            .ReturnsAsync(expectedSquawk);

        var appHandler = new CreateSquawkCommandHandler(mockSquawkDomainService.Object, logger.Object);
        CreateSquawkCommand command = new CreateSquawkCommand(userId, content);

        // Act
        var result = await appHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);

        // Verify that the event was published
        var publishedEvents = eventPublisher.GetPublishedEvents();
        Assert.Single(publishedEvents);
        Assert.IsType<SquawkCreatedEvent>(publishedEvents[0]);

        var squawkCreatedEvent = publishedEvents[0] as SquawkCreatedEvent;
        Assert.Equal(expectedSquawk.Id, squawkCreatedEvent.SquawkId);
        Assert.Equal(content, squawkCreatedEvent.Content);
        Assert.Equal(userId, squawkCreatedEvent.UserId);
        Assert.True(squawkCreatedEvent.CreatedAt <= DateTime.UtcNow);
    }
}
