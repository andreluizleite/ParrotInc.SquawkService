using ParrotInc.SquawkService.Domain.Entities;
using ParrotInc.SquawkService.Domain.Events;

namespace ParrotInc.SquawkService.Tests.Domain.Events
{
    public class SquawkCreatedEventTests
    {
        [Fact]
        public void SquawkCreatedEvent_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var squawkId = new SquawkId();
            var content = "This is a test squawk.";
            var userId = Guid.NewGuid();
            var createdAt = DateTime.UtcNow;

            // Act
            var squawkCreatedEvent = new SquawkCreatedEvent(squawkId, content, userId);

            // Assert
            Assert.Equal(squawkId, squawkCreatedEvent.SquawkId);
            Assert.Equal(content, squawkCreatedEvent.Content);
            Assert.Equal(userId, squawkCreatedEvent.UserId);
            Assert.True((squawkCreatedEvent.CreatedAt - DateTime.UtcNow).TotalSeconds < 1,
                        "CreatedAt should be close to the current UTC time.");
        }
    }
}
