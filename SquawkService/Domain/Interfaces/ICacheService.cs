namespace ParrotInc.SquawkService.Domain.Interfaces;

public interface ICacheService
{
    bool TryAdd(string key, string value, TimeSpan expiry);

    void Delete(string key);
}
