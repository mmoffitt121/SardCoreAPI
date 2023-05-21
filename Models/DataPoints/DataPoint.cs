using SardCoreAPI.Models.DataPoints.DataPointParameters;

namespace SardCoreAPI.Models.DataPoints
{
    public class DataPoint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        List<DataPointParameter> Parameters { get; set; }
    }
}
