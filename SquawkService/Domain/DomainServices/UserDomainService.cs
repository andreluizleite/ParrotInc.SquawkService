using ParrotInc.SquawkService.Domain.Entities;
using ParrotInc.SquawkService.Domain.ValueObjects;

namespace ParrotInc.SquawkService.Application.Services
{
    public interface IUserService
    {
        Task<bool> CreatePostAsync(UserId userId, string content);
        User GetUser(UserId userId);
        void AddUser(User user);
    }

    public class UserDomainService : IUserService
    {
        private readonly Dictionary<UserId, User> _users = new();

        public async Task<bool> CreatePostAsync(UserId userId, string content)
        {
            if (!_users.TryGetValue(userId, out var user))
            {
                return false; 
            }

            if (user.CanPost())
            {
                Console.WriteLine($"User {user.Name} created a post: {content}");

                // Update the last post time
                user.UpdateLastPostTime();
                return true;
            }

            return false; 
        }

        public User GetUser(UserId userId)
        {
            _users.TryGetValue(userId, out var user);
            return user; 
        }

        public void AddUser(User user)
        {
            if (!_users.ContainsKey(user.Id))
            {
                _users[user.Id] = user; 
            }
        }
    }
}
