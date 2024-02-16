namespace SardCoreAPI.Models.Common
{
    public class PagedSearchCriteria : IValidatable
    {
        public int? Id { get; set; }
        public string? Query { get; set; }
        public int? PageNumber { get; set; } = 0;
        public int? PageSize { get; set; }

        public virtual List<string> Validate()
        {
            return new List<string> { };
        }
    }
}
