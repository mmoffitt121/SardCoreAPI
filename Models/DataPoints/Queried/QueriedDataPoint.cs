using SardCoreAPI.Models.DataPoints.DataPointParameters;

namespace SardCoreAPI.Models.DataPoints.Queried
{
    public class QueriedDataPoint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public List<DataPointParameter>? Parameters { get; set; }
    }
}
