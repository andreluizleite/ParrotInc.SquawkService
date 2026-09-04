using ParrotInc.SquawkService.Domain.Exceptions;
using ParrotInc.SquawkService.Domain.ValueObjects;

namespace ParrotInc.SquawkService.Tests.Domain;

public sealed class SquawkContentTests
{
    [Fact]
    public void Create_ShouldTrimValidContent()
    {
        var content = SquawkContent.Create("  A useful engineering note.  ");

        Assert.Equal("A useful engineering note.", content.Value);
    }

    [Theory]
    [InlineData(null, "content_required")]
    [InlineData("   ", "content_required")]
    [InlineData("This mentions Twitter.", "restricted_content")]
    [InlineData("This mentions a Tweet.", "restricted_content")]
    public void Create_WithInvalidContent_ShouldReturnAStableRuleCode(
        string? value,
        string expectedCode)
    {
        var exception = Assert.Throws<SquawkRuleViolationException>(
            () => SquawkContent.Create(value));

        Assert.Equal(expectedCode, exception.Code);
    }

    [Fact]
    public void Create_WhenContentExceedsMaximum_ShouldFail()
    {
        var exception = Assert.Throws<SquawkRuleViolationException>(
            () => SquawkContent.Create(new string('a', SquawkContent.MaximumLength + 1)));

        Assert.Equal("content_too_long", exception.Code);
    }
}
