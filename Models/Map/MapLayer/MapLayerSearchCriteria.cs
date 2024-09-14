using LinqKit;
using Microsoft.AspNetCore.Server.IISIntegration;
using SardCoreAPI.Models.Common;
using SardCoreAPI.Utility.DataAccess;

namespace SardCoreAPI.Models.Map.MapLayer
{
    public class MapLayerSearchCriteria : PagedSearchCriteria
    {
        public bool? IsBaseLayer { get; set; }
        public bool? IsIconLayer { get; set; }
        public int? MapId { get; set; }

        public ExpressionStarter<MapLayer> GetQuery()
        {
            return GetQuery<MapLayer>()
                .AndIf(Id != null, x => x.Id.Equals(Id))
                .AndIf(IsBaseLayer != null, x => x.IsBaseLayer.Equals(IsBaseLayer))
                .AndIf(IsIconLayer != null, x => x.IsIconLayer.Equals(IsIconLayer))
                .AndIf(MapId != null, x => x.MapId.Equals(MapId))
                .AndIf(Query != null, x => x.Name.Contains(Query ?? ""))
                .AndIf(true, x => true);
        }
    }
}
