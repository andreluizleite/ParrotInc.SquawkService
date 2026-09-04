using ParrotInc.SquawkService.Domain.Exceptions;

namespace ParrotInc.SquawkService.Domain.ValueObjects;

public sealed record SquawkContent
{
    public const int MaximumLength = 400;

    private static readonly string[] RestrictedTerms = ["tweet", "twitter"];

    private SquawkContent(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static SquawkContent Create(string? value)
    {
        var normalized = value?.Trim();

        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new SquawkRuleViolationException(
                "content_required",
                "Squawk content is required.");
        }

        if (normalized.Length > MaximumLength)
        {
            throw new SquawkRuleViolationException(
                "content_too_long",
                $"Squawk content cannot exceed {MaximumLength} characters.");
        }

        if (RestrictedTerms.Any(term =>
            normalized.Contains(term, StringComparison.OrdinalIgnoreCase)))
        {
            throw new SquawkRuleViolationException(
                "restricted_content",
                "Squawk content contains a restricted term.");
        }

        return new SquawkContent(normalized);
    }

    public override string ToString() => Value;
}
