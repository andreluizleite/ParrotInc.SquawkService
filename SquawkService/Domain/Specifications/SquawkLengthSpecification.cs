using ParrotInc.SquawkService.Domain.Interfaces;

public class SquawkLengthSpecification : ISquawkSpecification
{
    private readonly int _maxLength;

    public SquawkLengthSpecification(int maxLength)
    {
        _maxLength = maxLength;
    }

    public bool IsSatisfiedBy(string content)
    {
        return content.Length <= _maxLength;
    }
}