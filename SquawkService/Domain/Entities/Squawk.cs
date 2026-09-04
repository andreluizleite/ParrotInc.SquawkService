using System.Security.Cryptography;
using System.Text;
using ParrotInc.SquawkService.Domain.Exceptions;
using ParrotInc.SquawkService.Domain.ValueObjects;

namespace ParrotInc.SquawkService.Domain.Entities;

public sealed class Squawk
{
    private Squawk(
        SquawkId id,
        SquawkContent content,
        SquawkMetadata metadata)
    {
        Id = id;
        Content = content;
        Metadata = metadata;
    }

    public SquawkId Id { get; }

    public SquawkContent Content { get; }

    public SquawkMetadata Metadata { get; }

    public static Squawk Create(
        Guid userId,
        string? content,
        DateTimeOffset createdAt)
    {
        if (userId == Guid.Empty)
        {
            throw new SquawkRuleViolationException(
                "user_required",
                "A valid user identifier is required.");
        }

        return new Squawk(
            SquawkId.New(),
            SquawkContent.Create(content),
            new SquawkMetadata(userId, createdAt));
    }

    public static string GenerateContentHash(Guid userId, string content)
    {
        var normalized = $"{userId:N}:{content.Trim().ToUpperInvariant()}";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(normalized));

        return Convert.ToHexString(hash);
    }
}
