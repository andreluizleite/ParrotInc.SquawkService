using Moq;
using ParrotInc.SquawkService.Domain.Entities;
using ParrotInc.SquawkService.Domain.Interfaces;

namespace ParrotInc.SquawkService.Tests.Domain
{
    public class SquawkSpecificationTests
    {
        [Fact]
        public void CreateSquawk_WithValidParameters_ShouldCreateSquawk()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var content = "This is a test squawk.";

            var specificationMock = new Mock<ISquawkSpecification>();
            specificationMock.Setup(spec => spec.IsSatisfiedBy(content)).Returns(true);

            var eventMock = new Mock<IEventPublisher>();

            var specifications = new List<ISquawkSpecification> { specificationMock.Object };

            // Act
            var squawk = Squawk.CreateSquawk(userId, content, specifications, eventMock.Object);

            // Assert
            Assert.NotNull(squawk);
            Assert.Equal(userId, squawk.Metadata.UserId);
            Assert.Equal(content, squawk.Content);
        }

        [Fact]
        public void CreateSquawk_WithInvalidContent_ShouldThrowArgumentNullException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            string content = ""; 

            var specificationMock = new Mock<ISquawkSpecification>();
            specificationMock.Setup(spec => spec.IsSatisfiedBy(content)).Returns(false);

            var eventMock = new Mock<IEventPublisher>();

            var specifications = new List<ISquawkSpecification> { specificationMock.Object };

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => Squawk.CreateSquawk(userId, content, specifications, eventMock.Object));
            Assert.Equal("content", exception.ParamName);
        }

        [Fact]
        public void CreateSquawk_WithNullContent_ShouldThrowArgumentNullException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            string content = null;

            var specificationMock = new Mock<ISquawkSpecification>();
            specificationMock.Setup(spec => spec.IsSatisfiedBy(content)).Returns(false);

            var eventMock = new Mock<IEventPublisher>();

            var specifications = new List<ISquawkSpecification> { specificationMock.Object };

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => Squawk.CreateSquawk(userId, content, specifications, eventMock.Object));
            Assert.Equal("content", exception.ParamName);
        }
        [Fact]
        public void CreateSquawk_WithMoreThan400Characters_ShouldThrowArgumentNullException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            string content = new string('a', 401); 

            var specificationMock = new Mock<ISquawkSpecification>();
            specificationMock.Setup(spec => spec.IsSatisfiedBy(content)).Returns(false); 

            var eventMock = new Mock<IEventPublisher>();

            var specifications = new List<ISquawkSpecification> { specificationMock.Object };

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                Squawk.CreateSquawk(userId, content, specifications, eventMock.Object));

            // Assert
            Assert.Equal("content", exception.ParamName);
        }
        [Fact]
        public void IsSatisfiedBy_ContentContainingTweet_ShouldReturnFalse()
        {
            // Arrange
            var specification = new SquawkContentRestrictionSpecification();
            var content = "This Squawk contains the word Tweet.";

            // Act
            var result = specification.IsSatisfiedBy(content);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public void IsSatisfiedBy_ContentContainingTwitter_ShouldReturnFalse()
        {
            // Arrange
            var specification = new SquawkContentRestrictionSpecification();
            var content = "This Squawk talks about Twitter.";

            // Act
            var result = specification.IsSatisfiedBy(content);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public void IsSatisfiedBy_ContentContainingBothRestrictedWords_ShouldReturnFalse()
        {
            // Arrange
            var specification = new SquawkContentRestrictionSpecification();
            var content = "This Squawk mentions both Tweet and Twitter.";

            // Act
            var result = specification.IsSatisfiedBy(content);

            // Assert
            Assert.False(result);
        }
    }
}
