using ParrotInc.SquawkService.Domain.Interfaces;
namespace ParrotInc.SquawkService.Domain.Specifications
{
    public class CompositeSquawkSpecification : ISquawkSpecification
    {
        private readonly IList<ISquawkSpecification> _specifications;

        public CompositeSquawkSpecification()
        {
            _specifications = new List<ISquawkSpecification>();
        }

        public void AddSpecification(ISquawkSpecification specification)
        {
            _specifications.Add(specification);
        }

        public bool IsSatisfiedBy(string content)
        {
            return _specifications.All(spec => spec.IsSatisfiedBy(content));
        }
    }
}