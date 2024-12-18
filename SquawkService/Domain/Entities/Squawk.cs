using ParrotInc.SquawkService.Domain.Events;
using ParrotInc.SquawkService.Domain.Interfaces;
using ParrotInc.SquawkService.Domain.ValueObjects;
using System.Security.Cryptography;
using System.Text;

namespace ParrotInc.SquawkService.Domain.Entities
{
    public class Squawk
    {
        public SquawkId Id { get; private set; }
        public string Content { get; private set; }
        public SquawkMetadata Metadata { get; private set; }


        private Squawk(SquawkId id, string content, SquawkMetadata metadata)
        {
            Id = id;
            Metadata = metadata;
            Content = content;
        }

        public static async Task<Squawk> CreateSquawkAsync(Guid userId, string content, IEventPublisher eventPublisher)
        {
            var squawkId = new SquawkId();
            var metadata = new SquawkMetadata(userId);
            var squawk = new Squawk(squawkId, content, metadata);

            // Publish the event
            var squawkCreatedEvent = new SquawkCreatedEvent(squawkId, content, userId);
            await eventPublisher.Publish(new List<SquawkCreatedEvent> { squawkCreatedEvent });

            return squawk;
        }
        public static string GenerateHash(Guid userId, string content)
        {
            using (var sha256 = SHA256.Create())
            {
                var combinedBytes = Encoding.UTF8.GetBytes($"{userId}-{content}");
                var hashBytes = sha256.ComputeHash(combinedBytes);

                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
