using SardCoreAPI.Models.Common;

namespace SardCoreAPI.Models.Map.MapLayer
{
    public class MapLayerSearchCriteria : PagedSearchCriteria
    {
        public bool? IsBaseLayer { get; set; }
        public bool? IsIconLayer { get; set; }
        public int? MapId { get; set; }
    }
}
