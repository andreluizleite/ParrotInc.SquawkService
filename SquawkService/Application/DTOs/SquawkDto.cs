using ParrotInc.SquawkService.Domain.Entities;

namespace ParrotInc.SquawkService.Application.Dtos;

public sealed record SquawkDto(
    Guid Id,
    Guid UserId,
    string Content,
    DateTimeOffset CreatedAt)
{
    public static SquawkDto FromDomain(Squawk squawk)
    {
        return new SquawkDto(
            squawk.Id.Value,
            squawk.Metadata.UserId,
            squawk.Content.Value,
            squawk.Metadata.CreatedAt);
    }
}
