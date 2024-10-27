using Moq;
using ParrotInc.SquawkService.Domain.Entities;
using ParrotInc.SquawkService.Application.Services;
using ParrotInc.SquawkService.Domain.Interfaces;

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

        var expectedSquawk = await Squawk.CreateSquawkAsync(userId, content, eventMock.Object);

        // Set up the mock to return a squawk when CreateSquawkAsync is called
        mockSquawkDomainService
            .Setup(service => service.CreateSquawkAsync(userId, content))
            .ReturnsAsync(expectedSquawk);

        var appService = new SquawkAppService(mockSquawkDomainService.Object);

        // Act
        var result = await appService.CreateSquawkAsync(userId, content);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedSquawk.Id, result.Id);
        Assert.Equal(expectedSquawk.Content, result.Content);
        Assert.Equal(userId, result.Metadata.UserId);
    }
}
