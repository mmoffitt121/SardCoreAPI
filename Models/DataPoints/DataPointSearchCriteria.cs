using SardCoreAPI.Models.Common;

namespace SardCoreAPI.Models.DataPoints
{
    public class DataPointSearchCriteria : PagedSearchCriteria
    {
        public int? TypeId { get; set; }
    }
}
