namespace ParrotInc.SquawkService.Domain.ValueObjects
{
    public record UserId
    {
        public Guid Value { get; private set; }

        public UserId(Guid value)
        {
            Value = value;
        }
    }
}
