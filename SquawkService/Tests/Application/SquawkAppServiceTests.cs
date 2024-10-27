using Moq;
using ParrotInc.SquawkService.Domain.Entities;
using ParrotInc.SquawkService.Domain.Interfaces;
using ParrotInc.SquawkService.Application.CommandHandlers;
using ParrotInc.SquawkService.Application.Commands;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;

public class SquawkAppServiceTests
{
    [Fact]
    public async Task CreateSquawkAsync_WithValidParameters_ShouldReturnSquawk()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var content = "This is a test squawk.";

        var mockSquawkDomainService = new Mock<ISquawkDomainService>();

        //Events
        var eventMock = new Mock<IEventPublisher>();

        //Logger
        var logger = new Mock<ILogger<CreateSquawkCommandHandler>>();

        var expectedSquawk = await Squawk.CreateSquawkAsync(userId, content, eventMock.Object);

        // Set up the mock to return a squawk when CreateSquawkAsync is called
        mockSquawkDomainService
            .Setup(service => service.CreateSquawkAsync(userId, content))
            .ReturnsAsync(expectedSquawk);

        var appHandler = new CreateSquawkCommandHandler(mockSquawkDomainService.Object, logger.Object);
        CreateSquawkCommand command = new CreateSquawkCommand(userId, content);

        // Act
        var result = await appHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedSquawk.Content, result.Squawk.Content);
        Assert.Equal(userId, result.Squawk.UserId);
    }
}
