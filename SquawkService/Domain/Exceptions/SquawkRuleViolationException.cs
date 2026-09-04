namespace ParrotInc.SquawkService.Domain.Exceptions;

public sealed class SquawkRuleViolationException : Exception
{
    public SquawkRuleViolationException(string code, string message)
        : base(message)
    {
        Code = code;
    }

    public string Code { get; }
}
