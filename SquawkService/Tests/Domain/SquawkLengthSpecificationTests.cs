using ParrotInc.SquawkService.Domain.Interfaces;
using ParrotInc.SquawkService.Specifications;
using Xunit;

namespace ParrotInc.SquawkService.Tests.Domain.Specifications
{
    public class SquawkLengthSpecificationTests
    {
        [Fact]
        public void IsSatisfiedBy_ContentWithinMaxLength_ShouldReturnTrue()
        {
            // Arrange
            var maxLength = 400;
            var specification = new SquawkLengthSpecification(maxLength);
            var content = new string('a', 400); // Content exactly 400 characters long

            // Act
            var result = specification.IsSatisfiedBy(content);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsSatisfiedBy_ContentExceedingMaxLength_ShouldReturnFalse()
        {
            // Arrange
            var maxLength = 400;
            var specification = new SquawkLengthSpecification(maxLength);
            var content = new string('a', 401); // Content exceeding 400 characters

            // Act
            var result = specification.IsSatisfiedBy(content);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSatisfiedBy_EmptyContent_ShouldReturnTrue()
        {
            // Arrange
            var maxLength = 400;
            var specification = new SquawkLengthSpecification(maxLength);
            var content = string.Empty; // Empty content

            // Act
            var result = specification.IsSatisfiedBy(content);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsSatisfiedBy_ContentSpecification_EmptyContent_ShouldReturnTrue()
        {
            // Arrange
            var specification = new SquawkContentSpecification();
            var content = string.Empty; // Empty content

            // Act
            var result = specification.IsSatisfiedBy(content);

            // Assert
            Assert.False(result);
        }
    }
}
