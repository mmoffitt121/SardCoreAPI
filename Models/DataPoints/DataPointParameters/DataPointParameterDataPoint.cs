namespace SardCoreAPI.Models.DataPoints.DataPointParameters
{
    public class DataPointParameterDataPoint : DataPointParameter
    {
        public int DataPointValueId { get; set; }
        public DataPoint DataPointValue { get; set; }

        public override string GetStringValue()
        {
            return DataPointValueId.ToString();
        }
    }
}
