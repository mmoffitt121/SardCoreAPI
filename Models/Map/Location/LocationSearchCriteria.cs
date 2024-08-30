using lt = SardCoreAPI.Models.Map.LocationType;
using SardCoreAPI.Models.Common;
using LinqKit;
using SardCoreAPI.Models.DataPoints;
using SardCoreAPI.Utility.DataAccess;

namespace SardCoreAPI.Models.Map.Location
{
    public class LocationSearchCriteria : PagedSearchCriteria
    {
        public List<int>? LocationTypeIds { get; set; }
        public List<int>? MapLayerIds { get; set; }
        public double? MinLatitude { get; set; }
        public double? MaxLatitude { get; set; }
        public double? MinLongitude { get; set; }
        public double? MaxLongitude { get; set; }
        public double? MinZoom { get; set; }
        public double? MaxZoom { get; set; }

        public ExpressionStarter<Location> GetQuery()
        {
            return GetQuery<Location>()
                .AndIf(Id != null, x => x.Id.Equals(Id))
                .AndIf(Query != null, x => x.Name.Contains(Query ?? ""))
                .AndIf(LocationTypeIds != null && LocationTypeIds.Count() > 0, x => LocationTypeIds!.Contains(x.LocationTypeId))
                .AndIf(MapLayerIds != null && MapLayerIds.Count() > 0, x => MapLayerIds!.Contains(x.LayerId))
                .AndIf(MinLatitude != null, x => x.Latitude > MinLatitude)
                .AndIf(MaxLatitude != null, x => x.Latitude < MaxLatitude)
                .AndIf(MinLongitude != null, x => x.Longitude > MinLongitude)
                .AndIf(MaxLongitude != null, x => x.Longitude < MaxLongitude)
                .AndIf(MinZoom != null, x => MinZoom >= (x.ZoomProminenceMin ?? x.LocationType.ZoomProminenceMin))
                .AndIf(MaxZoom != null, x => MaxZoom <= (x.ZoomProminenceMax ?? x.LocationType.ZoomProminenceMax))
                .AndIf(true, x => true);
        }
    }
}
