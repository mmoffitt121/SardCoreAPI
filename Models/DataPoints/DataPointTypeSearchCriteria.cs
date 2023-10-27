using SardCoreAPI.Models.Common;

namespace SardCoreAPI.Models.DataPoints
{
    public class DataPointTypeSearchCriteria : PagedSearchCriteria
    {
        public int[]? DataPointTypeIds { get; set; }
    }
}
