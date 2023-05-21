namespace SardCoreAPI.Models.Common
{
    public class PagedSearchCriteria
    {
        public int? Id { get; set; }
        public string? Query { get; set; }
        public int? PageNumber { get; set; } = 0;
        public int? PageSize { get; set; }
    }
}
