using ParrotInc.SquawkService.Domain.ValueObjects;
using System;

namespace ParrotInc.SquawkService.Domain.Entities
{
    public class User
    {
        public UserId Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        private DateTime _lastPostTime;

        public User(UserId id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
            _lastPostTime = DateTime.MinValue; 
        }

        public bool CanPost()
        {
            // Check if 20 seconds have passed since the last post
            return (DateTime.UtcNow - _lastPostTime).TotalSeconds >= 20;
        }

        public void UpdateLastPostTime()
        {
            _lastPostTime = DateTime.UtcNow;
        }
    }
}
