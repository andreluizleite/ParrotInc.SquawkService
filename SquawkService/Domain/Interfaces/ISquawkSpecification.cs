namespace ParrotInc.SquawkService.Domain.Interfaces
{
    public interface ISquawkSpecification
    {
        bool IsSatisfiedBy(string content);
    }
}
