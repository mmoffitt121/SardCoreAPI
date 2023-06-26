using lt = SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Models.Common;

namespace SardCoreAPI.Models.Map.Location
{
    public class LocationSearchCriteria : PagedSearchCriteria
    {
        public List<lt.LocationType>? LocationTypes { get; set; }
        public List<int>? MapLayerIds { get; set; }
        public double? MinLatitude { get; set; }
        public double? MaxLatitude { get; set; }
        public double? MinLongitude { get; set; }
        public double? MaxLongitude { get; set; }
        public double? MinZoom { get; set; }
        public double? MaxZoom { get; set; }
    }
}
