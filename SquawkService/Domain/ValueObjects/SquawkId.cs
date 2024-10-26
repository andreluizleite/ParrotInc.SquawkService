namespace ParrotInc.SquawkService.Domain.Entities
{
    public class SquawkId
    {
        public Guid Value { get; private set; }

        public SquawkId()
        {
            Value = Guid.NewGuid();
        }
        public SquawkId(Guid id)
        {
            Value = id;
        }

        public override bool Equals(object obj)
        {
            return obj is SquawkId id && Value.Equals(id.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
