using SardCoreAPI.Models.DataPoints.DataPointParameters;
using SardCoreAPI.Models.Map.Location;

namespace SardCoreAPI.Models.DataPoints.Queried
{
    public class QueriedDataPoint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Settings { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string? TypeSummary { get; set; }
        public string? TypeSettings { get; set; }
        public List<QueriedDataPointParameter>? Parameters { get; set; }
        public List<Location>? Locations { get; set; }
    }
}
