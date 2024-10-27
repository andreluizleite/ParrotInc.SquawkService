using ParrotInc.SquawkService.Domain.Specifications;
public class SquawkSpecificationTests
{
    [Fact]
    public void IsSatisfiedBy_ContentNotEmpty_ShouldReturnTrue()
    {
        // Arrange
        var specification = new ContentSpecification();
        string content = "This is a valid squawk.";

        // Act
        var result = specification.IsSatisfiedBy(content);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_EmptyContent_ShouldReturnFalse()
    {
        // Arrange
        var specification = new ContentSpecification(); 
        string content = ""; // Empty content

        // Act
        var result = specification.IsSatisfiedBy(content);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsSatisfiedBy_NullContent_ShouldReturnFalse()
    {
        // Arrange
        var specification = new ContentSpecification(); 
        string content = null; // Null content

        // Act
        var result = specification.IsSatisfiedBy(content);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsSatisfiedBy_ContentTooLong_ShouldReturnFalse()
    {
        // Arrange
        var specification = new ContentLengthSpecification(400);
        string content = new string('a', 401); 

        // Act
        var result = specification.IsSatisfiedBy(content);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsSatisfiedBy_ContentContainingTweet_ShouldReturnFalse()
    {
        // Arrange
        var specification = new ContentRestrictionSpecification();
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
        var specification = new ContentRestrictionSpecification();
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
        var specification = new ContentRestrictionSpecification();
        var content = "This Squawk mentions both Tweet and Twitter.";

        // Act
        var result = specification.IsSatisfiedBy(content);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsSatisfiedBy_ContentWithoutRestrictedWords_ShouldReturnTrue()
    {
        // Arrange
        var specification = new ContentRestrictionSpecification();
        var content = "This Squawk is free of restricted words.";

        // Act
        var result = specification.IsSatisfiedBy(content);

        // Assert
        Assert.True(result);
    }
}
