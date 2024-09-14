using SardCoreAPI.Attributes.Easy;
using SardCoreAPI.Models.DataPoints.DataPointParameters;

namespace SardCoreAPI.Models.DataPoints
{
    [Table("DataPointTypes")]
    public class DataPointType
    {
        [Column]
        public int Id { get; set; }
        [Column]
        public string Name { get; set; }
        [Column]
        public string? Summary { get; set; }
        [Column]
        public string? Settings { get; set; }
        public List<DataPointTypeParameter>? TypeParameters { get; set; }
    }
}
