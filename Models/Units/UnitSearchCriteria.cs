using SardCoreAPI.Models.Common;

namespace SardCoreAPI.Models.Units
{
    public class UnitSearchCriteria : PagedSearchCriteria
    {
        public int? MeasurableId { get; set; }
    }
}
