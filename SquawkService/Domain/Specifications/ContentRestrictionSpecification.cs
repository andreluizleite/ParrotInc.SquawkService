using ParrotInc.SquawkService.Domain.Interfaces;
namespace ParrotInc.SquawkService.Domain.Specifications
{
    public class ContentRestrictionSpecification : ISquawkSpecification
    {
        public bool IsSatisfiedBy(string content)
        {
            if (string.IsNullOrEmpty(content))
                return true;

            // Check if the content contains the restricted words
            return !content.Contains("Tweet", StringComparison.OrdinalIgnoreCase) &&
                   !content.Contains("Twitter", StringComparison.OrdinalIgnoreCase);
        }
    }
}