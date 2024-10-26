using ParrotInc.SquawkService.Domain.ValueObjects;

namespace ParrotInc.SquawkService.Domain.Entities
{
    public class User
    {
        public UserId Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }

        public User(UserId id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}
