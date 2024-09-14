using SardCoreAPI.Models.DataPoints.Queried;

namespace SardCoreAPI.Models.DataPoints.DataPointParameters
{
    public class DataPointParameterBoolean : DataPointParameter
    {
        public bool BoolValue { get; set; }

        public override string GetStringValue()
        {
            return BoolValue.ToString();
        }
    }
}
