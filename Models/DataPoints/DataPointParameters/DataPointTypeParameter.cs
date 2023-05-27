namespace SardCoreAPI.Models.DataPoints.DataPointParameters
{
    public class DataPointTypeParameter
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string? Summary { get; set; }
        public int DataPointTypeId { get; set; }
        public string TypeValue { get; set; }
        public int Sequence { get; set; }
    }
}
