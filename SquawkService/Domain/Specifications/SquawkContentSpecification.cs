using ParrotInc.SquawkService.Domain.Entities;
using ParrotInc.SquawkService.Domain.Interfaces;
namespace ParrotInc.SquawkService.Specifications;

public class SquawkContentSpecification: ISquawkSpecification
{
    public bool IsSatisfiedBy(string content)
    {
        return !string.IsNullOrWhiteSpace(content);
    }
}
