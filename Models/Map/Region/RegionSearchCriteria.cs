using SardCoreAPI.Models.Common;

namespace SardCoreAPI.Models.Map.Region
{
    public class RegionSearchCriteria : PagedSearchCriteria
    {
        public int? LocationId { get; set; }
    }
}
