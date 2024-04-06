using SardCoreAPI.Models.Easy;

namespace SardCoreAPI.Models.Common
{
    public class PagedSearchCriteria : QueryOptions, IValidatable
    {
        public int? Id { get; set; }
        public string? StringId { get; set; }
        public string? Query { get; set; }

        public virtual List<string> Validate()
        {
            return new List<string> { };
        }
    }
}
