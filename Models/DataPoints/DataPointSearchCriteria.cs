using SardCoreAPI.Models.Common;
using SardCoreAPI.Models.DataPoints.DataPointParameters;

namespace SardCoreAPI.Models.DataPoints
{
    public class DataPointSearchCriteria : PagedSearchCriteria
    {
        public int? TypeId { get; set; }
        public List<int>? TypeIds { get; set; }
        public List<DataPointParameter>? Parameters { get; set; }
    }
}
