namespace SardCoreAPI.Models.DataPoints.Queried
{
    public class FlatDataPointComponent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public string? Settings { get; set; }
        public string TypeName { get; set; }
        public string? TypeSummary { get; set; }
        public string TypeSettings { get; set; }
        public int TypeParameterId { get; set; }
        public string TypeParameterName { get; set; }
        public string? TypeParameterSummary { get; set; }
        public string TypeParameterTypeValue { get; set; }
        public int TypeParameterSequence { get; set; }
        public int DataPointTypeReferenceId { get; set; }
        public string TypeParameterSettings { get; set; }
        public string? Value { get; set; }
    }
}
