using ParrotInc.SquawkService.Domain.Interfaces;
namespace ParrotInc.SquawkService.Domain.Specifications
{
    public class ContentLengthSpecification : ISquawkSpecification
    {
        private readonly int _maxLength;

        public ContentLengthSpecification(int maxLength)
        {
            _maxLength = maxLength;
        }

        public bool IsSatisfiedBy(string content)
        {
            return content.Length <= _maxLength;
        }
    }
}