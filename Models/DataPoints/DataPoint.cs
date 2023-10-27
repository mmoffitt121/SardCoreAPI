using SardCoreAPI.Models.DataPoints.DataPointParameters;
using System.Text.Json.Serialization;

namespace SardCoreAPI.Models.DataPoints
{
    public class DataPoint
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public List<DataPointParameter>? Parameters { get; set; }
    }
}
