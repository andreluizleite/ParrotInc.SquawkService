using ParrotInc.SquawkService.Domain.Interfaces;
namespace ParrotInc.SquawkService.Domain.Specifications
{
    public class ContentSpecification : ISquawkSpecification
    {
        public bool IsSatisfiedBy(string content)
        {
            return !string.IsNullOrWhiteSpace(content);
        }
    }
}
