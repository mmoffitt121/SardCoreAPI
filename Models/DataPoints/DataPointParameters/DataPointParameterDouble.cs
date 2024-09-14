namespace SardCoreAPI.Models.DataPoints.DataPointParameters
{
    public class DataPointParameterDouble : DataPointParameter
    {
        public double? DoubleValue { get; set; }

        public override string GetStringValue()
        {
            return DoubleValue.ToString();
        }
    }
}
