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
        public int? DataPointTypeReferenceId { get; set; }
        public string? Settings { get; set; }
        public bool? IsMultiple { get; set; }

        public string GetTable()
        {
            if (string.Equals(TypeValue, "bit")) { return "DataPointParameterBoolean"; }
            if (string.Equals(TypeValue, "dat")) { return "DataPointParameterDataPoint"; }
            if (string.Equals(TypeValue, "dub")) { return "DataPointParameterDouble"; }
            if (string.Equals(TypeValue, "int")) { return "DataPointParameterInt"; }
            if (string.Equals(TypeValue, "str")) { return "DataPointParameterString"; }
            if (string.Equals(TypeValue, "sum")) { return "DataPointParameterSummary"; }
            if (string.Equals(TypeValue, "uni")) { return "DataPointParameterUnit"; }
            if (string.Equals(TypeValue, "tim")) { return "DataPointParameterTime"; }
            return "DataPointParameterString";
        }
    }
}
