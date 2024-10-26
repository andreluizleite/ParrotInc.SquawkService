namespace ParrotInc.SquawkService.Domain.ValueObjects
{
    public record SquawkContent
    {
        public string Value { get; private set; }

        public SquawkContent(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Squawk content cannot be empty.", nameof(value));
            }
            Value = value;
        }
    }
}
