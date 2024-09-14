namespace SardCoreAPI.Models.DataPoints.DataPointParameters
{
    public class DataPointParameterDocument : DataPointParameter
    {
        public string DocumentValue { get; set; }

        public override string GetStringValue()
        {
            return DocumentValue.ToString();
        }
    }
}
