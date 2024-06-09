namespace SardCoreAPI.Models.DataPoints.Queried
{
    public class QueriedDataPointParameter
    {
        public int TypeParameterId { get; set; }
        public string TypeParameterName { get; set; }
        public string? TypeParameterSummary { get; set; }
        public string TypeParameterTypeValue { get; set; }
        public int TypeParameterSequence { get; set; }
        public int DataPointTypeReferenceId { get; set; }
        public string TypeParameterSettings { get; set; }
        public string? Value { get; set; }
        public object? ValueData { get; set; }
    }
}
