namespace ParrotInc.SquawkService.Domain.Interfaces
{
    namespace ParrotInc.SquawkService.Domain.Services
    {
        public interface ICacheService
        {
            void Set(string key, string value, TimeSpan? expiry = null);
            string Get(string key);
            void Delete(string key);
            void Clear();
        }
    }
}
