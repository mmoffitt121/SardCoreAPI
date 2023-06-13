using SardCoreAPI.Models.Common;

namespace SardCoreAPI.Models.Map
{
    public class MapSearchCriteria : PagedSearchCriteria
    {
        public bool? IsDefault { get; set; }
    }
}
